﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpg1.Model
{
	public interface IFlow
	{
		IEnumerable<IMessage> GetFlow();
	}
}
