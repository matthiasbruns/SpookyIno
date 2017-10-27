using System;

namespace ARena
{
	[Serializable]
	public class Optional<T>
	{
		private T value;

		public bool useCustomValue = false;

		public Optional() {}

		public Optional(T value)
		{
			this.value = value;
		}

		public T Value {
			get {
				return value;
			}
			set {
				this.value = value;
			}
		}

		public bool IsNull() {
			return value == null;
		}

		public bool HasValue() {
			return value != null;
		}
	}
}

