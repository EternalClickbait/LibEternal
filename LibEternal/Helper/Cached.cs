using LibEternal.JetBrains.Annotations;
using System;

namespace LibEternal.Helper
{
	[PublicAPI]
	public class Cached<T>
	{
		public T Value
		{
			get
			{
				//If we aren't allow to have a null value, and the value is null, try updating it
				if (!allowNull && value is null)
					UpdateValue();

				return value;
			}
		}

		public T UpdateAndGetValue()
		{
			UpdateValue();
			return Value;
		}

		private readonly Func<T> updateDelegate;
		private T value = default;
		private readonly bool allowNull;

		public Cached(Func<T> updateDelegate)
		{
			this.updateDelegate = updateDelegate;
			UpdateValue();
		}

		public Cached(Func<T> updateDelegate, T initialValue, bool allowNull = false)
		{
			this.updateDelegate = updateDelegate;
			value = initialValue;
			this.allowNull = allowNull;
		}

		public void UpdateValue()
		{
			value = updateDelegate();
		}
	}
}