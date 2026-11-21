
using System;
using UnityEngine;

public class GarbageLocator : MonoBehaviour
{
    ItemsCollect[] _items;
    PapperShow[] _info;

    private int _quota;
    int _remainingItem;
    int _remainingPaper;

    bool _inited;
    private bool _quotaCollected;

    public int TotalItem => _items.Length;
    public int TotalPaper => _info.Length;
    public event Action<int, int> Changed; 
    public event Action<int, int> InfoChanged;


    public event Action AllCollected;
    public event Action<bool> QuotaCollected;

    void Awake() => Init();


    void Init()
    {
        if(!_inited)
            _inited = true;

        _items = GetComponentsInChildren<ItemsCollect>(true);
        _info = GetComponentsInChildren<PapperShow>(true);
        _remainingItem = _items.Length;
        _remainingPaper = _info.Length;

        _quota = _items.Length/2;

        foreach (var item in _items)
            item.Collected += OnCollected;

        foreach(var item in _info)
            item.OnCollected += OnCollectedPaper;
    }
    void OnDestroy()
    {
        if (_items == null) return;
        foreach (var item in _items)
            if (item != null) item.Collected -= OnCollected;

        if(_info == null)return;
        foreach(var item in _info)
            item.OnCollected -= OnCollectedPaper;
    }
    private void OnCollected(ItemsCollect _)
    {
        _remainingItem--;
        Changed?.Invoke(_remainingItem, TotalItem);

        if(!_quotaCollected &&_remainingItem <= _quota)
        {
            QuotaCollected?.Invoke(true);
            _quotaCollected = true;
        }
        else
        {
            QuotaCollected?.Invoke(false);
            _quotaCollected = false;
        }

        if (_remainingItem <= 0)
        {
            AllCollected?.Invoke();
            QuotaCollected?.Invoke(true);
        }       
    }

    public void OnCollectedPaper(PapperShow c)
    {
        _remainingPaper--;
        InfoChanged?.Invoke(_remainingPaper, TotalPaper);
    }

    public void RestartLocator()
    {
        _items = GetComponentsInChildren<ItemsCollect>(true);
        _info = GetComponentsInChildren<PapperShow>(true);
        
        _remainingItem = _items.Length;
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
