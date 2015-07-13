﻿using System;
using UrbanAirSharp.Dto;
using UrbanAirSharp.Request.Base;
using UrbanAirSharp.Response;

namespace UrbanAirSharp.Request
{
    /// <summary>
    /// Allows you to Register a new Device Token with Urbanairship
    /// </summary>
    /// <see cref="http://docs.urbanairship.com/reference/api/v3/registration.html"/>
	public class DeviceTokenRequest : PutRequest<BaseResponse, DeviceToken>
    {
        public DeviceTokenRequest(DeviceToken device, ServiceModelConfig cfg) : base(device, cfg)
        {
            if (string.IsNullOrEmpty(device.Token))
                throw new ArgumentException("The device tokens Token field is required", "device");

            RequestUrl = string.Format("api/device_tokens/{0}", device.Token);
        }
    }
}
