# tiny-key-value-server
This is a Redis-like server POC with C# and HttpListenerContext

I'm using a single thread for incoming requests job queue to preserve determinism

# API and usage
```
GET /key?key=<key name>
- Get a key:value

POST /key?key=<key name>&value=<value>
- Set a key:value

DELETE /key?key=<key name>
- Not yet implemented

GET /
- A hello root

GET /hello
- Server responds with a hello

GET /<anything else>
- A 404 message

```

# How to run
```
dotnet run -p <your port>

or

dotnet run --port <your port>
```
