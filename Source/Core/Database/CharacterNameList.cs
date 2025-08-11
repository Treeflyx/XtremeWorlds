using System.Collections;

namespace Core.Database;

public sealed class CharacterNameList : IEnumerable<string>
{
    private readonly HashSet<string> _names = new(StringComparer.OrdinalIgnoreCase);
    private readonly ReaderWriterLockSlim _lock = new();

    public int Count => ExecuteRead(() => _names.Count);

    public bool Contains(string characterName) => !string.IsNullOrWhiteSpace(characterName) && ExecuteRead(() => _names.Contains(characterName));

    public void Add(string characterName)
    {
        if (!IsValidName(characterName))
        {
            return;
        }

        ExecuteWrite(() => _names.Add(characterName));
    }

    public void Remove(string characterName)
    {
        if (string.IsNullOrWhiteSpace(characterName))
        {
            return;
        }

        ExecuteWrite(() => _names.Remove(characterName));
    }

    private static bool IsValidName(string name) => !string.IsNullOrWhiteSpace(name) && name.Length is >= 3 and <= 20;

    public IEnumerator<string> GetEnumerator() => ExecuteRead(() => _names.ToList().GetEnumerator());

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private T ExecuteRead<T>(Func<T> func)
    {
        _lock.EnterReadLock();
        try
        {
            return func();
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    private void ExecuteWrite(Action action)
    {
        _lock.EnterWriteLock();
        try
        {
            action();
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }
}