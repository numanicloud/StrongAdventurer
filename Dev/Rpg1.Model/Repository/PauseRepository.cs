using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Battle;
using Rpg1.Model.Entities;
using CsvHelper;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Rpg1.Model.Repository
{
	class PauseRepository
	{
		public bool DoEncrypt { get; set; }

		public void Save(PauseData pause)
		{
			var serializer = new DataContractJsonSerializer(typeof(PauseData), GetKnownAiType());

			using (var file = new FileStream(Def.PauseDataPath, FileMode.Create, FileAccess.Write))
			{
				if(DoEncrypt)
				{
					using (var memory = new MemoryStream())
					{
						serializer.WriteObject(memory, pause);
						Encryptor.Encrypt(memory, file, Def.DataPassword);
					}
				}
				else
				{
					serializer.WriteObject(file, pause);
				}
			}
		}

		public PauseData Load()
		{
			var serializer = new DataContractJsonSerializer(typeof(PauseData), GetKnownAiType());

			using (var file = new FileStream(Def.PauseDataPath, FileMode.Open, FileAccess.Read))
			{
				if(DoEncrypt)
				{
					using (var memory = new MemoryStream())
					{
						Encryptor.Decrypt(file, memory, Def.DataPassword);
						return serializer.ReadObject(memory) as PauseData;
					}
				}
				else
				{
					return serializer.ReadObject(file) as PauseData;
				}
			}
		}

		private Type[] GetKnownAiType()
		{
			return new Type[]
			{
				typeof(Battle.AI.CheckingAIPauseData),
				typeof(Battle.AI.RepeatAIPauseData),
				typeof(Battle.AI.NightKingAIPauseData)
			};
		}
	}
}
