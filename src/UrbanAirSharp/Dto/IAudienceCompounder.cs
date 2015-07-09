using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanAirSharp.Dto
{
	public interface IAudienceCompounder : IAudience
	{
		IEnumerable<IAudience> Audiences { get; set; }
	}
}
