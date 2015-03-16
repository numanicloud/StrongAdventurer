using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpg1.Model.Title
{
	public class TitleMenuMessage : IResponse<TitleMenuChoice>
	{
		public TitleMenuChoice[] Choices { get; set; }
		public TitleMenuChoice Response { get; set; }

		public TitleMenuMessage(TitleMenuChoice[] choices)
		{
			Choices = choices;
		}
	}
}
