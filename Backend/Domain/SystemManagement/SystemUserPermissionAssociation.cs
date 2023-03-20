using Elfo.Round.WriteCycle;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;

namespace Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement
{
	public class SystemUserPermissionAssociation : AggregateRoot<SystemUserPermissionAssociationID>
	{
		#region Ctor

		protected SystemUserPermissionAssociation() { }

		public static SystemUserPermissionAssociation Create(SystemUserID idUser, string permission)
		{
			Validate(idUser, permission)
				.TryThrow();

			return new SystemUserPermissionAssociation
			{
				Id = new SystemUserPermissionAssociationID(idUser, permission)
			};
		}

		#endregion

		#region Methods

		private static ValidationException Validate(SystemUserID idUser, string permission)
		{
			var ex = new ValidationException();

			if (idUser == null)
				ex.AddError(nameof(SystemUserPermissionAssociationID.IdUser), ValidationErrorCode.EmptyIdUser);

			if (string.IsNullOrWhiteSpace(permission))
				ex.AddError(nameof(SystemUserPermissionAssociationID.Permission), ValidationErrorCode.EmptyPermission);

			return ex;
		}

		#endregion
	}
}
