using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    [SerializeField] EnemyMovement _enemy;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        _enemy.Walk();
        
    }
}
