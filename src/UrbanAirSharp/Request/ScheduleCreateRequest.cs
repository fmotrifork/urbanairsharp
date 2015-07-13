﻿// Copyright (c) 2014-2015 Jeff Gosling (jeffery.gosling@gmail.com)

using UrbanAirSharp.Dto;
using UrbanAirSharp.Request.Base;
using UrbanAirSharp.Response;

namespace UrbanAirSharp.Request
{
	/// <summary>
	/// Used to form a SCHEDULE request
	/// http://docs.urbanairship.com/reference/api/v3/schedule.html#schedule-a-notification
	/// </summary>
	public class ScheduleCreateRequest : PostRequest<ScheduleCreateResponse, Schedule>
	{
		public ScheduleCreateRequest(Schedule schedule, ServiceModelConfig cfg)
			: base(schedule, cfg)
		{
			RequestUrl = "api/schedules/";
		}
	}
}
