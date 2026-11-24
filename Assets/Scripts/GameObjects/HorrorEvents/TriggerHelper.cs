using UnityEngine;

public class TriggerHelper : MonoBehaviour
{
    [SerializeField] EnemyTrigger[] _triggers;

    void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player"))return;

        for (int i = 0; i < _triggers.Length; i++)
        {
            _triggers[i].gameObject.SetActive(true);
        }
    }
}
