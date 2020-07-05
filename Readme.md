**Plug existing aggregates into NServiceBus sagas**  
The code samples for [this](https://medium.com/@mohsen.bazmi/plug-your-existing-domain-models-into-nservicebus-sagas-46415dbd25b2) article.  


**Prerequisites**
- Docker
- Docker Compose
- Dotnet Core 3.1

**How to run this**

In one terminal run these commands:  
`cd Environment`  
`docker-compose up`

In another terminal  
`cd WeApi`  
`dotnet run`

And in yet another terminal  
`cd EndPoint`  
`dotnet run`

**How to test it**

To Register a new random user open the developer tools in your browser and fetch the following link:
```js
fetch("https://localhost:5001/User/YourUserName",{method:'POST'}).then(res=>res.json()).then(console.log);
```
Then to see the domain event reaction:
```js
fetch("https://localhost:5001/User").then(res=>res.json()).then(console.log);
```
