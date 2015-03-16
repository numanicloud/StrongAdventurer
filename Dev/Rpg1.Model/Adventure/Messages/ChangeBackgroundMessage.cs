using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpg1.Model.Adventure.Messages
{
	public class ChangeBackgroundMessage : IMessage
	{
		public ChangeBackgroundMessage(string path)
		{
			ImagePath = path;
		}

		public string ImagePath { get; private set; }
	}
}
