# Dynamic app start / stop PoC

## Usage

To build and start:
```
dotnet build
dotnet run
```
It will start listening on `localhost:5000`

Use following routes (all GETs) to start / stop applications:
```
/start/1
/stop/1
/start/2
/stop/2
```

* Apps register respectively `/App1/` and `/App2/` routes.
* Both apps also register `/main`, but it's ignored, because they can only handle requests starting with the app's name
* Both apps use `WordWritingHandler`, but they use different implementation of `IWordProvider`
* Apps register absolute routes, but it would be trivial to support only relative routes. That is, app's registration of `/main`, but would result in registration of `<app-name>/main`
* This example doesn't use MVC, because using MVC requires a lot of dependencies to be set up. In Starcounter we probably wouldn't want vanilla MVC anyway.
