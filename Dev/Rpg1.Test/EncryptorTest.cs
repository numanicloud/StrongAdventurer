using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rpg1.Model.Repository;

namespace Rpg1.Test
{
	[TestClass]
	public class EncryptorTest
	{
		[TestMethod]
		public void EncryptDecryptTest()
		{
			const string password = "password";

			var str = "ほげほげ";
			byte[] encrypted;
			string result;

			using (var inMemory = new MemoryStream(Encoding.UTF8.GetBytes(str)))
			{
				using (var outMemory = new MemoryStream())
				{
					Encryptor.Encrypt(inMemory, outMemory, password);
					encrypted = outMemory.ToArray();
				}
			}

			using (var inMemory = new MemoryStream(encrypted))
			{
				using (var outMemory = new MemoryStream())
				{
					Encryptor.Decrypt(inMemory, outMemory, password);
					var bytes = outMemory.ToArray();
                    result = string.Concat(Encoding.UTF8.GetChars(bytes));
				}
			}

			Assert.AreEqual(str, result);
		}

		[TestMethod]
		public void EncryptDecryptForFileTest()
		{
			const string password = "password";
			const string testFile = "test.dat";

			var str = "NumAniCloud";
			string result;

			using (var inStream = new MemoryStream(Encoding.UTF8.GetBytes(str)))
			{
				using (var file = new FileStream(testFile, FileMode.Create))
				{
					Encryptor.Encrypt(inStream, file, password);
				}
			}

			using (var file = new FileStream(testFile, FileMode.Open))
			{
				using (var outStream = new MemoryStream())
				{
					Encryptor.Decrypt(file, outStream, password);
					var bytes = outStream.ToArray();
					result = string.Concat(Encoding.UTF8.GetChars(bytes));
				}
			}

			Assert.AreEqual(str, result);
		}
	}
}
