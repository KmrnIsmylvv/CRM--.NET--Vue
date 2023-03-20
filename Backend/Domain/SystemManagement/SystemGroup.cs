using Elfo.Round.WriteCycle;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;

namespace Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement
{
	public class SystemGroup : AggregateRoot<SystemGroupID>
	{
		#region Properties

		public virtual string Name { get; protected set; }
		public virtual bool IsEnabled { get; protected set; }

		#endregion

		#region Ctor

		protected SystemGroup() { }

		public static SystemGroup Create(string name)
		{
			var ex = ValidateName(name);

			var systemGroup = new SystemGroup
			{
				Name = name
			};

			ex.CollectErrors(() =>
			{
				systemGroup.Enable();
			})
			.TryThrow();

			return systemGroup;
		}

		#endregion

		#region Methods

		public virtual void Update(string name)
		{
			ValidateName(name)
				.TryThrow();

			Name = name;
		}

		public virtual void Enable() =>
			IsEnabled = true;

		public virtual void Disable() =>
			IsEnabled = false;

		private static ValidationException ValidateName(string name)
		{
			const int maxLength = 50;

			var ex = new ValidationException();

			if (string.IsNullOrWhiteSpace(name))
				ex.AddError(nameof(Name), ValidationErrorCode.EmptySystemGroupName);
			else
			if (name.Length > maxLength)
				ex.AddError(nameof(Name), ValidationErrorCode.InvalidSystemGroupNameLength, maxLength.ToString());

			return ex;
		}

		#endregion

	}
}
