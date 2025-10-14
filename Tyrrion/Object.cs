namespace Tyrrion;

public class Object {
    public string Name { get; set; } = "";
    public string Identifier { get; set; } = "";
    public string Description { get; set; } = "";
    public Dictionary<string, string> Actions { get; set; } = [];
    public int Capacity { get; set; } = 0;
    public List<string> Alternatives { get; set; } = [];
    public List<string> States { get; set; } = [];
}
