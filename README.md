# tiny-key-value-server
This is a Redis-like server POC with C# and HttpListenerContext

I'm using a single thread for incoming requests job queue to preserve determinism

# Design
## Functional requirements
- As a user, I wish to set key:values
- As a user, I wish retrieve key:values
- As a user, I want authentication

## Non-functional requirements
- As a user, I want a quick startup time
- As a user, I want to preserve temporal job queue order
	- For example, say you get incoming requests like this, we don't want update 
	  CreateOrSetKey(key = "mykey", value = 3) to happen before ReadKey(key = "mykey") due to threading
		- CreateOrSetKey(key = "mykey", value = 1)
		- ReadKey(key = "mykey")
		- CreateOrSetKey(key = "mykey", value = 3)

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
