using System;
using UnityEngine;

public class PlayerStates
{
    private readonly PlayerConfig _config;

    private float _currentStamina;
    private float _staminaTimer;
    private bool _staminaOut;

    public float Stamina => _currentStamina;
    public float StaminaNormalized => _currentStamina/_config.MaxStamina;

    public bool StaminaOut => _staminaOut;
    public bool CanSprint => !_staminaOut;


    public event Action<bool> StaminaDepleted;

    public PlayerStates(PlayerConfig config)
    {
        _config = config;

        _currentStamina = _config.MaxStamina;
    }
    public void Tick(PlayerContext ctx, float dt)
    {
        if (_staminaOut)
        {
            _staminaTimer += dt;

            if(_staminaTimer >= 3)
            {
                SetExhausted(false);
            }
        }

        bool canSprint = ctx.IsSprinting && CanSprint;

        if (canSprint)
        {
            StaminaDrain(dt);
        }
        else
        {
            StaminaRegenerate(dt);
        }
    }

    private void StaminaDrain(float dt)
    {
        _currentStamina -= _config.StaminaDrain*dt;

        if(_currentStamina <= 0)
        {
            _currentStamina = 0f;
            SetExhausted(true);
        }
    }
    private void StaminaRegenerate(float dt)
    {
        _currentStamina = Mathf.Min(_currentStamina + _config.StaminaRecover * dt, _config.MaxStamina);
    }
    private void SetExhausted(bool b)
    {
        if(_staminaOut == b)return;


        _staminaOut = b;
        _staminaTimer = 0f;

        StaminaDepleted?.Invoke(b);
    }
}
