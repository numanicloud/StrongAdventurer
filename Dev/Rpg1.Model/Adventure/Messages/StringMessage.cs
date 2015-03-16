using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpg1.Model.Adventure.Messages
{
	public class StringMessage : IMessage
	{
		public string Message { get; private set; }

		public StringMessage(string message)
		{
			Message = message;
		}
	}
}
