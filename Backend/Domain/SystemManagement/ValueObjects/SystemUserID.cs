using Elfo.Round.WriteCycle;
using System.Collections.Generic;

namespace Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects
{
	/// <summary>
	/// ID&lt;<see cref="int"/>&gt;
	/// </summary>
	public class SystemUserID : ID<int>
	{
		public SystemUserID() =>
			Value = default;

		public SystemUserID(int value) =>
			Value = value;

		protected override IEnumerable<object> GetEqualityComponents()
		{
			yield return Value;
		}
	}
}
