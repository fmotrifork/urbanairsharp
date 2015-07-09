// Copyright (c) 2014-2015 Jeff Gosling (jeffery.gosling@gmail.com)
using System;

namespace UrbanAirSharp.Type
{
	[Flags]
	public enum DeviceType : int
    {
		All,
		Android,
		Ios,
		Wns,
		Mpns,
		Blackberry,
	}
}
