using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ace;
using umw;

namespace Rpg1.Ace.Common
{
	class EasingComponent : Object2DComponent
	{
		int count, maxCount;
		float lastValue;
		float[] easing;
		Func<float> getter;
		Action<float> setter;
		Action callback;

		public EasingComponent(Func<float> getter, Action<float> setter, float goal, EasingStart start, EasingEnd end, int count, Action callback = null)
		{
			easing = Easing.GetEasingFunction(start, end);
			lastValue = goal;
			maxCount = count;

			this.getter = getter;
			this.setter = setter;
			this.callback = callback;
		}

		protected override void OnUpdate()
		{
			++count;
			var v = Easing.GetNextValue(getter(), lastValue, count, maxCount, easing);
			setter(v);
			if(count >= maxCount)
			{
				if(callback != null)
				{
					callback();
				}
				Vanish();
			}
		}

		public static EasingComponent CreateAlphaEasing(TextureObject2D obj, byte goal, EasingStart start, EasingEnd end, int count, Action callback = null)
		{
			return new EasingComponent(
				() => obj.Color.A,
				v => obj.Color = new Color(obj.Color.R, obj.Color.G, obj.Color.B, (byte)v),
				goal,
				start,
				end,
				count,
				callback);
		}

		public static EasingComponent CreateXEasing(Object2D obj, float goal, EasingStart start, EasingEnd end, int count, Action callback = null)
		{
			return new EasingComponent(
				() => obj.Position.X,
				v => obj.Position = new Vector2DF(v, obj.Position.Y),
				goal,
				start,
				end,
				count,
				callback);
		}

		public static EasingComponent CreateYEasing(Object2D obj, float goal, EasingStart start, EasingEnd end, int count, Action callback = null)
		{
			return new EasingComponent(
				() => obj.Position.Y,
				v => obj.Position = new Vector2DF(obj.Position.X, v),
				goal,
				start,
				end,
				count,
				callback);
		}
	}
}
