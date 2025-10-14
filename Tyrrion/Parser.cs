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
    ];

    public Parser() {
        // This path just makes sure that we always end up at the right place
        var path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)!,
        "../../../Resources/responses.txt");

        _responses = File.ReadAllLines(path);
    }

    public Action<GameContext> Parse(string[] words) {

        if (words[0].ToLower() == "quit" || words[0].ToLower() == "exit") {
            if (words.Length == 1 || (words.Length == 2 && words[1].ToLower() == "game")) {
                return context => { context.HasWon = true; };
            }
        }

        // Figure out verbs and other words by looking at the first few words and then removing them from the list. It would
        // probably also be useful to use Contains() to see if multiple words can be detected

        if (words[0].ToLower() == "south") {
            return context => context.MoveRoom(Directions.South);
        }
        else if (words[0].ToLower() == "north") {
            return context => context.MoveRoom(Directions.North);
        }

        if (!_verbs.Contains(words[0].ToLower())) {
            Random random = new();
            var i = random.Next(_responses.Length);
            return context => Console.WriteLine(_responses[i]);
        }

        if (words[0].ToLower() == "look") {
            return context => context.PrintRoom(true);
        }

        return context => {
            foreach (string word in words) {
                Console.WriteLine(word);
            }

        };
    }
}