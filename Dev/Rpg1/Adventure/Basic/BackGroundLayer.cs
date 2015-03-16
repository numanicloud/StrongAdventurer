using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ace;
using Rpg1.Ace.Common;
using Rpg1.Model;
using Rpg1.Model.Adventure;
using Rpg1.Model.Adventure.Messages;
using Rpg1.Model.Entities;

namespace Rpg1.Ace.Adventure.Basic
{
	class BackGroundLayer : Layer2D
	{
		private TextureObject2D background, nextBackground;

		public BackGroundLayer(AdventureContext context)
		{
			string path = null;
			switch(context.DayOrNight)
			{
			case DayOrNight.Morning:
				path = "Texture/Adventure/Back1.png";
				break;
			case DayOrNight.Noon:
				path = "Texture/Adventure/Back2.png";
				break;
			case DayOrNight.Evening:
				path = "Texture/Adventure/Back3.png";
				break;
			case DayOrNight.Night:
				path = "Texture/Adventure/Back4.png";
				break;
			}

			background = new TextureObject2D()
			{
				Texture = Engine.Graphics.CreateTexture2D(path),
				DrawingPriority = 0,
			};
			AddObject(background);
		}

		public void ChangeBackGround(Texture2D texture, Action callback)
		{
			nextBackground = new TextureObject2D()
			{
				Texture = texture,
				Color = new Color(255, 255, 255, 0),
				DrawingPriority = 0,
			};
			AddObject(nextBackground);

			nextBackground.AddComponent(
				new EasingComponent(
					() => nextBackground.Color.A,
					v => nextBackground.Color = new Color(255, 255, 255, (byte)v),
					255,
					umw.EasingStart.Start,
					umw.EasingEnd.End,
					40,
					() =>
			{
				background.Vanish();
				background = nextBackground;
				nextBackground = null;
				callback();
			}),
				"BackgroundsFadein");
		}
	}
}
