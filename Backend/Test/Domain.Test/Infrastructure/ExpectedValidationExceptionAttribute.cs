using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Elfo.Contoso.LearningRoundKamran.Domain.Test
{
	public class ExpectedValidationExceptionAttribute : ExpectedExceptionBaseAttribute
	{
		private readonly ICollection<(string subject, ValidationErrorCode? code)> subjectsAndCodes
			= new List<(string subject, ValidationErrorCode? code)>();

		public ExpectedValidationExceptionAttribute() { }

		public ExpectedValidationExceptionAttribute(params ValidationErrorCode[] codes)
		{
			foreach (var code in codes)
				subjectsAndCodes.Add((null, code));
		}

		public ExpectedValidationExceptionAttribute(params string[] subjects)
		{
			foreach (var subject in subjects)
				subjectsAndCodes.Add((subject, null));
		}

		protected override void Verify(Exception exception)
		{
			Assert.IsInstanceOfType(exception, typeof(ValidationException));

			var validationException = (ValidationException)exception;

			Assert.IsTrue(validationException.HasErrors);

			foreach (var (subject, code) in subjectsAndCodes)
			{
				if (subject != null)
				{
					validationException.Errors.TryGetValue(subject, out var errors);

					Assert.IsNotNull(errors);

					if (code != null)
						Assert.IsTrue(errors.Any(e => e.Code == code.ToString()));
				}
				else if (code != null)
					Assert.IsTrue(validationException.Errors
						.SelectMany(e => e.Value.Select(v => v.Code))
						.Any(c => c == code.ToString()));
			}
		}
	}
}
