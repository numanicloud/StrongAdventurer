using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ace;
using Rpg1.Ace.Common;

namespace Rpg1.Ace.Adventure
{
	class DamageIndicator : TextObject2D
	{
		private bool finished { get;set; }

		public DamageIndicator(Font font, int damage, Vector2DF position)
		{
			Text = damage.ToString();
			Position = position;
			Font = font;
		}

		protected override void OnStart()
		{
			var size = Font.CalcTextureSize(Text, WritingDirection);
			CenterPosition = new Vector2DF(size.X / 2, size.Y / 2);

			AddComponent(
				new EasingComponent(
					() => Position.Y,
					v => Position = new Vector2DF(Position.X, v),
					Position.Y - 30,
					umw.EasingStart.StartRapidly2,
					umw.EasingEnd.EndSlowly3,
					60,
					() => finished = true),
				"Easing");
		}

		protected override void OnUpdate()
		{
			if(finished)
			{
				Vanish();
			}
		}
	}
}
