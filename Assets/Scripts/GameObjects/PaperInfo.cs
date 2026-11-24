using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "Paper", menuName = "Item/Paper")]
public class PaperInfo : ScriptableObject
{
    [Header("Settings")]
    [SerializeField] int _indexPaper;
    [SerializeField] LocalizedString _text;
    [SerializeField] float _fear;

    [Header("Jumpscares")]
    [SerializeField] JumpscareSystem _jumpscareType;

    public JumpscareSystem Jumpscare => _jumpscareType;

    public int Index => _indexPaper;
    public LocalizedString Text => _text;
    public float Fear => _fear;
}
