using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpg1.Model
{
	public interface IMessage
	{
	}

	public interface IResponse<T> : IMessage
	{
		T Response { get;set; }
	}
}
