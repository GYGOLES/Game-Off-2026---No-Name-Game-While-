using UnityEngine;

public class GarbageBagSpawner : MonoBehaviour
{
    [SerializeField] Garbage_Bag[] _bags;


    public void RespawnAll()
    {
        foreach (var item in _bags)
        {
            if(item != null)
                item.gameObject.SetActive(true);
        }
    }
}
