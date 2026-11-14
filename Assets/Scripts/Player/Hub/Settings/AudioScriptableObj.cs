using UnityEngine;

[CreateAssetMenu(fileName = "Audio", menuName = "Settings/Audio")]
public class AudioScriptableObj : ScriptableObject
{
    [Header("Settings")]
    [SerializeField] float volume = 0.2f;
    [SerializeField] float pitch = 1;

    [Header("Audio")]
    [SerializeField] AudioClip[] arrayClips;
    [SerializeField] AudioClip clip;

    public float Volume => volume;
    public float Pitch => pitch;

    public AudioClip[] ArraClips => arrayClips;
    public AudioClip Clip => clip;
}
