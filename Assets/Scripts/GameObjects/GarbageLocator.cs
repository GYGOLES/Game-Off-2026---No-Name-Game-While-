
using UnityEngine;

public class GarbageLocator : MonoBehaviour
{
    int _garbage;

    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _garbage = i;
        }   
        
    }
}
