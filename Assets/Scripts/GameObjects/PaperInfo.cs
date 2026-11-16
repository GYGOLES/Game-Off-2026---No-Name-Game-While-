using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "Paper", menuName = "Item/Paper")]
public class PaperInfo : ScriptableObject
{
    [SerializeField] Sprite _spirte;
    [SerializeField] LocalizedString _text;
    [SerializeField] float _fear;

    public Sprite Sprite => _spirte;
    public LocalizedString Text => _text;
    public float Fear => _fear;
}
