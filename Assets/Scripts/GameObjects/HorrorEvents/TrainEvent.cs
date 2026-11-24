using PrimeTween;
using UnityEngine;
using VContainer;

public class TrainEvent : MonoBehaviour
{
    [Header("AUDIO")]
    [SerializeField] AudioSource _source;
    [SerializeField] float _volume;

    [Header("Transform")]
    [SerializeField] Transform _trainTransform;
    [SerializeField] Transform _targetDistance;
    [SerializeField] float _duration = 7f;

    [Header("ID")]
    [SerializeField] int _id;

    private Tween _train;
    private Tween _volumeTween;


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
        _fear.OnJumpscareTriggered += OnMovePosition;
    }

    private void OnMovePosition(JumpscareSystem s)
    {
        if(s.JumpscareType != JumpscareType.Train)return;
        if(s.ID != _id)return;

        _train.Stop();
        _source.Stop();


        _source.volume = _volume;    
        _source.Play();

        _train = Tween.Position(_trainTransform, _trainTransform.position, _targetDistance.position, _duration).OnComplete(() => 
        {
            _source.Stop( );
            _trainTransform.position = _startPosition;
            gameObject.SetActive(false);
        });

        _volumeTween = Tween.AudioVolume(_source, 0f, 1.5f, startDelay: _duration - 1.5f).OnComplete(() => {_source.Stop(); _source.volume = _volume;});
    }
    void OnDisable()
    {   
        if(_fear != null)
        _fear.OnJumpscareTriggered -= OnMovePosition;
        _trainTransform.position = _startPosition;

        _train.Stop();
        _volumeTween.Stop();
    }
}
