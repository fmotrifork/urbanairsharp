using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanAirSharp.Dto
{
	public static class DtoExtensions
	{
		public static IAudience And(this IAudience a, IAudience b)
		{
			if (a == null)
				throw new ArgumentNullException("a");

			if (a is AudienceBase)
				return (a as AudienceBase).And(b);

			throw new InvalidOperationException("Type is not supported: " + a.GetType().FullName);
		}

		public static IAudience Or(this IAudience a, IAudience b)
		{
			if (a == null)
				throw new ArgumentNullException("a");

			if (a is AudienceBase)
				return (a as AudienceBase).Or(b);

			throw new InvalidOperationException("Type is not supported: " + a.GetType().FullName);
		}

		public static AudienceNot Not(this IAudience a)
		{
			if (a == null)
				return null;

			if (a is AudienceBase)
				return (a as AudienceBase).Not();

			throw new InvalidOperationException("Type is not supported: " + a.GetType().FullName);
		}
	}
}
