// Copyright (c) 2014-2015 Jeff Gosling (jeffery.gosling@gmail.com)
using System;

namespace UrbanAirSharp.Type
{
	[Flags]
	public enum DeviceType : int
    {
		None = 0,
		Android = 1,
		Ios = 2,
		Wns = 4,
		Mpns = 8,
		Blackberry = 16,
		All = Android | Ios | Wns | Mpns | Blackberry,
	}
}
