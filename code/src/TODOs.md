
**TODOs**

- For the persistent logger:
  - Support and expand environment variables within the filename, and
  - Try creating non-existing directories in the filename.
- Implement IEventLogger
  - A logger that writes into the event log (a distinction between windows and others is possibly necessary).
- Implement IHttpLogger
- Implement IMultiLogger
  - A logger that combines a set of all other loggers.
- Migrating the libraries to .NET Standard v2.1 as soon as it is released.
