using System;
using UnityEngine;

[Serializable]
public class BackgroundSounds
{
    public AudioSource[] Sources;
    public bool IgnoreAudioListener;

    public void SetIgnore()
    {
        if(Sources.Length <= 0)return;

        for (int i = 0; i < Sources.Length; i++)
        {
            Sources[i].ignoreListenerPause = IgnoreAudioListener;
        }
    }
}