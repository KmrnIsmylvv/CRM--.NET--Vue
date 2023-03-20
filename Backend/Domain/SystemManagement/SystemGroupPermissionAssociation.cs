using Elfo.Round.WriteCycle;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;

namespace Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement
{
	public class SystemGroupPermissionAssociation : AggregateRoot<SystemGroupPermissionAssociationID>
	{
		#region Ctor

		protected SystemGroupPermissionAssociation() { }

		public static SystemGroupPermissionAssociation Create(SystemGroupID idGroup, string permission)
		{
			Validate(idGroup, permission)
				.TryThrow();

			return new SystemGroupPermissionAssociation
			{
				Id = new SystemGroupPermissionAssociationID(idGroup, permission)
			};
		}

		#endregion

		#region Methods

		private static ValidationException Validate(SystemGroupID idGroup, string permission)
		{
			var ex = new ValidationException();

			if (idGroup == null)
				ex.AddError(nameof(SystemGroupPermissionAssociationID.IdGroup), ValidationErrorCode.EmptyIdGroup);

			if (string.IsNullOrWhiteSpace(permission))
				ex.AddError(nameof(SystemGroupPermissionAssociationID.Permission), ValidationErrorCode.EmptyPermission);

			return ex;
		}

		#endregion
	}
}
