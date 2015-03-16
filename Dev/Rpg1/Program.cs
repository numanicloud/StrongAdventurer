using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ace;
using Rpg1.Model;

namespace Rpg1.Ace
{
	class Program
	{
		public static readonly Vector2DF WindowSize = new Vector2DF(640, 480);
		public static readonly Keys DecideKey = Keys.Z;

		public static SteppingChannel<IMessage> CurrentChannel { get; set; }
		public static bool IsFinished { get; set; }

		public static void ChangeFlow(IEnumerable<IMessage> flow, params IChannelable[] channelables)
		{
			var channel = new SteppingChannel<IMessage>(flow.GetEnumerator());
			channelables.ForEach(x => x.AddMessageHandlerTo(channel));
			channel.AddMessageHandler<TerminateMessage>(Terminate);
			CurrentChannel = channel;
		}

		public static Action ChangeChannel(params IChannelable[] channelables)
		{
			var tuple = CurrentChannel.GetNewChannel();
			channelables.ForEach(x => x.AddMessageHandlerTo(tuple.Item1));
			tuple.Item1.AddMessageHandler<TerminateMessage>(Terminate);
			CurrentChannel = tuple.Item1;
			return tuple.Item2;
		}

		private static void Terminate(TerminateMessage msg, Action callback)
		{
			IsFinished = true;
			callback();
		}

		static void Main(string[] args)
		{
#if !DEBUG
			try
			{
#endif
			Engine.Initialize("冒険者は森に強い", (int)WindowSize.X, (int)WindowSize.Y, new EngineOption());
			Engine.File.AddRootDirectories("Resources.pack");

			Model.Def.GetDataStream = GetDataStream;

			var flow = new Model.Title.LogoFlow();
			var scene = new Title.LogoScene();
			Engine.ChangeSceneWithTransition(scene, new TransitionFade(1, 1));
			ChangeFlow(flow.GetFlow(), scene);

			while(Engine.DoEvents() && !IsFinished)
			{
				CurrentChannel.Update();
				Engine.Update();
			}

			Engine.Terminate();
#if !DEBUG
			}
			catch (Exception ex)
			{
				var footer = DateTime.Now.ToString("yyyyMMddhhmmss");
				using (var log = new StreamWriter("error" + footer + ".txt"))
				{
					log.WriteLine(ex.GetType().Name);
					log.WriteLine(ex.Message);
					log.WriteLine(ex.StackTrace);
				}
				throw;
			}
#endif
		}

		private static TextReader GetDataStream(string filepath)
		{
			var path = Path.Combine("Data/", filepath);
			var file = Engine.File.CreateStaticFile(path);
			var bytes = file.ReadAllBytes();
			var fileString = string.Concat(Encoding.UTF8.GetChars(bytes.ToArray()));
			return new StringReader(fileString);
		}
	}
}
