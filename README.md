
# KarizmaConnection

**KarizmaConnection** is a lightweight abstraction layer built on top of SignalR for .NET Core, designed to eliminate "single hub overload" and simplify the management of requests, actions, and events within a SignalR-based system. It provides a modular and scalable structure, making it easier to handle multiple actions and events across different handlers.

## Features

- Modular handling of SignalR requests and events.
- Attribute-based routing for actions and events.
- Simplified client-server interaction using a single hub connection.

## Installation

To install the package:

```bash
dotnet add package KarizmaConnection
```

## Getting Started

### Setup

1. Define your handlers using attributes:
   ```csharp
   [Handler("user")]
   public class UserHandler
   {
       [Action("login")]
       public Task Login(string username, string password) { /* logic */ }
   }
   ```

2. Add KarizmaConnection to the service collection:
   ```csharp
   services.AddKarizmaConnection();
   ```

3. Initialize the KarizmaConnection hub in WebApplication:
   ```csharp
   app.MapKarizmaConnectionHub("/mainHub");
   ```

### Attributes
- **`[Handler]`**: Marks a class as a request handler.
- **`[Action]`**: Marks a method as an executable action.
- **`[Event]`**: Marks a method for handling hub events (e.g., `onConnected`, `onDisconnected`).

## Contributing

Feel free to contribute by submitting issues or pull requests!
