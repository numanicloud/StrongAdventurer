using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ace;
using Rpg1.Ace.Common;

namespace Rpg1.Ace.Title
{
	class TitleBackgroundLayer : Layer2D
	{
		public TitleBackgroundLayer()
		{
			var back = new TextureObject2D()
			{
				Texture = Engine.Graphics.CreateTexture2D("Texture/Title/TitleBack.png"),
				Position = new Vector2DF(0, 0),
				DrawingPriority = -1
			};
			AddObject(back);

			var magicCircle = new TextureObject2D()
			{
				Texture = Engine.Graphics.CreateTexture2D("Texture/Title/MagicCircle.png"),
				Position = new Vector2DF(180, Program.WindowSize.Y / 2),
				DrawingPriority = 0,
				Color = new Color(255, 255, 255, 240),
				AlphaBlend = AlphaBlendMode.Mul,
				TextureFilterType = TextureFilterType.Linear,
			};
			magicCircle.SetCenterPosition(CenterPosition.CenterCenter);
			AddObject(magicCircle);

			magicCircle.AddComponent(new TimeAnimationComponent(t => magicCircle.Angle = -t * 5), "Rotation");
		}
	}
}
