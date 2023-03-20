using Elfo.Round.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Elfo.Contoso.LearningRoundKamran.Domain
{
	[Serializable]
	public class ValidationException : Exception, IValidationException
	{
		#region Properties

		public IDictionary<string, ICollection<IValidationError>> Errors { get; set; }
			= new Dictionary<string, ICollection<IValidationError>>();
		public bool HasErrors => Errors.Any();

		#endregion

		#region Ctor

		public ValidationException() { }

		public ValidationException(string subject, ValidationErrorCode code, params object[] additionalInfo)
			: this(subject, new ValidationError(code, additionalInfo)) { }

		public ValidationException(string subject, params IValidationError[] errors)
		{
			foreach (var error in errors)
				AddError(subject, error);
		}

		#endregion

		#region Methods

		public void TryThrow()
		{
			if (HasErrors)
				throw this;
		}

		public ValidationException CollectErrors(Action action)
		{
			try
			{
				action();
			}
			catch (ValidationException ex)
			{
				MergeWith(ex);
			}

			return this;
		}

		public void AddError(string subject, ValidationErrorCode code, params object[] additionalInfo) =>
			AddError(subject, new ValidationError(code, additionalInfo));

		private void AddError(string subject, IValidationError error)
		{
			if (Errors.ContainsKey(subject))
				Errors[subject].Add(error);
			else
				Errors.Add(subject, new List<IValidationError> { error });
		}

		private void MergeWith(IValidationException ex)
		{
			foreach (var item in ex.Errors)
				foreach (var error in item.Value)
					AddError(item.Key, error);
		}

		#endregion

		#region Operators

		public static ValidationException operator +(ValidationException left, ValidationException right)
		{
			left.MergeWith(right);

			return left;
		}

		#endregion
	}
}
