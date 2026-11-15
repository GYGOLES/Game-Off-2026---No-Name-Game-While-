using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "Paper", menuName = "Item/Paper")]
public class PaperInfo : ScriptableObject
{
    [SerializeField] Sprite _spirte;
    [SerializeField] LocalizedString _text;

    public Sprite Sprite => _spirte;
    public LocalizedString Text => _text;
}
