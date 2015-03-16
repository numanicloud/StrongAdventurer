using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rpg1.Model;
using Rpg1.Model.Entities;

namespace Rpg1.Test
{
	[TestClass]
	public class RpgHelperTest
	{
		[TestMethod]
		public void IsLoseTest()
		{
			Assert.IsTrue(RpgHelper.IsLose(SkillKind.Attack, SkillKind.Counter));
			Assert.IsTrue(RpgHelper.IsLose(SkillKind.Counter, SkillKind.Absorb));
			Assert.IsFalse(RpgHelper.IsLose(SkillKind.Attack, SkillKind.Absorb));
			Assert.IsFalse(RpgHelper.IsLose(SkillKind.Attack, SkillKind.Attack));
		}
	}
}
