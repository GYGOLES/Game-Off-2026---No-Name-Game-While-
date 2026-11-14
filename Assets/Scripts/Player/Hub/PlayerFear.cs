using System;
using UnityEngine;

public class PlayerFear
{
    private readonly PlayerConfig _config;

    private float _currentFear;

    public float CurrentFear => _currentFear;
    public float FearNormalized => _currentFear / _config.MaxFear;


    public event Action OnGetInsane;
    public event Action Ambience;
    public event Action Effects;
    public PlayerFear(PlayerConfig config)
    {
        _config = config;

        _currentFear = _config.MaxFear;
    }

    public void DrainFear(float f)
    {
        _currentFear -= f;
        if(_currentFear <= 0)
            _currentFear = 0f;

        ActionRegister();
    }

    public void GetFear(float f)
    {
        _currentFear += f;
        if(_currentFear >= 100f)
        {
            _currentFear = 100f;
            OnGetInsane?.Invoke();
        }
        ActionRegister();
    }


    private void ActionRegister()
    {
        if(_currentFear <= 26 && _currentFear <= 50)
        {
            Ambience?.Invoke();
        }
        else if (_currentFear <= 51 && _currentFear <= 75)
        {
            Ambience?.Invoke();
        }
        else if(_currentFear <= 76 && _currentFear <= 99)
        {
            Effects?.Invoke();
        }
    }
}