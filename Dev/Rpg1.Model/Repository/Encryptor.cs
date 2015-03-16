using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Rpg1.Model.Repository
{
	class Encryptor
	{
		private static void GenerateKey(string password, int keySize, int blockSize, out byte[] key, out byte[] iv)
		{
			byte[] salt = Encoding.UTF8.GetBytes("NumAniSalt");
			var derivebytes = new Rfc2898DeriveBytes(password, salt);

			key = derivebytes.GetBytes(keySize / 8);
			iv = derivebytes.GetBytes(blockSize / 8);
		}

		public static void Encrypt(Stream source, Stream dest, string password)
		{
			var rijndael = new RijndaelManaged();
			rijndael.Padding = PaddingMode.PKCS7;

			byte[] key, iv;
			GenerateKey(password, rijndael.KeySize, rijndael.BlockSize, out key, out iv);
			rijndael.Key = key;
			rijndael.IV = iv;

			source.Position = 0;

			using (var encryptor = rijndael.CreateEncryptor())
			{
				using (var cryptoStream = new CryptoStream(dest, encryptor, CryptoStreamMode.Write))
				{
					byte[] bytes = new byte[1024];
					var readLength = 0;
					while((readLength = source.Read(bytes, 0, bytes.Length)) > 0)
					{
						cryptoStream.Write(bytes, 0, readLength);
					}
					cryptoStream.FlushFinalBlock();
				}
			}
		}

		public static void Decrypt(Stream source, Stream dest, string password)
		{
			var rijndael = new RijndaelManaged();
			rijndael.Padding = PaddingMode.PKCS7;

			byte[] key, iv;
			GenerateKey(password, rijndael.KeySize, rijndael.BlockSize, out key, out iv);
			rijndael.Key = key;
			rijndael.IV = iv;

			using (var decryptor = rijndael.CreateDecryptor())
			{
				using (var cryptoStream = new CryptoStream(source, decryptor, CryptoStreamMode.Read))
				{
					byte[] bytes = new byte[1024];
					var readLength = 0;
					while((readLength = cryptoStream.Read(bytes, 0, bytes.Length)) > 0)
					{
						dest.Write(bytes, 0, readLength);
					}
				}
			}

			dest.Position = 0;
		}
	}
}
