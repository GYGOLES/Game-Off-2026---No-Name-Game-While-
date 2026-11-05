using UnityEngine;

[CreateAssetMenu(fileName = "Audio", menuName = "Settings/Audio")]
public class AudioScriptableObj : ScriptableObject
{
    [SerializeField] float volume;
    [SerializeField] AudioClip[] arrayClips;
    [SerializeField] AudioClip clip;

    public float Volume => volume;
    public AudioClip[] ArraClips => arrayClips;
    public AudioClip Clip => clip;
}
