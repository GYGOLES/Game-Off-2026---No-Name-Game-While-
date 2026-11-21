using System;

public enum FearState
{
    Fine, Critical, Insane
}

public class PlayerFear
{
    private readonly PlayerConfig _config;

    private float _currentFear;

    public float CurrentFear => _currentFear;
    public float FearNormalized => _currentFear / _config.MaxFear;

    private FearState _currentState;
    public FearState State => _currentState;

    public event Action OnGetInsane;
    public event Action Ambience;
    public event Action FearDrained;
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

        FearDrained?.Invoke();
    }

    public void GetFear(float f)
    {
        _currentFear += f;
        if(_currentFear >= _config.MaxFear)
        {
            _currentFear = _config.MaxFear;
            OnGetInsane?.Invoke();
        }
        Ambience?.Invoke();
    }
}