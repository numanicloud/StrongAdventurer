using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ace;

namespace Rpg1.Ace.Common
{
	class TimerLayerComponent : Layer2DComponent
	{
		private int time { get; set; }
		private Action callback { get;set; }

		public TimerLayerComponent(int time, Action callback)
		{
			this.time = time;
			this.callback = callback;
		}

		protected override void OnUpdated()
		{
			time--;
			if(time == 0)
			{
				callback();
				Vanish();
			}
		}
	}
}
