namespace Tyrrion;

public class Room {
    public string Name { get; set; } = "";
    public string Identifier { get; set; } = "";
    public List<string> Objects { get; set; } = [];
    public List<string> Items { get; set; } = [];
    public string Description { get; set; } = "";
    public Dictionary<string, string> Actions { get; set; } = [];
    public List<Exit> Exits { get; set; } = [];

    public bool Visited { get; set; } = false;
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

public enum Directions {
    North,
    South, 
    East,
    West,
    Up,
    Down,
    Enter,
    Southeast,
    Southwest,
    Northeast,
    Northwest,
}