namespace Tyrrion;

public class Game {

    public static void Run() {
        PrintHeader();
        // Create the game context and parser
        var context = new GameContext();
        var parser = new Parser();

        // Main game loop
        while (!context.HasWon) {
            Console.Write("> ");
            string? input = Console.ReadLine();


            if (input != null) {
                string[] words = input.Split();
                var action = parser.Parse(words);
                action.Invoke(context);
            }
        }
    }

    private static void PrintHeader() {
        Console.WriteLine(" _______            _");
        Console.WriteLine("|__   __|Welcome to(_)");
        Console.WriteLine("   | |_   _ _ _ _ _ _  ___  _ __");
        Console.WriteLine("   | | | | | '_/ '_/ |/ _ \\| '_ \\");
        Console.WriteLine("   | | |_| | | | | | | (_) | | | |");
        Console.WriteLine("   |_|\\__, |_| |_| |_|\\___/|_| |_|");
        Console.WriteLine("       __/ |    A game by");
        Console.WriteLine("      |___/          outta_space");
        Console.WriteLine();
    }
}