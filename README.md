# DNS Caching Server

# Study project

[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://travis-ci.org/joemccann/dillinger)

DNS Caching Server  is a simple, windows form appliction, that allows to create single DNS server listener that works on udp.

  - Run app
  - Start server
  - DeSerialize data
  - Serialize data
  - Stop server

# New Features!

  - Start an Udp DNS Server 
  - Stop an Udp DNS Server

You can also:
  - Serialize data to configuration file
  - Deserialize data from configuration file

That's an amazing app

> This app gives you wide open spaces
> to create your own Dns Server

After starting server listening port 53 and resolving DNS questions with google 8.8.8.8 dns server if there no answer in cache
All resolved data are saving to cache
When ResourceRecord IsExpired server delete it from cache
Cache serializing when server stoppes
To Exit with saving cache firstly stop server


### Building and Staring

Desirable to have [Net.Core](https://dotnet.microsoft.com/download/dotnet-core/3.1) v3.1+ .

On CMD

Build an appliction in UdpTimeServer with.

```cmd
cd CacheDnsServer
dotnet build --configuration Release CacheDnsServer.sln
```

Go to Release folder

```cmd
 cd CacheDnsServer/bin/Release/netcoreapp3.1/
```
Run it
```cmd
 CacheDnsServer.exe
```

If configuration file in the same path with appliction data will be serialized!

![Configuration File Example](https://imgur.com/KMPEXep)
![Appliction iterface](https://imgur.com/mehP0Aq)
# Студент
Анисимов Андрей Александрович 

группа МО-201 / МЕН-282201
