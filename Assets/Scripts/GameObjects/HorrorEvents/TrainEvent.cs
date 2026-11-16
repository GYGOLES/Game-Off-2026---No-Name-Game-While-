using PrimeTween;
using UnityEngine;

public class TrainEvent : MonoBehaviour
{
    [SerializeField] Transform _trainTransform;
    [SerializeField] Transform _targetDistance;
    [SerializeField] float _durance;

    private Tween _train;
    private Vector3 _startPosition;
    void Start()
    {
        _startPosition = _trainTransform.position;
    }

    private void OnMovePosition()
    {
        if (!_trainTransform.gameObject.activeSelf)
        {
           _trainTransform.gameObject.SetActive(true);
        }

        _train = Tween.Position(_trainTransform, _trainTransform.position, _targetDistance.position, _durance).OnComplete(() => 
        {
            _trainTransform.gameObject.SetActive(false);
            _trainTransform.position = _startPosition;
        });
    }

    

    void OnDisable() => _train.Stop();
}
