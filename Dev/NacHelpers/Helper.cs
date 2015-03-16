using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NacHelpers
{
	public static class Helper
	{
		public static int Clamp(int value, int min, int max)
		{
			if(min > max)
			{
				throw new ArgumentException("最大値は最小値以上である必要があります。");
			}
			else if(value <= min)
			{
				return min;
			}
			else if(value >= max)
			{
				return max;
			}
			return value;
		}

		public static T GetRandom<T>(this IEnumerable<T> source, Random random = null)
		{
			var list = source.ToList();
			if(!list.Any())
			{
				throw new Exception("空のコレクションから値を取り出すことはできません。");
			}

			random = random ?? new Random();
			var index = random.Next(list.Count);
			return list[index];
		}

		public static IEnumerable<T> Append<T>(this IEnumerable<T> source, T obj)
		{
			foreach(var item in source)
			{
				yield return item;
			}
			yield return obj;
		}
	}
}
