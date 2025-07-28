using System.Collections.Generic;
using UnityEngine;

public class LogPool : MonoBehaviour
{
    [SerializeField] private LogEntry entryPrefab;
    [SerializeField] private int initialSize = 10;

    private Queue<LogEntry> _pool = new Queue<LogEntry>();

    private void Awake()
    {
        for (int i = 0; i < initialSize; i++)
        {
            var e = Instantiate(entryPrefab, transform);
            e.gameObject.SetActive(false);
            _pool.Enqueue(e);
        }
    }

    public LogEntry Get()
    {
        if (_pool.Count == 0)
        {
            var e = Instantiate(entryPrefab, transform);
            e.gameObject.SetActive(false);
            _pool.Enqueue(e);
        }
        var entry = _pool.Dequeue();
        entry.gameObject.SetActive(true);
        return entry;
    }

    public void Release(LogEntry entry)
    {
        entry.gameObject.SetActive(false);
        _pool.Enqueue(entry);
    }
}
