using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Elfo.Contoso.LearningRoundKamran.Domain.Test
{
	[TestClass]
	public class SystemUserTest
	{
		[TestMethod]
		[DataRow("username", "en", "en", "Lol", "Lolita", "username@user.com", null, "password")]
		[DataRow("username", "en", "en", "Lol", "Lolita", "username@user.com", "+380900000000", "password")]
		public void Create_Default_Ok(string username, string languageCode, string cultureCode, string firstName, string lastName, string email, 
			string telephoneNumber, string password)
		{
			var user = new SystemUserBuilder()
				.WithUserName(username)
				.WithLanguage(languageCode)
				.WithCulture(cultureCode)
				.WithFirstName(firstName)
				.WithLastName(lastName)
				.WithEmail(email)
				.WithTelephoneNumber(telephoneNumber)
				.WithPassword(password)
				.Build();

			Assert.AreEqual(username, user.Username);
			Assert.AreEqual(languageCode, user.LanguageCode);
			Assert.AreEqual(cultureCode, user.CultureCode);
			Assert.AreEqual(firstName, user.FirstName);
			Assert.AreEqual(lastName, user.LastName);
			Assert.AreEqual(email, user.Email);
			Assert.AreEqual(telephoneNumber, user.UserInformation.TelephoneNumber);

			// password should be hashed
			Assert.AreNotEqual(password, user.Password);
		}

		[TestMethod]
		[DataRow(""), DataRow(" "), DataRow(null)]
		[ExpectedValidationExceptionAttribute(ValidationErrorCode.EmptyUsername)]
		public void Create_EmptyUsername_Error(string username)
		{
			new SystemUserBuilder().WithUserName(username).Build();
		}

		[TestMethod]
		[DataRow("adasdasdasasasasasasasasasasasasasasasasasasasasasasasasasasasasas@elfo.net")]
		[ExpectedValidationExceptionAttribute(ValidationErrorCode.InvalidUsernameLength)]
		public void Create_InvalidUsernameLength_Error(string username)
		{
			new SystemUserBuilder().WithUserName(username).Build();
		}

		[TestMethod]
		[DataRow("elfolol"), DataRow("elfo.net"), DataRow("elfo@."), DataRow("elfo@.1"), DataRow("@elfo.net")]
		[ExpectedValidationExceptionAttribute(ValidationErrorCode.InvalidUserEmail)]
		public void Create_InvalidUserEmail_Error(string email)
		{
			new SystemUserBuilder().WithEmail(email).Build();
		}

		[TestMethod]
		[DataRow("username", "en", "en-GB", "Lol", "Lolita", "username@user.com", null)]
		[DataRow("username", "en", "en", "Lol", "Lolita", "username@user.com", "+380900000000")]
		public void Update_Default_Ok(string username, string languageCode,
			string cultureCode, string firstName, string lastName, string email, string telephoneNumber)
		{
			var user = new SystemUserBuilder()
				.Build();

			user.Update(username, languageCode, cultureCode, firstName, lastName, email, telephoneNumber);

			Assert.AreEqual(username, user.Username);
			Assert.AreEqual(languageCode, user.LanguageCode);
			Assert.AreEqual(cultureCode, user.CultureCode);
			Assert.AreEqual(firstName, user.FirstName);
			Assert.AreEqual(lastName, user.LastName);
			Assert.AreEqual(email, user.Email);
			Assert.AreEqual(telephoneNumber, user.UserInformation.TelephoneNumber);
		}

		[TestMethod]
		[DataRow("I'mValidPassword")]
		public void ChangePassword_Default_Ok(string password)
		{
			var user = new SystemUserBuilder().WithPassword(password).Build();

			Assert.IsNotNull(user);
			Assert.AreNotEqual(password, user.Password);
		}

		[TestMethod]
		[DataRow("")]
		[DataRow(" ")]
		[DataRow(null)]
		[ExpectedValidationExceptionAttribute(ValidationErrorCode.EmptyPassword)]
		public void ChangePassword_EmptyPassword_Error(string password)
		{
			new SystemUserBuilder().WithPassword(password).Build();
		}
	}
}