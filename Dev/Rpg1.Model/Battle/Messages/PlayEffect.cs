using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpg1.Model.Battle.Messages
{
	public class PlayEffect : IMessage
	{
		public int BattlerIndex { get; private set; }
		public string EffectPath { get; private set; }
		public string SoundPath { get; private set; }
		public float Wait { get; private set; }

		public PlayEffect(int battlerIndex, string effectPath, string soundPath, float wait)
		{
			BattlerIndex = battlerIndex;
			EffectPath = effectPath;
			SoundPath = soundPath;
			Wait = wait;
		}
	}
}
