namespace dotNet_Chat_App.TAP_Socket_Model
{
	public class TAPResultPattern
	{
		protected TAPResultPattern(bool success, string error)
		{
			Success = success;
			Error = error;
		}

		public bool Success { get; }
		public string Error { get; }

		public bool Failure => !Success;

		public static TAPResultPattern Fail(string message)
		{
			return new TAPResultPattern(false, message);
		}

		public static TAPResultPattern<T> Fail<T>(string message)
		{
			return new TAPResultPattern<T>(default(T), false, message);
		}

		public static TAPResultPattern Ok()
		{
			return new TAPResultPattern(true, string.Empty);
		}

		public static TAPResultPattern<T> Ok<T>(T value)
		{
			return new TAPResultPattern<T>(value, true, string.Empty);
		}

		public static TAPResultPattern Combine(params TAPResultPattern[] results)
		{
			foreach (TAPResultPattern result in results)
			{
				if (result.Failure)
				{
					return result;
				}
			}

			return Ok();
		}
	}

	public class TAPResultPattern<T> : TAPResultPattern
	{
		public T Value { get; }

		protected internal TAPResultPattern(T value, bool success, string error)
			: base(success, error)
		{
			Value = value;
		}
	}
}
