
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
- 