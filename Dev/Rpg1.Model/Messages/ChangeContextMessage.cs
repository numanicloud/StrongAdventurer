using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpg1.Model
{
	public class ChangeContextMessage<TContext> : IMessage
	{
		public TContext Context { get; private set; }

		public ChangeContextMessage(TContext context)
		{
			Context = context;
		}
	}
}
