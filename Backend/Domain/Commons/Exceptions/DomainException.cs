using Elfo.Round.Exceptions;
using System;
using System.Runtime.Serialization;

namespace Elfo.Contoso.LearningRoundKamran.Domain
{
	[Serializable]
	public class DomainException : Exception, IDomainException
	{
		#region Properties

		public string Code => ExceptionCode.ToString();
		public DomainExceptionCode? ExceptionCode { get; }
		public object[] AdditionalInfo { get; }

		#endregion

		#region Ctor

		public DomainException() { }

		public DomainException(string message, DomainExceptionCode code, params object[] additionalInfo)
			: base(message)
		{
			ExceptionCode = code;
			AdditionalInfo = additionalInfo;
		}

		public DomainException(DomainExceptionCode code, params object[] parameters)
			: this(code.ToString(), code, parameters) { }

		public DomainException(string message) : base(message) { }

		public DomainException(string message, Exception inner) : base(message, inner) { }

		protected DomainException(SerializationInfo info, StreamingContext context) : base(info, context) { }

		#endregion
	}
}
