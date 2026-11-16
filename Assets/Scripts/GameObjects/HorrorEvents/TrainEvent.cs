using PrimeTween;
using UnityEngine;
using VContainer;

public class TrainEvent : MonoBehaviour
{
    [SerializeField] Transform _trainTransform;
    [SerializeField] Transform _targetDistance;
    [SerializeField] float _duration = 7f;

    private Tween _train;
    private Vector3 _startPosition;

    private PlayerFear _fear;
    [Inject]
    public void Construct(PlayerFear f) => _fear = f;
    void Start()
    {
        _startPosition = _trainTransform.position;
    }

    void OnEnable()
    {
        if(_fear != null)
        _fear.Ambience += OnMovePosition;
    }

    private void OnMovePosition(float f)
    {
        _train.Stop();
        _train = Tween.Position(_trainTransform, _trainTransform.position, _targetDistance.position, _duration).OnComplete(() => 
        {
            _trainTransform.position = _startPosition;
        });
    }
    void OnDisable()
    {   
        if(_fear != null)
        _fear.Ambience -= OnMovePosition;
        _trainTransform.position = _startPosition;

        _train.Stop();
    }
}
