using Elfo.Round.WriteCycle;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;

namespace Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement
{
	public class SystemUserGroupAssociation : AggregateRoot<SystemUserGroupAssociationID>
	{
		#region Ctor

		protected SystemUserGroupAssociation() { }

		public static SystemUserGroupAssociation Create(SystemUserID idUser, SystemGroupID idGroup)
		{
			Validate(idUser, idGroup)
				.TryThrow();

			return new SystemUserGroupAssociation
			{
				Id = new SystemUserGroupAssociationID(idUser, idGroup)
			};
		}

		#endregion

		#region Methods

		private static ValidationException Validate(SystemUserID idUser, SystemGroupID idGroup)
		{
			var ex = new ValidationException();

			if (idUser == null)
				ex.AddError(nameof(SystemUserGroupAssociationID.IdUser), ValidationErrorCode.EmptyIdUser);

			if (idGroup == null)
				ex.AddError(nameof(SystemUserGroupAssociationID.IdGroup), ValidationErrorCode.EmptyIdGroup);

			return ex;
		}

		#endregion
	}
}
