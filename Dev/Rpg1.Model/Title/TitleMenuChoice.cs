using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpg1.Model.Title
{
	public class TitleMenuChoice
	{
		public string ImagePath { get; set; }
		public bool IsEnabled { get; set; }
		public Func<IEnumerable<IMessage>> GetFlow { get; set; }

		public TitleMenuChoice()
		{
			IsEnabled = true;
		}
	}
}
