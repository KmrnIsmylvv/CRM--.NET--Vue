using Elfo.Round.WriteCycle;
using System.Collections.Generic;

namespace Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects
{
	/// <summary>
	/// ID&lt;<see cref="short"/>&gt;
	/// </summary>
	public class SystemGroupID : ID<short>
	{
		public SystemGroupID() =>
			Value = default;

		public SystemGroupID(short value) =>
			Value = value;

		protected override IEnumerable<object> GetEqualityComponents()
		{
			yield return Value;
		}
	}
}
