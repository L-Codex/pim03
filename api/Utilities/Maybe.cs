namespace api.Utilities
{
    public readonly struct Maybe<T>
    {
        public bool HasError { get; }
        public bool HasValue { get; }
        public RepoError? Error { get; }
        public T Value { get; }

        public Maybe(T value)
        {
            HasError = false;
            HasValue = true;
            Error = null;
            Value = value;
        }

        public Maybe(RepoError error)
        {
            HasError = true;
            HasValue = false;
            Error = error;
            Value = default!;
        }
    }
}
