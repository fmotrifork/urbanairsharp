UrbanAirSharp [![Build status](https://ci.appveyor.com/api/projects/status/e9e6cy3p299h1wi6?svg=true)](https://ci.appveyor.com/project/ronin1/urbanairsharp)
=============

UrbanAirSharp is a C# .Net library to simplify server-side calls to the Urban Airship API Version 3.
http://docs.urbanairship.com/reference/api/v3/

Urban Airship unifies and simplifies the sending of push notifications to mobile devices.
http://urbanairship.com/

For the latest *bleeding edge* nuget packages, please add this url to your nuget path:
https://ci.appveyor.com/nuget/urbanairsharp

# API Support

The following API functionality is currently supported

- Push
- Validate
- Schedule
- Device Tokens
- Tags

Support for the following still needs to be added

- Feeds
- Reports
- Segments
- Location

# Getting Started

The source includes a test project which has examples on the supported functionality of the library

Using the library is very easy. Simply supply your UA keys and call Push
```csharp
var client = new UrbanAirSharpGateway(AppKey, AppMasterSecret);
var myPhone = new Device("my-device-id", DeviceType.Ios);
var message = new Push("What's up", myPhone);
client.Push(message);
```

Alternatively, you can push using UA's proprietary ChannelID introduced in v3 of their API instead of direct device id like so:
```csharp
var client = new UrbanAirSharpGateway(AppKey, AppMasterSecret);
var myPhone = new Device("ua-channel-id", DeviceType.Ios, true);
var message = new Push("What's up", myPhone);
client.Push(message);
```

Note that you can store the keys in your web config or environment variables and use the default constructor to instanciate clients like so:
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
	<appSettings>
		<add key="UrbanAirSharp.uaAppKey" value="your app key"/>
		<add key="UrbanAirSharp.uaAppMAsterSecret" value="your app secret"/>

		<!-- incase you need to force all requests through a traffic inspector -->
		<add key="UrbanAirSharp.host" value="https://custom.ua/api-path"/> 
	</appSettings>
</configuration>
```
```cmd
setx UrbanAirSharp.uaAppKey "your app key" /m
setx UrbanAirSharp.uaAppMAsterSecret "your app secret" /m

@rem incase you need to force all requests through a traffic inspector
setx UrbanAirSharp.uaAppMAsterSecret "https://custom.ua/api-path" /m
```
```csharp
var client = new UrbanAirSharpGateway();
```

Here are some more examples of the supported functionality
```csharp
var drivingCars = new Audience(AudienceType.Segment, "automotive")
	.And(new Audience(AudienceType.Tag, "running"))
	.And(new Audience(AudienceType.Tag, "street"))
	.And(new Audience(AudienceType.Tag, "parked").Not());

client.Validate(new Push("Honk horns", drivingCars));
```

Which is the same as
```csharp
var drivingCars = new Audience(AudienceType.Segment, "automotive") &
	new Audience(AudienceType.Tag, "running") &
	new Audience(AudienceType.Tag, "street") &
	!new Audience(AudienceType.Tag, "parked");

client.Validate(new Push("Honk horns", drivingCars));
```

More use cases for push
```csharp
client.Push(new Push("Broadcast Alert")); //push to everyone

client.Push(new Push("Push to all Androids") { DeviceTypes = new[] { DeviceType.Android } });

client.Push(new Push(
				new Audience(AudienceType.Ios, "iphone-6-abc") |
				new Audience(AudienceType.Ios, "ipad-1-xyz") |
				new Audience(AudienceType.Windows, "workstation-123"),
				"Every device own by a User"));
```

This is an example of a more complicated, audience targeted Push
```csharp
client.Push(new Push(new[]
{
	new AndroidAlert()
	{
		Alert = "Custom Android Alert",
		CollapseKey = "Collapse_Key",
		DelayWhileIdle = true,
		GcmTimeToLive = 5
	}
}, "Custom Android Alert per device type"));
```

# License
Copyright (c) 2014-2015 Jeff Gosling Licensed under the MIT license.

http://opensource.org/licenses/mit-license.php
