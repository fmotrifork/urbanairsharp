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
```
var client = new UrbanAirSharpGateway(AppKey, AppMasterSecret);
var myPhone = new Device("my-device-id", DeviceType.Ios);
var message = new Push("What's up", myPhone);
client.Push(message);

```
Here are some more examples of the supported functionality

    client.Validate("Validate push", new List<DeviceType>() { DeviceType.Android }, "946fdc3d-0284-468f-a2f7-d007ed694907"); 

	client.Push("Broadcast Alert");

	client.Push("Broadcast Alert to Androids", new List<DeviceType>() { DeviceType.Android });

	client.Push("Targeted Alert to device", new List<DeviceType>() { DeviceType.Android }, "946fdc3d-0284-468f-a2f7-d007ed694907");

This is an example of a more complicated, audience targeted Push
	
	client.Push("Custom Alert per device type", null, null, new List<BaseAlert>()
	{
		new AndroidAlert()
		{
			Alert = "Custom Android Alert",
			CollapseKey = "Collapse_Key",
			DelayWhileIdle = true,
			GcmTimeToLive = 5
		}
	});

	//these are just examples of tags
	var rugbyFanAudience = new Audience(AudienceType.Tag, "Rugby Fan");
	var footballFanAudience = new Audience(AudienceType.Tag, "Football Fan");
	var notFootballFanAudience = new Audience().NotAudience(footballFanAudience);
	var newZealandAudience = new Audience(AudienceType.Alias, "NZ");
	var englishAudience = new Audience(AudienceType.Tag, "language_en");

	var fansAudience = new Audience().OrAudience(new List<Audience>() { rugbyFanAudience, notFootballFanAudience });

	var customAudience = new Audience().AndAudience(new List<Audience>() { fansAudience, newZealandAudience, englishAudience });

	client.Push("English speaking New Zealand Rugby fans", null, null, null, customAudience);

# License
Copyright (c) 2014-2015 Jeff Gosling Licensed under the MIT license.

http://opensource.org/licenses/mit-license.php
