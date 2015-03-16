using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Entities;

namespace Rpg1.Model.Battle.AI
{
	[DataContract]
	abstract class AiCustomize
	{
		[DataMember]
		public int Priority { get; set; }

		public abstract bool ConditionIsSatisfied(ActionContext context);

		public abstract SkillKind Invoke();
	}
}
