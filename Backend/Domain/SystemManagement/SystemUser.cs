using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;
using Elfo.Round.WriteCycle;
using System.Net.Mail;

namespace Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement
{
	public class SystemUser : AggregateRoot<SystemUserID>
	{
		#region Constants

		private const string standardPassword = "csv";

		private const short maxUsernameLength = 50;
		private const short languageCodeLength = 2;
		private const short minCultureCodeLength = 2;
		private const short maxCultureCodeLength = 10;

		#endregion

		#region Properties

		public virtual string Username { get; protected set; }
		public virtual string Password { get; protected set; }
		public virtual string LanguageCode { get; protected set; }
		public virtual string CultureCode { get; protected set; }
		public virtual string FirstName { get; protected set; }
		public virtual string LastName { get; protected set; }
		public virtual string Email { get; protected set; }
		public virtual bool IsEnabled { get; protected set; }
		public virtual UserInformation UserInformation { get; protected set; }

		#endregion

		#region Ctor

		protected SystemUser() { }

		public static SystemUser Create(string username, string languageCode, string cultureCode, string firstName, string lastName, string email,
			string telephoneNumber = default, string password = standardPassword)
		{
			var ex = Validate(username, languageCode, cultureCode, firstName, lastName, email)
					 + ValidatePassword(password);

			var systemUser = new SystemUser
			{
				Username = username,
				LanguageCode = languageCode,
				CultureCode = cultureCode,
				FirstName = firstName,
				LastName = lastName,
				Email = email
			};

			ex.CollectErrors(() =>
			{
				systemUser.Enable();
				systemUser.UserInformation = UserInformation.Create(systemUser, telephoneNumber);
			}).TryThrow();

			systemUser.ChangePassword(password);

			return systemUser;
		}

		#endregion

		#region Methods

		public virtual void Update(string username, string languageCode, string cultureCode, string firstName, string lastName, string email,
			string telephoneNumber = default)
		{
			var ex = Validate(username, languageCode, cultureCode, firstName, lastName, email);

			Username = username;
			LanguageCode = languageCode;
			CultureCode = cultureCode;
			FirstName = firstName;
			LastName = lastName;
			Email = email;

			ex.CollectErrors(() => UserInformation?.Update(telephoneNumber))
				.TryThrow();
		}

		public virtual void Enable() =>
			IsEnabled = true;

		public virtual void Disable() =>
			IsEnabled = false;

		public virtual void ChangePassword(string password = standardPassword)
		{
			ValidatePassword(password)
				.TryThrow();

			Password = HashPassword(password, Username);
		}

		private static string HashPassword(string password, string username) =>
			HashHelper.Hash(password, username);

		private static ValidationException Validate(string username, string languageCode,
			string cultureCode, string firstName, string lastName, string email)
		{
			var ex = new ValidationException();

			if (string.IsNullOrWhiteSpace(username))
				ex.AddError(nameof(Username), ValidationErrorCode.EmptyUsername);
			else if (username.Length > maxUsernameLength)
				ex.AddError(nameof(Username), ValidationErrorCode.InvalidUsernameLength,
					new { maxUsernameLength });

			if (string.IsNullOrWhiteSpace(languageCode))
				ex.AddError(nameof(LanguageCode), ValidationErrorCode.EmptyLanguageCode);
			else if (languageCode.Length != languageCodeLength)
				ex.AddError(nameof(LanguageCode), ValidationErrorCode.InvalidLanguageCodeLength,
					new { languageCodeLength });

			if (string.IsNullOrWhiteSpace(cultureCode))
				ex.AddError(nameof(CultureCode), ValidationErrorCode.EmptyCultureCode);
			else if (cultureCode.Length > maxCultureCodeLength || cultureCode.Length < minCultureCodeLength)
				ex.AddError(nameof(LanguageCode), ValidationErrorCode.InvalidCultureCodeLength,
					new { minCultureCodeLength, maxCultureCodeLength });

			if (string.IsNullOrWhiteSpace(firstName))
				ex.AddError(nameof(FirstName), ValidationErrorCode.EmptyFirstName);

			if (string.IsNullOrWhiteSpace(lastName))
				ex.AddError(nameof(LastName), ValidationErrorCode.EmptyLastName);

			var isValidAsEmail = false;

			try
			{
				var validEmail = new MailAddress(email);
				isValidAsEmail = email == validEmail.Address;
			}
			catch
			{
				// ignored
			}

			if (!isValidAsEmail)
				ex.AddError(nameof(Email), ValidationErrorCode.InvalidUserEmail);

			return ex;
		}

		private static ValidationException ValidatePassword(string password)
		{
			var ex = new ValidationException();

			if (string.IsNullOrWhiteSpace(password))
				ex.AddError(nameof(Password), ValidationErrorCode.EmptyPassword);

			return ex;
		}

		#endregion
	}
}
