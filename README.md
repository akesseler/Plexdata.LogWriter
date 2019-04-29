
## Plexdata Logging Writer

The _Plexdata Logging Writer_ is nothing else but yet another logging interface. However, there are some special features that other loggers don't support. 

One of those features is a more detailed logging level support. For example, it is possible to group issues by _Error_, _Fatal_ and _Critical_. Another feature that other loggers don't support is the integration of console logging. Usually, console logging takes place by using method ``System.Console.Write("...")`` with the effect that a _Winforms_ application for example is unable to write anything into a console window. This is because of such a window is never opened. Yet the console logger included in the package is able to do it by creating its own console window.

### Licensing

The software has been published under the terms of _MIT License_.

### Documentation

The documentation with an overview, an introduction as well as examples is available under [https://akesseler.github.io/Plexdata.LogWriter/](https://akesseler.github.io/Plexdata.LogWriter/).
