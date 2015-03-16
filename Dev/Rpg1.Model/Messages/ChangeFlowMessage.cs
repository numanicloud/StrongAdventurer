using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpg1.Model.Messages
{
	public class ChangeFlowMessage<TContext> : IMessage
	{
		public IEnumerable<IMessage> Flow { get; private set; }
		public TContext Context { get; private set; }

		public ChangeFlowMessage(IEnumerable<IMessage> flow, TContext context)
		{
			Flow = flow;
			Context = context;
		}
	}
}
