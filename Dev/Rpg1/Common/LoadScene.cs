using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ace;
using Rpg1.Model;
using Rpg1.Model.Adventure;
using Rpg1.Model.Messages;

namespace Rpg1.Ace.Common
{
	class LoadScene : Scene, IChannelable
	{
		public Action Callback { get; set; }

		protected override void OnTransitionFinished()
		{
			if(Callback != null)
			{
				Callback();
			}
		}

		protected override void OnUpdateForTheFirstTime()
		{
			var layer = new Layer2D();
			AddLayer(layer);

			var obj = new TextureObject2D()
			{
				Texture = Engine.Graphics.CreateTexture2D("Texture/Title/Loading.png"),
				Position = Program.WindowSize / 2,
			};
			obj.SetCenterPosition(CenterPosition.CenterCenter);
            layer.AddObject(obj);
		}

		public void AddMessageHandlerTo(SteppingChannel<IMessage> channel)
		{
			channel.AddMessageHandler<ChangeFlowMessage<Model.Adventure.AdventureContext>>(ChangeToAdventure);
		}

		private void ChangeToAdventure(ChangeFlowMessage<AdventureContext> msg, Action callback)
		{
			var scene = new Adventure.Basic.AdventureScene(msg.Context);
			Program.ChangeFlow(msg.Flow, scene);
			Engine.ChangeSceneWithTransition(scene, new TransitionFade(1, 1));
		}
	}
}
