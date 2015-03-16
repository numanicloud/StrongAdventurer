using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpg1.Model.Adventure.Messages
{
	public class PlayBgmMessage : IMessage
	{
		public PlayBgmMessage(string bgmPath)
		{
			BgmPath = bgmPath;
		}

		public string BgmPath { get; private set; }
	}
}
