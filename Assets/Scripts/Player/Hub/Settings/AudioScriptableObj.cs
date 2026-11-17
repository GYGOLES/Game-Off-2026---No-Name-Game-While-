using UnityEngine;

[CreateAssetMenu(fileName = "Audio", menuName = "Settings/Audio")]
public class AudioScriptableObj : ScriptableObject
{
    [Header("Settings")]
    [SerializeField, Range(0f, 1f)] float volume = 0.2f;
    [SerializeField, Range(0.01f, 3f)] float pitch = 1;
    [SerializeField, Range(0.01f, 3f)] float _minPitch = 0.8f;
    [SerializeField] bool _randomPitch = false;

    [Header("Audio")]
    [SerializeField] AudioClip[] arrayClips;
    [SerializeField] AudioClip clip;

    public float Volume => volume;
    public float Pitch => pitch;
    public float MinPitch => _minPitch;

    public bool RandomPitch => _randomPitch;

    public AudioClip[] ArraClips => arrayClips;
    public AudioClip Clip => clip;

    void OnValidate() => pitch = Mathf.Max(pitch, _minPitch);
}
