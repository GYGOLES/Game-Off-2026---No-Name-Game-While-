using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "Paper", menuName = "Item/Paper")]
public class PaperInfo : ScriptableObject
{
    [SerializeField] int _indexPaper;
    [SerializeField] LocalizedString _text;
    [SerializeField] float _fear;

    public int Index => _indexPaper;
    public LocalizedString Text => _text;
    public float Fear => _fear;
}
