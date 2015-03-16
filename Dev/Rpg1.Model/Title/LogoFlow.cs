using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Messages;

namespace Rpg1.Model.Title
{
	public class LogoFlow
	{
		public IEnumerable<IMessage> GetFlow()
		{
			yield return new WaitLogoFinished();

			var title = new TitleFlow();
			yield return new ChangeFlowMessage<object>(title.GetFlow(), null);
		}
	}
}
