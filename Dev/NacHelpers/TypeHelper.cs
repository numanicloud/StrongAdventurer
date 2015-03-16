using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NacHelpers
{
	public static class TypeHelper
	{
		public static IEnumerable<T> GetValues<T>() where T : struct
		{
			var enumType = typeof(T);
			if(!enumType.IsEnum)
			{
				throw new ArgumentException("列挙型でない値が指定されました。", "enumType");
			}
			return Enum.GetValues(enumType).Cast<T>();
		}

		public static bool IsImplementationOf<TInterface>(this Type type)
		{
			return type.GetInterfaces().Contains(typeof(TInterface));
		}
	}

}