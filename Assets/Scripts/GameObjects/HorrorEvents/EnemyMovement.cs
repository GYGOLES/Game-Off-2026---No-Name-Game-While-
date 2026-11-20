using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Animator _enemy;
    [SerializeField] Transform _target;
    [SerializeField] NavMeshAgent _agent;
    

    private bool _applyWalk = false;
    private bool _setMovement = false;
    private void ApplyMovement(bool t)
    {
        _enemy.SetBool("isWalk", t);
        _applyWalk = t;
        _agent.isStopped = !t;
    }

    public void SetMovement() => _setMovement = true;

    private void Distanation()
    {
        float dist = (_agent.transform.position - _target.transform.position).magnitude;

    

        if(dist <= 5 && _applyWalk)
        {
            ApplyMovement(false);
        }

        if(dist > 5 && !_applyWalk)
        {
            ApplyMovement(true);
        }
        if(_applyWalk)
        _agent.SetDestination(_target.position);
    }

    void Update()
    {
        if(!_setMovement)return;
        Distanation();
    }
}
