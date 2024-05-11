# 🚢 Ship Fleet Management Application 🚢

This C# console application is designed to manage a fleet of ships. It enables users to store ship information, track their positions and manage cargo loading for container ships, as well as refueling tanks installed on tanker ships.

## Table of Contents

- [Features](#features)
- [Usage](#usage)
- [Testing](#testing)
- [License](#license)

## Features

- **Shipowners**: Each shipowner maintains an individual list of ships.

- **Adding Ships**: Ships of two types can be added - Container Ships and Tanker Ships. Each ship is identified by a unique IMO number and has properties such as name, length, width, maximum load and current position.

- **Position History**: Ship positions can be modified, while maintaining a history of their previous positions along with the timestamp of each update.

- **Container Ships**: Container ships can load and unload containers. Each container comes with details like the sender, addressee, cargo description and weight. Additionally, container ships have a maximum load capacity and a specific limit on the number of containers they can carry, which can't be exceeded.

- **Tanker Ships**: Tanker ships can refuel various types of fuel into permanently installed tanks. Each tank has its own capacity in liters and can be filled with one type of fuel to its maximum capacity. Tankers also have a maximum permitted load which cannot be exceeded.

## Usage

To run the application navigate to the poject directory and execute:

```
dotnet run
```

Follow the on-screen instructions to perform various operations such as adding ships, updating positions, loading containers, and refueling tanks.

## Testing

Unit tests have been provided to ensure the correct operation of the application. To run the tests execute:

```
dotnet test
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
