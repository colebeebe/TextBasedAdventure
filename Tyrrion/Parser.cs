using System.Reflection;

namespace Tyrrion;

public class Parser {

    private readonly string[] _responses;

    private readonly string[] _verbs = [
        "look",
        "examine",

        "take",
        "pocket",

        "go",
        "move",

        "yell",
        "say",
        "whisper",

        "open",

        "save",
        "load",

        "use",

        "inventory",

        "quit",
        "exit",
    ];

    private readonly string[] _directions = [
        "north",
        "south",
        "east",
        "west",
        "northeast",
        "northwest",
        "southeast",
        "southwest",
        "enter",
        "up",
        "down",
    ];

    public Parser() {
        // This path just makes sure that we always end up at the right place
        var path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)!,
        "../../../Resources/responses.txt");

        _responses = File.ReadAllLines(path);
    }

    public Action<GameContext> Parse(string[] words) {

        // Figure out verbs and other words by looking at the first few words and then removing them from the list. It would
        // probably also be useful to use Contains() to see if multiple words can be detected

        // If the first word is unrecognized, print a humorous message
        if (!_verbs.Contains(words[0].ToLower()) && !_directions.Contains(words[0].ToLower())) {
            Random random = new();
            var i = random.Next(_responses.Length);
            return context => Console.WriteLine("\n" + _responses[i] + "\n");
        }

        if (words[0].ToLower() == "quit") {
            if (words.Length == 1 || (words.Length == 2 && words[1].ToLower() == "game"))
            {
                return context => { context.HasWon = true; };
            }
        }

        if (words[0].ToLower() == "south")
        {
            return context => context.MoveRoom(Directions.South);
        }
        else if (words[0].ToLower() == "north")
        {
            return context => context.MoveRoom(Directions.North);
        }
        else if (words[0].ToLower() == "east")
        {
            return context => context.MoveRoom(Directions.East);
        }
        else if (words[0].ToLower() == "west")
        {
            return context => context.MoveRoom(Directions.West);
        }
        else if (words[0].ToLower() == "up")
        {
            return context => context.MoveRoom(Directions.Up);
        }
        else if (words[0].ToLower() == "down")
        {
            return context => context.MoveRoom(Directions.Down);
        }
        else if (words[0].ToLower() == "northeast")
        {
            return context => context.MoveRoom(Directions.Northeast);
        }
        else if (words[0].ToLower() == "northwest")
        {
            return context => context.MoveRoom(Directions.Northwest);
        }
        else if (words[0].ToLower() == "southeast")
        {
            return context => context.MoveRoom(Directions.Southeast);
        }
        else if (words[0].ToLower() == "southwest")
        {
            return context => context.MoveRoom(Directions.Southwest);
        }
        else if (words[0].ToLower() == "enter")
        {
            return context => context.MoveRoom(Directions.Enter);
        }
        else if (words[0].ToLower() == "exit")
        {
            return context => context.MoveRoom(Directions.Exit);
        }
        

        if (words[0].ToLower() == "look") {
            if (words.Length == 3 && words[1] == "at") {
                return context => context.LookAt(words[2]);
            }
            return context => context.PrintRoom(true);
        }

        return context => {
            foreach (string word in words) {
                Console.WriteLine(word);
            }

        };
    }
}