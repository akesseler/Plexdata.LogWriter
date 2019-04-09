
## Plexdata.LogWriter.Console.Windows

This library implements the ``IConsoleLogger`` interface that allows printing 
of logging messages into the console window.

Main feature of this library is that NOT only pure console applications are 
able to write logging messages into the console window. For example when using 
an old-fashioned Windows Forms application an extra window is opened that shows 
all logging messages. 

Unfortunately, Visual Studio prints all message for a non-console application 
into its own output window. This in turn means that all logging message are lost 
when running such a program as release and without having a Debugger. Therefore, 
this library ensures that all logging messages are put into the console window by 
bypassing the output behavior of Visual Studio.
