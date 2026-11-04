using System;
using UnityEngine;

[Serializable]
public sealed class PlayerConfig
{
    public const float MinSensitivity = 0.01f;
    public const float MaxSensitivity = 3.5f;

    private const string SensKey = "look.sensitivity";
    private const string InvertYKey = "look.InvertY";

    [Header("Sensitivity")]
    [SerializeField, Range(1f, 89f)] private float _lookLimit = 80f;
    [SerializeField] private float _sensitivity = 0.05f;
    [SerializeField] private bool _invertY = false;

    public float LookLimit => _lookLimit;
    public float Sensitivity => _sensitivity;
    public bool InvertY => _invertY;

    [Header("Movement")]
    [SerializeField, Range(0.1f, 12)] float _walkSpeed = 4f;
    [SerializeField, Range(3f, 12)] float _sprintSpeed = 6.65f;

    public float WalkSpeed => _walkSpeed;
    public float SprintSpeed => _sprintSpeed;

    [Header("Physics")]
    [SerializeField, Range(0, 25f)] float _gravity = 20f;
    [SerializeField, Range(-10f, 0)] float _stayGround = -2f;

    public float Gravity => _gravity;
    public float StayGround => _stayGround;

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