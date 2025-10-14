namespace Tyrrion;

public class Object {
    public string Name { get; set; } = "";
    public string Identifier { get; set; } = "";
    public string Description { get; set; } = "";
    public Dictionary<string, string> Actions = [];
    public int Capacity = 0;
    public List<string> Alternatives = [];
    public List<string> States = [];
}
