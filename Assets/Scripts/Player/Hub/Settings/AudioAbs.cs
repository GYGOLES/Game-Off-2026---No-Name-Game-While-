using UnityEngine;
using VContainer;

public abstract class AudioAbs : MonoBehaviour
{
    [SerializeField] protected AudioScriptableObj _audio;
    protected AudioClip lastClip;

    protected IAudioService service;
    [Inject]
    public void Construct(IAudioService service)
    {
        this.service = service;
    }
    protected void PlayAtPosition(AudioClip clip,Vector3 pos, float volume = 0.1f)
    {
        if(clip == null)return;
        service.PlaySound(clip ,pos, volume);
    }
    protected void PlaySingleClip(AudioClip clip,Vector3 pos, bool atPosition)
    {
        service.PlaySound(clip,atPosition? pos : transform.position);
    }
    protected AudioClip GetRandomClip(AudioClip[] clips)    
    {
        if(clips.Length == 0)return null;
        if(clips.Length == 1)return clips[0];

        AudioClip clip = clips[Random.Range(0,clips.Length)];
        int attemps = 0;
        while (clip == lastClip&&attemps < 10&&clips.Length > 1)
        {
            clip = clips[Random.Range(0, clips.Length)];
            attemps ++;
        }
        lastClip = clip;
        return clip;
    }
}
