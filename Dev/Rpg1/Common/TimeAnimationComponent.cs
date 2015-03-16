using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ace;

namespace Rpg1.Ace.Common
{
	class TimeAnimationComponent : Object2DComponent
	{
		float time;
		Action<float> setter;

		public TimeAnimationComponent(Action<float> setter)
		{
			this.setter = setter;
		}

		protected override void OnUpdate()
		{
			time += Engine.DeltaTime;
			setter(time);
		}
	}
}
