using Elfo.Round.WriteCycle;
using System.Collections.Generic;

namespace Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects
{
	public class SystemGroupPermissionAssociationID : ValueObject
	{
		#region Properties

		public virtual SystemGroupID IdGroup { get; protected set; }
		public virtual string Permission { get; protected set; }

		#endregion

		public SystemGroupPermissionAssociationID() { }

		public SystemGroupPermissionAssociationID(SystemGroupID idGroup, string permission)
		{
			IdGroup = idGroup;
			Permission = permission;
		}

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

			var other = (SystemGroupPermissionAssociationID)obj;
			return IdGroup == other.IdGroup && Permission == other.Permission;
		}

		#endregion
	}
}
