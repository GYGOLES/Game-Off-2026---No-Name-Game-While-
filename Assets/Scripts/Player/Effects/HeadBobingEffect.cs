using UnityEngine;

public class HeadBobingEffect : MonoBehaviour
{
    [SerializeField] Transform _head;

    [SerializeField] float _frequency = 0.015f;
    [SerializeField] float _acceleration = 15;

    float _weight;
    Vector3 _startpos;
    float _timer = 0f;
    void Awake()
    {
        _startpos = _head.localPosition;
    }

    public void Tick(PlayerContext ctx, float dt)
    {
        float mag = ctx.currentInput.sqrMagnitude;

        float targetW = mag > 0.0001f? 1 : 0;
        _weight = Mathf.MoveTowards(_weight, targetW, dt);

        if(mag > 0.001f)
        {
            _timer += dt*_acceleration;
        }
        else
        {
            _timer = 0f;
        }

        float sin = Mathf.Sin(_timer)*_frequency*_weight;
        float cos = Mathf.Sin(_timer*0.5f)*_frequency*_weight;

        _head.localPosition = _startpos + new Vector3(cos, sin, 0f);
    }
}