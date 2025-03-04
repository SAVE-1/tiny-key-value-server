# tiny-key-value-server
This is a Redis-like server POC with C# and HttpListenerContext

I'm using a single thread for incoming requests job queue to preserve determinism

# API and usage
```
GET /get?key=<key name>
- get a key:value

POST /set?key=<key name>&value=<value>
- set a key:value

GET /
- a hello root

GET /hello
- server responds with a hello

GET /<anything else>
- a 404 message

```
