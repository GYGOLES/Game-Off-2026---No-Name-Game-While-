using System;
using UnityEngine;

[Serializable]
public sealed class PlayerConfig
{
    public const float MinSensitivity = 0.01f;
    public const float MaxSensitivity = 3.5f;
    
    private const string SensKey = "look.sensitivity";
    private const string InvertYKey = "look.InvertY";

    [SerializeField, Range(1f, 89f)] private float _lookLimit = 80f;
    [SerializeField] private float _sensitivity = 0.05f;
    [SerializeField] private bool _invertY = false;

    public float LookLimit => _lookLimit;
    public float Sensitivity => _sensitivity;
    public bool InvertY => _invertY;

    public void SetSensitivity(float value) => _sensitivity = Mathf.Clamp(value, MinSensitivity, MaxSensitivity);
    public void SetInvertY(bool b) => _invertY = b;

    public void Load()
    {
        SetSensitivity(PlayerPrefs.GetFloat(SensKey, _sensitivity));
        _invertY = PlayerPrefs.GetInt(InvertYKey, _invertY? 1 : 0) == 1;
    }

    public void Save()
    {
        PlayerPrefs.SetFloat(SensKey, _sensitivity);
        PlayerPrefs.SetInt(InvertYKey, _invertY ? 1:0);
        PlayerPrefs.Save();
    }
}