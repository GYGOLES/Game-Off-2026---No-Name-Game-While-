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

    public FearState State {get; private set;}

    public event Action OnGetInsane;
    public event Action OnFearChange;
    public event Action<FearState> OnFearStateChange;
    public event Action<JumpscareSystem> OnJumpscareTriggered;
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
        if(f <= 0)return;

        _currentFear += f;
        OnFearChange?.Invoke();
        UpdateState();


        if(_currentFear >= _config.MaxFear)
        {
            _currentFear = _config.MaxFear;
            OnGetInsane?.Invoke();
        }
    }

    public void TriggerJumpscare(JumpscareSystem jumpscareSystem, float bonusFear = 0f)
    {
        if(jumpscareSystem.JumpscareType == JumpscareType.None)return;

        OnJumpscareTriggered?.Invoke(jumpscareSystem);
        if (bonusFear > 0f)
        {
            GetFear(bonusFear);
        }
    }

    private void UpdateState()
    {
        FearState s = Evaluate(FearNormalized);
        if(s == State)return;

        State = s;

        OnFearStateChange?.Invoke(State);
    }

    private FearState Evaluate(float normalized)
    {
        if(normalized >= 1)return FearState.Insane;
        if(normalized >= 0.6f)return FearState.Critical;
        
        return FearState.Fine;
    }
}