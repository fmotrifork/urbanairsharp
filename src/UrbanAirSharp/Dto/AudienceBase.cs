using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanAirSharp.Dto
{
	public class AudienceBase : IAudience
	{
		static IAudience Op<T>(IAudience a, IAudience init)
			where T : class, IAudienceCompounder, new()
		{
			if (a != null)
			{
				if (init is T)
				{
					var self = init as T;
					List<IAudience> list = self.Audiences.ToList();
					if (a is T)
					{
						var other = a as T;
						list.AddRange(other.Audiences);
					}
					else
						list.Add(a);

					self.Audiences = list;
				}
				else
					return new T { Audiences = new[] { init, a } };
			}
			return init;
		}

		static IAudience BatchOps(IEnumerable<IAudience> arr, IAudience init, Func<IAudience, IAudience> op)
		{
			IAudience res = init;
			if (arr != null)
			{
				arr = from a in arr where a != null select a;
				if (arr.Count() != 0)
				{
					foreach (IAudience a in arr)
					{
						if (a == null)
							continue;

						res = op(a);
					}
				}
			}
			return res;
		}

		#region NOT logic

		public AudienceNot Not()
		{
			return new AudienceNot { Audience = this };
		}

		public static AudienceNot operator !(AudienceBase a)
		{
			if (a != null)
				return a.Not();

			return null;
		}

		#endregion

		#region OR logic

		public IAudience Or(IAudience a)
		{
			return Op<AudienceOr>(a, this);
		}

		public IAudience Or(IEnumerable<IAudience> arr)
		{
			return BatchOps(arr, this, Or);
		}

		public static IAudience operator |(IAudience a, AudienceBase b)
		{
			if (b != null)
				return b.Or(a);

			return a;
		}

		//public static IAudience operator |(Audience a, AudienceBase b)
		//{

		//}

		#endregion

		#region AND logic

		public IAudience And(IAudience a)
		{
			return Op<AudienceAnd>(a, this);
		}

		public IAudience And(IEnumerable<IAudience> arr)
		{
			return BatchOps(arr, this, And);
		}

		public static IAudience operator &(IAudience a, AudienceBase b)
		{
			if (b != null)
				return b.And(a);

			return a;
        }

		//public static IAudience operator &(Audience a, AudienceBase b)
		//{
		//	if (a != null)
		//		return a.And(b);
		//	else if (b != null)
		//		return b.And(a);
		//	else
		//		return null;
		//}

		//public static IAudience operator &(IAudienceCompounder a, AudienceBase b)
		//{
		//	if (b != null)
		//		return b.And(a);

		//	return a;
		//}

		//public static IAudience operator &(AudienceBase a, AudienceBase b)
		//{
		//	if (a != null)
		//		return a.And(b);
		//	else if (b != null)
		//		return b.And(a);
		//	else
		//		return null;
		//}

		#endregion
	}
}
