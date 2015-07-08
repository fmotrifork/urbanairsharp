﻿// Copyright (c) 2014-2015 Jeff Gosling (jeffery.gosling@gmail.com)
using System;
using UrbanAirSharp.Request.Base;
using UrbanAirSharp.Response;

namespace UrbanAirSharp.Request
{
	/// <summary>
	/// Used to form a SCHEDULE request
	/// http://docs.urbanairship.com/reference/api/v3/schedule.html#schedule-a-notification
	/// </summary>
	public class ScheduleGetRequest : GetRequest<ScheduleGetResponse>
	{
		public ScheduleGetRequest(Guid scheduleId, ServiceModelConfig cfg) : base(cfg)
		{
			RequestUrl = "api/schedules/" + scheduleId + "/";
		}
	}
}
