using System.Collections;
using UnityEngine;
using VContainer;

public class LightEvent : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] Light[] lights;
    [SerializeField] float intensity = 8f;
    [SerializeField] float range = 25;

    [Header("Settings")]
    [SerializeField] float minInterval = 0.05f;
    [SerializeField] float maxInterval = 0.3f;
    [SerializeField] float duration = 5f;
    [SerializeField] bool applyReset;
    [Inject] PlayerFear _fear;


    private bool _blinkingLight;
    private float _timer = 0f;
    private float _blinkTimer = 0f;
    private float _nextBlinkInterval;
    void OnEnable()
    {
        if(_fear == null)return;
        _fear.Ambience += LightsOff;

    }
    void OnDisable()
    {
        if(_fear == null)return;
        _fear.Ambience -= LightsOff;

    }

    private void LightsOff()
    {
        _timer = 0f;
        _blinkTimer = 0f;
        _blinkingLight = true;
        _nextBlinkInterval = RandomInterval;
    }

    private float RandomInterval => 
    Random.Range(Mathf.Lerp(maxInterval, minInterval, _fear.FearNormalized),Mathf.Lerp(maxInterval * 2, minInterval, _fear.FearNormalized));
    private void BlinkingLight()
    {
        if(lights.Length == 0)return;

        if(applyReset)
        ResetLight();

        int i = Random.Range(0 ,lights.Length);

        lights[i].intensity = Mathf.Lerp(lights[i].intensity ,Random.Range(1, intensity), Time.deltaTime*2);
        lights[i].range = Random.Range(5, range);

        lights[i].enabled = !lights[i].enabled;
    }

    void Update()
    {
        if(!_blinkingLight)return;

        _blinkTimer += Time.deltaTime;
        _timer += Time.deltaTime;


        if(_blinkTimer >= _nextBlinkInterval)
        {
            _blinkTimer = 0f;
            BlinkingLight();

            _nextBlinkInterval = RandomInterval;
        }

        if(_timer >= duration )
        {
            ResetLight();
            _blinkingLight = false;
        }
        
    }

    void ResetLight()
    {
        foreach(var i in lights)
        {
            i.enabled = true;
            i.intensity = intensity;
            i.range = range;
        }
    }
}
