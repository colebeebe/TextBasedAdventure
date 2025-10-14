namespace Tyrrion;

public class Room {
    public string Name { get; set; } = "";
    public string Identifier { get; set; } = "";
    public List<string> Objects { get; set; } = [];
    public List<string> Items { get; set; } = [];
    public string Description { get; set; } = "";
    public Dictionary<string, string> Actions { get; set; } = [];
    public List<Exit> Exits { get; set; } = [];

    public void Print() {
        Console.WriteLine($"Name: {Name}");
        Console.WriteLine($"Identifier: {Identifier}");
        Console.WriteLine($"Objects: {string.Join(", ", Objects ?? new())}");
        Console.WriteLine($"Items: {string.Join(", ", Items ?? new())}");
        Console.WriteLine($"Description: {Description}");

        Console.WriteLine("Actions:");
        if (Actions != null) {
            foreach (var action in Actions)
                Console.WriteLine($"  {action.Key}: {action.Value}");
        }

        Console.WriteLine("Exits:");
        if (Exits != null) {
            foreach (var exit in Exits) {
                Console.WriteLine($"  Direction: {exit.Direction}, Destination: {exit.Destination}");
                if (exit.Conditions != null) {
                    foreach (var cond in exit.Conditions) {
                        Console.WriteLine($"    Condition: {cond.Type}, Object: {cond.Object}, State: {cond.State}");
                    }
                }
            }
        }
    }
}

public class Exit {
    public Directions Direction { get; set; }
    public string Destination { get; set; } = "";
    public List<Condition> Conditions { get; set; } = [];
}

public class Condition {
    public ConditionType Type { get; set; }
    public string Object { get; set; } = "";
    public string State { get; set; } = "";
}

public enum ConditionType {
    ObjectState
}
