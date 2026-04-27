namespace api.Utilities
{
    public readonly struct Maybe<T>
    {
        public bool HasError { get; }
        public bool HasValue { get; }
        public ErrorCodes? Error { get; }
        public T Value { get; }

        public Maybe(T value)
        {
            HasError = false;
            HasValue = true;
            Error = null;
            Value = value;
        }

        public Maybe(ErrorCodes error)
        {
            HasError = true;
            HasValue = false;
            Error = error;
            Value = default!;
        }
    }
}
