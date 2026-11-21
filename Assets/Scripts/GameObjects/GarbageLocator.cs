
using System;
using UnityEngine;

public class GarbageLocator : MonoBehaviour
{
    ItemsCollect[] _items;
    PapperShow[] _info;

    private int _quota;
    int _remaining;
    bool _inited;
    private bool _quotaCollected;

    public int Total => _items.Length;
    public event Action<int, int> Changed; 
    public event Action AllCollected;
    public event Action<bool> QuotaCollected;

    void Awake() => Init();


    void Init()
    {
        if(!_inited)
            _inited = true;

        _items = GetComponentsInChildren<ItemsCollect>(true);
        _info = GetComponentsInChildren<PapperShow>(true);
        _remaining = _items.Length;
        _quota = _items.Length/2;

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

        if(!_quotaCollected &&_remaining <= _quota)
        {
            QuotaCollected?.Invoke(true);
            _quotaCollected = true;
        }
        else
        {
            QuotaCollected?.Invoke(false);
            _quotaCollected = false;
        }

        if (_remaining <= 0)
        {
            AllCollected?.Invoke();
            QuotaCollected?.Invoke(true);
        }
            
            
    }

    public void RestartLocator()
    {
        _items = GetComponentsInChildren<ItemsCollect>(true);
        _info = GetComponentsInChildren<PapperShow>(true);
        
        _remaining = _items.Length;
        _quota = _items.Length/2;

        _quotaCollected = false;
        
        foreach (var item in _items)
        {
            item.gameObject.SetActive(true);
            item.enabled = true;
        }     
        foreach (var item in _info)
        {
            item.gameObject.SetActive(true);
            item.enabled = true;
        }
    }
}
