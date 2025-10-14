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
    public GameContext() {
        HasWon = false;

        _gameRooms = [];
        LoadAllRooms();
        LoadSettings();

        if (_gameRooms.TryGetValue(_currentRoom, out Room? value)) {
            var curr = value;
            Console.WriteLine();
            Console.WriteLine(curr.Name.ToUpper());
            Console.WriteLine();
            if (!curr.Visited) {
                Console.WriteLine(curr.Description);
                Console.WriteLine();
                curr.Visited = true;
            }
        }
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
        if (_gameRooms.TryGetValue(_currentRoom, out Room? value)) {
            curr = value;
            Console.WriteLine();
            Console.WriteLine(curr.Name.ToUpper());
            Console.WriteLine();
            if (!curr.Visited) {
                Console.WriteLine(curr.Description);
                Console.WriteLine();
                curr.Visited = true;
            }
        }
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
}