using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using Rpg1.Model.Entities;

namespace Rpg1.Model.Repository
{
	public static class ScoreRepository
	{
		public static void Save(Score score, bool doEncrypt)
		{
			var serializer = new DataContractJsonSerializer(typeof(Score));

			using (var file = new FileStream(Def.ScoreDataPath, FileMode.Create, FileAccess.Write))
			{
				if(doEncrypt)
				{
					using (var memory = new MemoryStream())
					{
						serializer.WriteObject(memory, score);
						Encryptor.Encrypt(memory, file, Def.DataPassword);
					}
				}
				else
				{
					serializer.WriteObject(file, score);
				}
			}
		}

		/// <summary>
		/// スコアファイルをロードする。存在しない場合は作成する。
		/// </summary>
		/// <param name="doEncrypt">復号化するかどうか。</param>
		/// <returns></returns>
		public static Score Load(bool doEncrypt)
		{
			if(File.Exists(Def.ScoreDataPath))
			{
				var serializer = new DataContractJsonSerializer(typeof(Score));

				using (var file = new FileStream(Def.ScoreDataPath, FileMode.Open, FileAccess.Read))
				{
					if(doEncrypt)
					{
						using (var memory = new MemoryStream())
						{
							Encryptor.Decrypt(file, memory, Def.DataPassword);
							return serializer.ReadObject(memory) as Score;
						}
					}
					else
					{
						return serializer.ReadObject(file) as Score;
					}
				}
			}
			else
			{
				var result = Score.CreateInitial();
				Save(result, doEncrypt);
				return result;
			}
		}
	}
}
