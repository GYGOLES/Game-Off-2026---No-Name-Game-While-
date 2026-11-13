
using System;
using UnityEngine;

public class GarbageLocator : MonoBehaviour
{
    ItemsCollect[] _items;
    int _remaining;
    bool _inited;

    public int Total => _items.Length;
    public event Action<int, int> Changed; 
    public event Action AllCollected;

    void Awake() => Init();


    void Init()
    {
        if(!_inited)
            _inited = true;

        _items = GetComponentsInChildren<ItemsCollect>(true);
        _remaining = _items.Length;

        foreach (var item in _items)
            item.Collected += OnCollected;
    }
    void OnDestroy()
    {
        if (_items == null) return;
        foreach (var item in _items)
            if (item != null) item.Collected -= OnCollected;
    }
    private void OnCollected(ItemsCollect _)
    {
        _remaining--;
        Changed?.Invoke(_remaining, Total);

        if (_remaining <= 0)
            AllCollected?.Invoke();
            
    }

    public void RestartLocator()
    {
        _items = GetComponentsInChildren<ItemsCollect>(true);
        _remaining = _items.Length;
        
        foreach (var item in _items)
        {
            item.gameObject.SetActive(true);
            item.enabled = true;
        }     
    }
}
