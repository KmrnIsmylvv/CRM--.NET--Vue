using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement;

namespace Elfo.Contoso.LearningRoundKamran.Domain.Test
{
	public class SystemUserBuilder
	{
		private string username = "roundIsBetterThanKernel";
		private string languageCode = "en";
		private string cultureCode = "en-GB";
		private string firstName = "Dimascopo";
		private string lastName = "Huyascopo";
		private string email = "roundIsBetterThanKernel@round.come";
		private string telephoneNumber = "+380930886565";
		private string password = "MyPasswordIsSheet";

		public SystemUserBuilder() { }

		public SystemUserBuilder WithUserName(string value)
		{
			username = value;
			return this;
		}

		public SystemUserBuilder WithPassword(string value)
		{
			password = value;
			return this;
		}

		public SystemUserBuilder WithFirstName(string value)
		{
			firstName = value;
			return this;
		}

		public SystemUserBuilder WithLastName(string value)
		{
			lastName = value;
			return this;
		}

		public SystemUserBuilder WithLanguage(string value)
		{
			languageCode = value;
			return this;
		}

		public SystemUserBuilder WithCulture(string value)
		{
			cultureCode = value;
			return this;
		}

		public SystemUserBuilder WithEmail(string value)
		{
			email = value;
			return this;
		}

		public SystemUserBuilder WithTelephoneNumber(string value)
		{
			telephoneNumber = value;
			return this;
		}

		public SystemUser Build() =>
			SystemUser.Create(username, languageCode, cultureCode, firstName, lastName, email, telephoneNumber, password);
	}
}
