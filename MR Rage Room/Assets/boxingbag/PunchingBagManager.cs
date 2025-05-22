using System.Collections.Generic;
using UnityEngine;

public class PunchingBagManager : MonoBehaviour
{
    public static PunchingBagManager Instance { get; private set; }

    [Tooltip("Max bags allowed in the scene at once")]
    public int maxBags = 5;

    private readonly Queue<GameObject> _bags = new Queue<GameObject>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Called by each bag when it spawns/enables.
    /// </summary>
    public void RegisterBag(GameObject bag)
    {
        // Enforce limit
        if (_bags.Count >= maxBags)
        {
            var old = _bags.Dequeue();
            if (old != null) Destroy(old);
        }
        _bags.Enqueue(bag);
    }

    /// <summary>
    /// Called by each bag when it is destroyed/disabled.
    /// </summary>
    public void UnregisterBag(GameObject bag)
    {
        // Remove it if it’s in the queue (won’t error if not present)
        var temp = new Queue<GameObject>();
        while (_bags.Count > 0)
        {
            var b = _bags.Dequeue();
            if (b != bag) temp.Enqueue(b);
        }
        while (temp.Count > 0) _bags.Enqueue(temp.Dequeue());
    }

    /// <summary>
    /// (Optional) Clear all bags immediately.
    /// </summary>
    public void DespawnAll()
    {
        while (_bags.Count > 0)
        {
            var b = _bags.Dequeue();
            if (b != null) Destroy(b);
        }
    }
}
