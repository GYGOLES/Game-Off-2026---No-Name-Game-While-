using NUnit.Framework.Constraints;
using PrimeTween;
using UnityEngine;
using VContainer;

public class EnemyMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Animator _enemy;
    [SerializeField] Transform _target;
    [SerializeField] float _duration = 5;

    private Vector3 _startPosition;
    private Tween _walkTween;

    void OnEnable()
    {
        _startPosition = transform.position;
    }
    void OnDisable()
    {
        transform.position = _startPosition;
        _walkTween.Stop();
    }

    public void Walk()
    {
        _enemy.SetBool("isWalk", true);

        _walkTween = Tween.Position(transform, transform.position, Velocity(_target.position, _startPosition), _duration).OnComplete(() => gameObject.SetActive(false));
    }

    private static Vector3 Velocity(Vector3 v, Vector3 s) => new Vector3(v.x, s.y , v.z);
}
