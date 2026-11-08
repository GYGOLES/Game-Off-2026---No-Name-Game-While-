using System;
using UnityEngine;

public class TimerCounter : MonoBehaviour
{
    [SerializeField] float _timer;

    private float _defaultTimer;
    
    
    public event Action TimerDeath;
    public float CurrentTimer => _timer;

    void Awake()
    {
        _defaultTimer = _timer;
    }
    void Update()
    {
        if(_timer <= 0)return;

        _timer -= Time.deltaTime;

        if(_timer <= 0)
        {
            _timer = 0;
            TimerDeath?.Invoke();
        }
    }

    public void SetTimerBack()
    {
        _timer = _defaultTimer;
    }
}
