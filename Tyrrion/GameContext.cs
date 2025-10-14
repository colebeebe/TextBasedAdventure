using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tyrrion;

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

public class GameContext {

    private readonly string _resourcesDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)!,
        "../../../Resources/");

    public bool HasWon;
    private string _currentRoom;
    private readonly Dictionary<string, Room> _gameRooms;
    public GameContext() {
        HasWon = false;

        _gameRooms = [];
        var room = LoadRoom("northOfHouse");
        if (room != null) {
            _gameRooms.Add(room.Identifier, room);
        }
        room = LoadRoom("frontRoom");
        if (room != null) {
            _gameRooms.Add(room.Identifier, room);
        }

        _currentRoom = "northOfHouse";

        if (_gameRooms.TryGetValue(_currentRoom, out Room? value)) {
            var curr = value;
            Console.WriteLine();
            Console.WriteLine(curr.Name.ToUpper());
            Console.WriteLine();
            Console.WriteLine(curr.Description);
            Console.WriteLine();
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
            Console.WriteLine(curr.Description);
            Console.WriteLine();
        }
    }

    private Room? LoadRoom(string roomName) {
        var path = _resourcesDirectory + "Rooms/" + roomName + ".json";
        var json = File.ReadAllText(path);
        var options = new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        };
        options.Converters.Add(new JsonStringEnumConverter());
        Room? room = JsonSerializer.Deserialize<Room>(json, options);
        return room;
    }
}