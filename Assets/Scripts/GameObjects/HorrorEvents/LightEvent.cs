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
    [Inject] PlayerFear _fear;

    private Coroutine _cor;
    void OnEnable()
    {
        if(_fear == null)return;
        _fear.Ambience += LightsOff;

    }
    void OnDisable()
    {
        if(_fear == null)return;
        _fear.Ambience -= LightsOff;

        if(_cor != null)
        StopCoroutine(_cor);
    }

    private void LightsOff()
    {
        _cor = StartCoroutine(Coroutine());
    }

    public IEnumerator Coroutine()
    {
        float timer = 0f;
        while (timer <= duration)
        {
            timer += Time.deltaTime;

            int i = Random.Range(0 ,lights.Length);


            float perlinIntensity = Random.Range(1, intensity);
            float perlinRange = Random.Range(5, range);

            lights[i].intensity = perlinIntensity;
            lights[i].range = perlinRange;

            lights[i].enabled = !lights[i].enabled;
            
            float interval = Random.Range(minInterval, maxInterval);

            yield return new WaitForSeconds(interval);
            timer += interval;
        }
        ResetLight();
        _cor = null;
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
