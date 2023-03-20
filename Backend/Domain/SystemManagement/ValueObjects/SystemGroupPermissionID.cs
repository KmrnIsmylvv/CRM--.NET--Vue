using Elfo.Round.WriteCycle;
using System.Collections.Generic;

namespace Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects
{
	public class SystemGroupPermissionID : ValueObject
	{
		public SystemGroupPermissionID() { }

		public SystemGroupPermissionID(SystemGroupID idGroup, string permission)
		{
			IdGroup = idGroup;
			Permission = permission;
		}

		#region Properties

		public SystemGroupID IdGroup { get; protected set; }
		public string Permission { get; protected set; }

		#endregion

		#region Equality
		protected override IEnumerable<object> GetEqualityComponents()
		{
			yield return IdGroup;
			yield return Permission;
		}

		public override int GetHashCode() => IdGroup.GetHashCode() ^ Permission.GetHashCode();

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
				return false;

			var other = (SystemGroupPermissionID)obj;
			return IdGroup == other.IdGroup && Permission == other.Permission;
		}

		#endregion
	}
}
