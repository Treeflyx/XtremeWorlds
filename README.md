![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat-square&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-13.0-239120?style=flat-square&logo=c-sharp)
![License](https://img.shields.io/github/license/Treeflyx/XtremeWorlds?style=flat-square)
![GitHub contributors](https://img.shields.io/github/contributors/Treeflyx/XtremeWorlds?style=flat-square)
![GitHub downloads](https://img.shields.io/github/downloads/Treeflyx/XtremeWorlds/total?style=flat-square)

# XtremeWorlds Game Engine

A simple 2D MMORPG game engine written in C# (.NET 9.0).

Based on the Orion+ conversion and MirageBasic frameworks.

## Overview

XtremeWorlds is a tile-based 2D MMORPG game engine designed for ease of use and
rapid development. The engine provides both client and server applications with
an intuitive GUI and built-in live editing features that enable seamless 
collaborative development.

## Game Features

- Basic Character Creation/Class Selection
- Movement/Attacking
- NPC/Computer Characters for attacking
- Items & Spells
- Event System

## Creation Features

The client has editors for the world (maps), items, spells, animations, NPCs, and more from the in-game admin panel.

### How do I access the editors?

Log in to the game with the client. On the server, type the command /access name 5 to promote yourself to owner. Now, go back to the client and tap Insert for each of the editor options.

## Quick Start

### Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [PostgreSQL](https://www.postgresql.org/download/)
- Git

### Installation

#### 1. Clone the repository
   ```bash
   git clone https://github.com/Treeflyx/XtremeWorlds.git
   cd XtremeWorlds
   ```

#### 2. Set up PostgreSQL

- Install [PostgreSQL](https://www.postgresql.org/download/)
- Create a database with password: `mirage`
- *Note: You can modify database credentials in the server settings JSON file*

#### 3. Build the solution
   ```bash
   dotnet build
   ```

#### 4. Run the applications

- Start the server application first
- Launch the client application
- They will connect automatically using default settings

## Support & Community

- **Discord**: [Join our community](https://discord.gg/ARYaWbN6b2)
- **Issues**: Report bugs and request features through GitHub Issues
- **Updates**: Check releases for the latest improvements and features

## License

See the [LICENSE](LICENSE) file for details.
