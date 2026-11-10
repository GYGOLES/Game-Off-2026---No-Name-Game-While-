using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class HeadBobingEffect : MonoBehaviour
{
    [Serializable]
    private class BobSettings
    {
        public float Frequency = 8f;
        public float VerticalAmplitude = 0.02f;
        public float SideAmplitude = 0.01f;
        public float TiltMove = 0.6f;
    }

    [Header("Links")]
    [SerializeField] Transform _head;
    [SerializeField] Transform _hand;

    [Header("Walk / Sprint")]
    [SerializeField] BobSettings _walk;
    [SerializeField] BobSettings _sprint;

    [Header("Idle (Perlin)")]
    [SerializeField] float _idleSpeed = 0.4f;
    [SerializeField] float _idleAmplitude = 0.01f;
    [SerializeField] float _idleRoll = 0.4f;      

    [Header("Blend")]
    [SerializeField] float _blendTime = 0.25f;

    private Vector3 _startPos;
    private Quaternion _startRot;

    private Vector3 _startPosHand;
    private Quaternion _startRotHand;

    private float _walkTimer;
    private float _idleTime;
    private float _weight, _weightVel;
    private float _seedX, _seedY, _seedR;

    void Awake()
    {
        _startPos = _head.localPosition;
        _startRot = _head.localRotation;

        _startPosHand = _hand.localPosition;
        _startRotHand = _hand.localRotation;

        _seedX = Random.value * 100f;
        _seedY = Random.value * 100f;
        _seedR = Random.value * 100f;
    }

    static float Noise(float t, float seed) => Mathf.PerlinNoise(t, seed) * 2f - 1f;

    public void Tick(PlayerContext ctx, float dt)
    {
        bool moving = ctx.currentInput.sqrMagnitude > 0.0001f;
        _weight = Mathf.SmoothDamp(_weight, moving ? 1f : 0f, ref _weightVel, _blendTime, Mathf.Infinity, dt);

        _idleTime += _idleSpeed * dt;

        float idleX = Noise(_idleTime, _seedX);
        float idleY = Noise(_idleTime, _seedY);
        float idleR = Noise(_idleTime * 0.6f, _seedR);


        Vector3 idle = new Vector3(idleX, idleY , 0f) * _idleAmplitude;
        float roll = idleR * _idleRoll;

        var s = ctx.IsSprinting ? _sprint : _walk;
        if (_weight > 0.0001f)
            _walkTimer = Mathf.Repeat(_walkTimer + s.Frequency * dt, Mathf.PI * 2f);

        float x = Mathf.Cos(_walkTimer) * s.SideAmplitude;
        float y = Mathf.Sin(_walkTimer * 2f) * s.VerticalAmplitude;

        Vector3 bobPosition = new Vector3(x, y, 0f);
        Vector3 finalPos = Vector3.Lerp(idle, bobPosition, _weight);

        float finalRotation = Mathf.Lerp(roll , Mathf.Sin(_walkTimer)*s.TiltMove, _weight);

        _head.localPosition = _startPos + finalPos;
        _head.localRotation = _startRot * Quaternion.Euler(0f, 0f, finalRotation);

        _hand.localPosition = _startPosHand + finalPos;
        _hand.localRotation = _startRotHand * Quaternion.Euler(0f, 0f, finalRotation);
    }
}