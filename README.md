# Overview

The major goal for this project was to create a Zork-like text-based adventure game. The focus was placed majorly on creating a data-driven system. With the engine, one can place any number of JSON files in the Rooms, Items, and Objects folders and (provided the formatting is correct) the game will run without having to recompile.

Central to this design was the data-driven engine. The engine checks for files in the different directories and automatically instantiates objects in memory. The player can then interact with those objects in various ways. 

I found lambda functions to be rather interesting, so I decided to practice using them with the parser. When a sentence is parsed, it returns a lambda function that is then run. This way, I could pass along functions, edit variables, or do many other things given the context of the user request. It was a good exploration of the syntax surrounding lambdas in C#.

My main purpose for writing this was to become more familiar with C# and how it can work within a project. As mentioned before, I also wanted to practice a data-driven design structure, and I now understand the relationship from JSON to objects in memory much better.

[Software Demo Video](https://youtu.be/EMnjFofEby8)

# Development Environment

For this project, I mostly used Visual Studio 2022 and Visual Studio code.

.NET 8.0 is required to be able to run this program. Once the .NET SDKs and Runtimes are installed, there are several options. The project can be run from a C#-enabled IDE (Visual Studio, Rider, or even Visual Studio Code with C# extensions). Once the project folder is opened in the IDE, clicking the run button should either pull up a console or run it in the IDE's integrated terminal. The other option is using the .NET CLI (Command-Line Interface). After navigating to the project's main folder (the folder containing Tyrrion.csproj), run `dotnet run` and it should run the program directly in the terminal. Or, run `dotnet build` and take note of the build path. Navigating to the build path and double clicking the Tyrrion program should also launch the console and begin running the game.

As it stands, the game currently only supports referring to objects and items in the game by their identifier, which is often a camel-case version of its name (for example, the Battery-Powered Lantern becomes `batteryPoweredLantern`).

# Useful Websites

I only refrenced a few websites. Luckily, a while back there was an article written about how the parser worked in Zork, so I used that for a lot of inspiration. Whenever I got stuck on syntax I used Microsoft's C# Documentation pages.

- [Zork: The Great Inner Workings](https://medium.com/swlh/zork-the-great-inner-workings-b68012952bdc)
- [C# Documentation](https://learn.microsoft.com/en-us/dotnet/?view=net-8.0)

# Future Work

There is a lot to be done in the future with this project, a few of which are:

- Implement alternate names so that identifiers don't have to be used
- Implement conditional situations; doors must be opened, certain items must be held for things to happen, keys to doors, etc.
- Create a win condition
- Create a score system
- Implement a move counter
- Have data-driven enemies that can wander, or stay put in a designated room
- Implement weight system
- Implement health system
- Create many more rooms
- Introduce items that can hold other items (things like bags)
- Implement darkness ("You are likely to be eaten by a grue")
- Create a death state
- Implement save/restore features