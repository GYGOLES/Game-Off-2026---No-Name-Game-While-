using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    [SerializeField] CharacterController _controller;
    [SerializeField] float dist = 0.15f;
    [SerializeField] LayerMask mask;
    public void Tick(PlayerContext ctx)
    {
        RaycastHit hit;
        float leg = _controller.transform.position.y + (_controller.height*0.5f - _controller.radius);
        Vector3 origin = new Vector3(_controller.transform.position.x, leg, _controller.transform.position.z);
        Physics.Raycast(origin, Vector3.down, out hit, dist, mask, QueryTriggerInteraction.Ignore );

        ctx.hit = hit;
    }
}
