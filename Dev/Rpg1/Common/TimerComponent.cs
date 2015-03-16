using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ace;

namespace Rpg1.Ace.Common
{
	class TimerComponent : Object2DComponent
	{
		private float time;
		private Action callback;

		public TimerComponent(float time, Action callback)
		{
			this.time = time;
			this.callback = callback;
		}

		protected override void OnUpdate()
		{
			time -= Engine.DeltaTime;
			if(time <= 0)
			{
				callback();
				Vanish();
			}
		}
	}
}
