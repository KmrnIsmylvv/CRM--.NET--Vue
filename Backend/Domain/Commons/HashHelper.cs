using Elfo.Round.Cryptography;

namespace Elfo.Contoso.LearningRoundKamran.Domain
{
	public static class HashHelper
	{
		public static string Hash<T>(T value, string salt) =>
			Pbkdf2.GetHash(value.ToString(), salt.ToLower());
	}
}
