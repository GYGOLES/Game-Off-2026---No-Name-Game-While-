using UnityEngine;

public class TrainTrigger : MonoBehaviour
{
    [SerializeField] TrainEvent @event;


    void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player"))return;

        @event.gameObject.SetActive(true);
    }
}
