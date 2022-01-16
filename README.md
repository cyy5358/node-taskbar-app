# node-taskbar-app
###### Getting or focusing app on taskbar with c#.

- [node-taskbar-app](#node-taskbar-app)
          - [Getting or focusing app on taskbar with c#.](#getting-or-focusing-app-on-taskbar-with-c)
  - [Installation](#installation)
    - [Supported Platforms](#supported-platforms)
  - [Usage](#usage)
  - [Contributing](#contributing)

## Installation

Requires Node 4+

```
    npm install taskbar-process
```

This module is __not supported__ in browsers.

### Supported Platforms

Currently, this module is only supported on Windows, and uses a .NET console app to manage windows.

Pull requests are welcome - it would be great to have this API work cross-platform.

## Usage

1) Get active processes

```javascript
    var processWindows = require("taskbar-process");

    var processesOnTaskbar = processWindows.getTaskbarProcesses(function(err, processes) {
        processes.forEach(function (p) {
            console.log("PID: " + p.pid.toString());
            console.log("MainWindowTitle: " + p.mainWindowTitle);
            console.log("ProcessName: " + p.processName);
        });
    });
```

2) Focus a window

```javascript
    var processWindows = require("taskbar-process");

    // Focus window by process...
    var processesOnTaskbar = processWindows.getTaskbarProcesses(function(err, processes) {
        var chromeProcesses = processes.filter(p => p.processName.indexOf("chrome") >= 0);

        // If there is a chrome process active, focus the first window
        if(chromeProcesses.length > 0) {
            processWindows.focusWindow(chromeProcesses[0]);
        }
    });

    // Or focus by name
    processWindows.focusWindow("chrome");
```


## Contributing

Pull requests are welcome

