namespace Tyrrion;

public class Item {
    public string Name { get; set; } = "";
    public string Identifier { get; set; } = "";
    public string Description { get; set; } = "";
    public int Capacity { get; set; } = 0;
    public int Size { get; set; } = 1;
    public List<string> Alternatives = [];
    public Dictionary<string, string> Actions = [];
    public List<string> Items = [];
}

