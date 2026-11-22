
using UnityEngine;
using UnityEngine.Localization;

[System.Serializable]
public sealed class EndingView
{
    [SerializeField] LocalizedString _texts;
    [SerializeField] LocalizedString _stamps;

    public LocalizedString Text => _texts;
    public LocalizedString Stamp => _stamps;
}
