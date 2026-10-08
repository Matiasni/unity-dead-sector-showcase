public readonly struct LocalizedMessage
{
    public static readonly LocalizedMessage Empty = new(null);

    public string Key { get; }
    public object[] Args { get; }

    public bool IsEmpty => string.IsNullOrEmpty(Key);

    public LocalizedMessage(string key, params object[] args)
    {
        Key = key;
        Args = args;
    }
}
