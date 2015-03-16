using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ace;
using Rpg1.Ace.Common;
using Rpg1.Model;
using Rpg1.Model.Messages;
using Rpg1.Model.Title;

namespace Rpg1.Ace.Title
{
	class LogoScene : Scene, IChannelable
	{
		TextureObject2D timerObject;

		public LogoScene()
		{
			var layer = new Layer2D();
			AddLayer(layer);

			var logo = new TextureObject2D()
			{
				Texture = Engine.Graphics.CreateTexture2D("Texture/Title/LogoScreen.png"),
				Position = Program.WindowSize / 2,
			};
			logo.SetCenterPosition(CenterPosition.CenterCenter);
			layer.AddObject(logo);

			timerObject = new TextureObject2D()
			{
				IsDrawn = false,
			};
			layer.AddObject(timerObject);
		}

		public void AddMessageHandlerTo(SteppingChannel<IMessage> channel)
		{
			channel.AddMessageHandler<WaitLogoFinished>(WaitLogo);
			channel.AddMessageHandler<ChangeFlowMessage<object>>(GoToTitle);
		}

		private void GoToTitle(ChangeFlowMessage<object> msg, Action callback)
		{
			var scene = new TitleScene();
			Program.ChangeFlow(msg.Flow, scene);
			Engine.ChangeSceneWithTransition(scene, new TransitionFade(1, 1));
		}

		private void WaitLogo(WaitLogoFinished msg, Action callback)
		{
			timerObject.AddComponent(new TimerComponent(2, callback), "Timer");
		}
	}
}
