using System;
using UnityEngine;

public class PlayerFear
{
    private readonly PlayerConfig _config;

    private float _currentFear;

    public float CurrentFear => _currentFear;
    public float FearNormalized => _currentFear / _config.MaxFear;


    public event Action OnGetInsane;
    public event Action<float> Ambience;
    public PlayerFear(PlayerConfig config)
    {
        _config = config;

        _currentFear = 0f;
    }

    public void DrainFear(float f)
    {
        _currentFear -= f;
        if(_currentFear <= 0)
            _currentFear = 0f;

        Ambience?.Invoke(_currentFear);
    }

    public void GetFear(float f)
    {
        _currentFear += f;
        if(_currentFear >= _config.MaxFear)
        {
            _currentFear = _config.MaxFear;
            OnGetInsane?.Invoke();
        }
        Ambience?.Invoke(_currentFear);
    }
}