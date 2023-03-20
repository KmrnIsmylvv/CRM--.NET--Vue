using Elfo.Round.WriteCycle;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;

namespace Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement
{
	public class UserInformation : Entity<SystemUserID>
	{
		#region Properties

		protected internal virtual SystemUser User { get; set; }
		public virtual string TelephoneNumber { get; protected set; }

		#endregion

		#region Ctor

		protected UserInformation() { }

		protected internal static UserInformation Create(SystemUser user, string telephoneNumber = default)
		{
			var ex = ValidateUser(user);

			ex.TryThrow();

			return new UserInformation
			{
				User = user,
				TelephoneNumber = telephoneNumber
			};
		}

		#endregion

		#region Methods

		protected internal virtual void Update(string telephoneNumber)
		{
			TelephoneNumber = telephoneNumber;
		}

		private static ValidationException ValidateUser(SystemUser user)
		{
			var ex = new ValidationException();

			if (user == null)
				ex.AddError(nameof(user), ValidationErrorCode.EmptyIdUser);

			return ex;
		}

		#endregion
	}
}
