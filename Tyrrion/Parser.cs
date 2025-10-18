using System.Reflection;

namespace Tyrrion;

public class Parser {

    private readonly string[] _responses;

    private readonly string[] _verbs = [
        "look",
        "examine",
        
        "read",

        "take",
        "pocket",
        
        "drop",
        "leave",

        "go",
        "move",

        "yell",
        "scream",
        "say",
        "whisper",

        "open",

        "save",
        "load",

        "use",

        "inventory",
        "i",

        "quit",
    ];

    private readonly Dictionary<string, Directions> _directions = new() {
        {"north", Directions.North},
        {"south", Directions.South},
        {"east", Directions.East},
        {"west", Directions.West},
        {"northeast", Directions.Northeast},
        {"northwest", Directions.Northwest},
        {"southeast", Directions.Southeast},
        {"southwest", Directions.Southwest},
        {"enter", Directions.Enter},
        {"exit", Directions.Exit},
        {"up", Directions.Up},
        {"down", Directions.Down},
    };

    public Parser() {
        // This path just makes sure that we always end up at the right place
        var path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)!,
        "../../../Resources/responses.txt");

        _responses = File.ReadAllLines(path);
    }

    public Action<GameContext> Parse(List<string> words) {

        // Figure out verbs and other words by looking at the first few words and then removing them from the list. It would
        // probably also be useful to use Contains() to see if multiple words can be detected
        
        // Get the verb and remove it from the words list
        var verb = words[0].ToLower();
        words.RemoveAt(0);

        // If the user just put the direction with no verb, let them move
        if (_directions.TryGetValue(verb, out var dir)) {
            return context => context.MoveRoom(dir);
        }
        
        // If the verb is unrecognized, print a message to the user
        if (!_verbs.Contains(verb)) return context => {
            var random = new Random();
            var i = random.Next(_responses.Length);
            Console.WriteLine("\n" + _responses[i] + "\n");
        };

        if (verb == "go" || verb == "move") {
            var direction = words[0].ToLower();
            words.RemoveAt(0);
            if (_directions.TryGetValue(direction, out dir)) {
                return context => context.MoveRoom(dir);
            }
            return context => Console.WriteLine($"\nI don't understand the word '{direction}'.\n");
        }

        if (verb == "i" || verb == "inventory") {
            return context => context.ShowInventory();
        }

        if (verb == "take" || verb == "pocket") {
            if (words.Count == 0) {
                return context => Console.WriteLine("\nWhat would you like to take?\n");
            }
            return context => context.Take(words[0]);
        }

        if (verb == "drop") {
            if (words.Count == 0) {
                return context => Console.WriteLine("\nWhat would you like to drop?\n");
            }
            return context => context.Drop(words[0]);
        }

        if (verb == "yell" || verb == "scream") {
            if (words.Count == 0) {
                return context => Console.WriteLine("\nYou scream out as loud as you can.\n");
            }

            var yellString = "";
            foreach (var word in words) {
                yellString += " ";
                yellString += word.ToUpper();
            }

            return context => Console.WriteLine($"\nYou scream out:{yellString}!\n");
        }

        if (verb == "whisper") {
            if (words.Count == 0) {
                return context => Console.WriteLine("\nYou mutter under your breath.\n");
            }

            var whisperString = "";
            foreach (var word in words) {
                whisperString += " ";
                whisperString += word;
            }

            return context => Console.WriteLine($"\nYou whisper:{whisperString}");
        }

        if (verb == "say") {
            if (words.Count == 0) {
                return context => Console.WriteLine("\nWhat would you like to say?\n");
            }

            var sayString = "";
            foreach (var word in words) {
                sayString += " ";
                sayString += word;
            }

            return context => Console.WriteLine($"\nYou say: {sayString}\n");
        }

        if (verb == "quit") {
            if (words.Count == 0 || (words.Count == 1 && words[0].ToLower() == "game"))
            {
                // For right now, we just trigger the win state to exit the game
                return context => { context.HasWon = true; };
            }
        }
        
        if (verb == "look") {
            if (words.Count == 2 && words[0] == "at") {
                return context => context.LookAt(words[1]);
            }
            return context => context.PrintRoom(true);
        }

        if (verb == "read") {
            if (words.Count == 0) {
                return context => Console.WriteLine("\nWhat would you like to read?\n");
            }

            return context => context.Read(words[0]);
        }

        return context => {
            foreach (string word in words) {
                Console.WriteLine(word);
            }

        };
    }
}