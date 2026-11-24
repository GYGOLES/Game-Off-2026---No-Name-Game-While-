using UnityEngine;
public enum JumpscareType
{
    None, TheGuy, Train, LightBlinking
}
[System.Serializable]
public class JumpscareSystem
{
    [SerializeField] int _id;
    [SerializeField] JumpscareType jumpscareType;

    public int ID => _id;
    public JumpscareType JumpscareType => jumpscareType;
}