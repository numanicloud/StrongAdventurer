using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ace;
using Rpg1.Ace.Common;
using Rpg1.Model;
using Rpg1.Model.Title;

namespace Rpg1.Ace.Title
{
	class TitleLayer : TitleSceneLayer
	{
		TextureObject2D logo { get; set; }

		public TitleLayer()
		{
			logo = new TextureObject2D()
			{
				Texture = Engine.Graphics.CreateTexture2D("Texture/Title/TitleLogo.png"),
				Position = new Vector2DF(-640, 20),
				DrawingPriority = 1
			};
			AddObject(logo);

			logo.AddComponent(
				new EasingComponent(
					() => logo.Position.X,
					v => logo.Position = new Vector2DF(v, 20),
					0,
					umw.EasingStart.StartRapidly2,
					umw.EasingEnd.EndSlowly3,
					60),
				"SlideIn");

			logo.AddComponent(
				new TimeAnimationComponent(
					t => logo.Position = new Vector2DF(logo.Position.X, 20 + (float)Math.Sin(t) * 5)),
				"LogoWave");
		}

		public override void AddMessageHandlerTo(SteppingChannel<IMessage> channel)
		{
			channel.AddMessageHandler<TitleMenuMessage, TitleMenuChoice>(InputMenu);
		}

		public override void Dispose()
		{
			logo.RemoveComponent("SliedIn");
			logo.RemoveComponent("LogoWave");
			logo.AddComponent(
				EasingComponent.CreateYEasing(
					logo,
					-logo.Texture.Size.Y,
					umw.EasingStart.StartRapidly3,
					umw.EasingEnd.EndSlowly3,
					30,
					Vanish),
				"SlideOut");
		}

		private void InputMenu(TitleMenuMessage msg, Action<TitleMenuChoice> callback)
		{
			AddObject(new TitleMenu(msg.Choices, callback));
		}
	}
}
