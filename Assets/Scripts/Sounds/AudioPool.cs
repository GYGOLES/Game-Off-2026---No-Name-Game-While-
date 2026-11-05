using System;
using UnityEngine;

[Serializable]
public class AudioPool
{
    [SerializeField] int _poolSize = 40;
    [SerializeField] AudioSource _prefab;

    public int PoolSize => _poolSize;
    public AudioSource Prefab => _prefab;
}
