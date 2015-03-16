using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model;

namespace Rpg1.Ace
{
	interface IChannelable
	{
		void AddMessageHandlerTo(SteppingChannel<IMessage> channel);
	}
}
