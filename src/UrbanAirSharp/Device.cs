using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanAirSharp.Type;

namespace UrbanAirSharp
{
	public class Device
	{
		public Device(String id, DeviceType t)
		{
			Id = id;
			Type = t;
		}

		string _id;
		public String Id
		{
			get { return _id; }
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					throw new ArgumentException("Id can not be null or blank");

				_id = value;
			}
		}

		DeviceType _t;
		public DeviceType Type
		{
			get { return _t; }
			set
			{
				if (value == DeviceType.None)
					throw new ArgumentException("Type can not be None");

				_t = value;
			}
		}
	}
}
