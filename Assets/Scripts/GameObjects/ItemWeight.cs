using Assets.Scripts.Inputmanager;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/Settings")]
public class ItemWeight : ScriptableObject
{
    public string ItemName;
    public float Weight;  
    public bool ALotOfGarbage = false;  
    public InputPhase Phase = InputPhase.started;
}
