using System;
using System.Collections.Generic;
using Rpg1.Model.Entities;

namespace Rpg1.Model.Messages
{
	public class ChoiceMessage<T> : IResponse<T>
	{
		public IEnumerable<T> Choices { get; set; }
		public T Response { get; set; }

		public ChoiceMessage(IEnumerable<T> choices)
		{
			Choices = choices;
		}
	}
}