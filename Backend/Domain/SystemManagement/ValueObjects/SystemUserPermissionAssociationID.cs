using Elfo.Round.WriteCycle;
using System.Collections.Generic;

namespace Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects
{
	public class SystemUserPermissionAssociationID : ValueObject
	{
		#region Properties

		public virtual SystemUserID IdUser { get; protected set; }
		public virtual string Permission { get; protected set; }

		#endregion

		public SystemUserPermissionAssociationID() { }

		public SystemUserPermissionAssociationID(SystemUserID idUser, string permission)
		{
			IdUser = idUser;
			Permission = permission;
		}

		#region Equality

		protected override IEnumerable<object> GetEqualityComponents()
		{
			yield return IdUser;
			yield return Permission;
		}

		public override int GetHashCode() => IdUser.GetHashCode() ^ Permission.GetHashCode();

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
				return false;

			var other = (SystemUserPermissionAssociationID)obj;
			return IdUser == other.IdUser && Permission == other.Permission;
		}

		#endregion
	}
}
