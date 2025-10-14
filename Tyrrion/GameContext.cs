using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tyrrion;

public class GameContext {

    private readonly string _resDir = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)!,
        "../../../Resources/");

    public bool HasWon;
    private string _currentRoom = "";
    private readonly Dictionary<string, Room> _gameRooms;
    private readonly Dictionary<string, Item> _gameItems;
    private readonly Dictionary<string, Object> _gameObjects;
    private readonly Dictionary<string, Item> _inventory;
    public GameContext() {
        HasWon = false;

        LoadSettings();

        _gameRooms = [];
        _gameItems = [];
        _gameObjects = [];
        _inventory = [];

        LoadAllRooms();
        LoadAllItems();
        LoadAllObjects();

        PrintRoom();

    }

    public void MoveRoom(Directions dir) {
        var curr = _gameRooms[_currentRoom];
        foreach (var exit in curr.Exits) {
            if (exit.Direction == dir) {
                if (_gameRooms.TryGetValue(exit.Destination, out Room? dest)) {
                    _currentRoom = dest.Identifier;
                }
            }
        }

        PrintRoom();
    }

    private void LoadAllRooms()
    {
        var path = _resDir + "Rooms";

        Debug.Assert(Directory.Exists(path), "Rooms directory does not exist.");

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        options.Converters.Add(new JsonStringEnumConverter());

        string[] files = Directory.GetFiles(path);
        foreach (var filepath in files)
        {
            try
            {
                string jsonContent = File.ReadAllText(filepath);
                JsonDocument.Parse(jsonContent);
                var room = JsonSerializer.Deserialize<Room>(jsonContent, options);
                if (room != null)
                {
                    _gameRooms.Add(room.Identifier, room);
                }
            }
            catch (JsonException)
            {
                Console.WriteLine($"INVALID JSON: {Path.GetFileName(filepath)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR reading file {Path.GetFileName(filepath)}: {ex.Message}");
            }
        }
    }

    private void LoadAllItems() {
        var path = _resDir + "Items";

        Debug.Assert(Directory.Exists(path), "Items directory does not exist.");

        var options = new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        };
        options.Converters.Add(new JsonStringEnumConverter());

        string[] files = Directory.GetFiles(path);
        foreach (var filepath in files) {
            try {
                string jsonContent = File.ReadAllText(filepath);
                JsonDocument.Parse(jsonContent);
                var item = JsonSerializer.Deserialize<Item>(jsonContent, options);
                if (item != null) {
                    _gameItems.Add(item.Identifier, item);
                }
            }
            catch (JsonException) {
                Console.WriteLine($"INVALID JSON: {Path.GetFileName(filepath)}");
            }
            catch (Exception ex) {
                Console.WriteLine($"ERROR reading file {Path.GetFileName(filepath)}: {ex.Message}");
            }
        }
    }

    private void LoadAllObjects() {
        var path = _resDir + "Objects";

        Debug.Assert(Directory.Exists(path), "Objects directory does not exist.");

        var options = new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        };
        options.Converters.Add(new JsonStringEnumConverter());

        string[] files = Directory.GetFiles(path);
        foreach (var filepath in files) {
            try {
                string jsonContent = File.ReadAllText(filepath);
                JsonDocument.Parse(jsonContent);
                var obj = JsonSerializer.Deserialize<Object>(jsonContent, options);
                if (obj != null) {
                    _gameObjects.Add(obj.Identifier, obj);
                }
            }
            catch (JsonException) {
                Console.WriteLine($"INVALID JSON: {Path.GetFileName(filepath)}");
            }
            catch (Exception ex) {
                Console.WriteLine($"ERROR reading file {Path.GetFileName(filepath)}: {ex.Message}");
            }
        }
    }
    
    private void LoadSettings() {
        var path = _resDir + "settings.json";

        Debug.Assert(File.Exists(path), "JSON file for settings does not exist.");

        try {
            string jsonContent = File.ReadAllText(path);
            var settings = JsonDocument.Parse(jsonContent).RootElement;
            if (settings.TryGetProperty("startRoom", out var startRoomElement))
            {
                var startRoom = startRoomElement.GetString();
                Debug.Assert(startRoom != null, "ERROR: No start room defined in settings.json.");
                _currentRoom = startRoom;
            }
        }
        catch (JsonException) {
            Console.WriteLine($"INVALID SETTINGS JSON: {Path.GetFileName(path)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR reading file {Path.GetFileName(path)}: {ex.Message}");
        }
    }

    public void PrintRoom(bool showDescription = false)
    {
        if (_gameRooms.TryGetValue(_currentRoom, out Room? value))
        {
            var curr = value;
            Console.WriteLine();
            Console.WriteLine(curr.Name.ToUpper());
            Console.WriteLine();
            if (!curr.Visited || showDescription)
            {
                var description = curr.Description;
                foreach (var obj in curr.Objects)
                {
                    if (_gameObjects.TryGetValue(obj, out Object? roomObject))
                    {
                        description += " " + roomObject?.Description;
                    }
                }
                foreach (var item in curr.Items)
                {
                    if (_gameItems.TryGetValue(item, out Item? roomItem))
                    {
                        description += " " + roomItem.Description;
                    }
                }
                WrapText(description);
                Console.WriteLine();
                curr.Visited = true;
            }
        }
    }
    
    public void LookAt(string name) {
        var room = _gameRooms[_currentRoom];
        if (room.Objects.Contains(name)) {
            if (_gameObjects.TryGetValue(name, out var obj)) {
                if (obj.Actions.TryGetValue("look", out var desc)) {
                    Console.WriteLine("\n" + desc + "\n");
                }
                else {
                    Console.WriteLine($"\nThere doesn't seem to be anything of note about the {obj.Name.ToLower()}\n");
                }
            }
            else {
                Debug.Assert(true, "ERROR: Object does not exist in objects folder.");
            }
        }
        else {
            Console.WriteLine("\nI don't seem to be able to see that here.\n");
        }
    }

    static void WrapText(string text) {
        int width = Console.WindowWidth - 1; // Subtract 1 for safety
        string[] words = text.Split(' ');
        int currentLineLength = 0;

        foreach (var word in words) {
            if (currentLineLength + word.Length + 1 > width) {
                Console.WriteLine();
                currentLineLength = 0;
            }

            if (currentLineLength > 0) {
                Console.Write(" ");
                currentLineLength++;
            }

            Console.Write(word);
            currentLineLength += word.Length;
        }

        Console.WriteLine();
    }

}

