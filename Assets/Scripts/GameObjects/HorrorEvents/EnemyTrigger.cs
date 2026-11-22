using UnityEngine;
using VContainer;

public class EnemyTrigger : MonoBehaviour
{
    [SerializeField] EnemyMovement _enemy;
    [SerializeField] bool _activateFromFront = true;
    [Inject] PlayerConfig _config;

    void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player"))return;
        if(_config == null)return;

        float dot = Vector3.Dot(_config.Camera.transform.forward, gameObject.transform.forward);

        if (_activateFromFront && dot <= 0) return;
        if (!_activateFromFront && dot >= 0) return;
        
        _enemy.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
