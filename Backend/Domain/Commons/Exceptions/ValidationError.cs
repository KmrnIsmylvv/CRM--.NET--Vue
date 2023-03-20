using Elfo.Round.Exceptions;

namespace Elfo.Contoso.LearningRoundKamran.Domain
{
	public class ValidationError : IValidationError
	{
		public string Code => ErrorCode.ToString();
		public ValidationErrorCode ErrorCode { get; }
		public object[] AdditionalInfo { get; }

		public ValidationError(ValidationErrorCode code, object[] additionalInfo = null)
		{
			ErrorCode = code;
			AdditionalInfo = additionalInfo;
		}
	}
}
