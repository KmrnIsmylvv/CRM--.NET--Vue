using Elfo.Round.WriteCycle;
using System;
using System.Collections.Generic;

namespace Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects
{
	[Serializable]
	public class SystemUserGroupAssociationID : ValueObject
	{
		#region Properties

		public virtual SystemUserID IdUser { get; protected set; }
		public virtual SystemGroupID IdGroup { get; protected set; }

		#endregion

		public SystemUserGroupAssociationID() { }

		public SystemUserGroupAssociationID(SystemUserID idUser, SystemGroupID idGroup)
		{
			IdUser = idUser;
			IdGroup = idGroup;
		}

		#region Equality

		protected override IEnumerable<object> GetEqualityComponents()
		{
			yield return IdUser;
			yield return IdGroup;
		}

		public override int GetHashCode() => IdUser.GetHashCode() ^ IdGroup.GetHashCode();

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
				return false;

			var other = (SystemUserGroupAssociationID)obj;

			return IdUser == other.IdUser && IdGroup == other.IdGroup;
		}

		#endregion
	}
}
