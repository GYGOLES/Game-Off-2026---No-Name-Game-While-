using UnityEngine;
namespace Assets.Scripts.Player.Hub
{    
    public sealed class PlayerHit
    {
        private readonly PlayerConfig _config;
        public PlayerHit(PlayerConfig config)
        {
            _config = config;
        }
        public void Tick(PlayerContext ctx)
        {
            RaycastHit hit;
            float leg = _config.Controller.transform.position.y + (_config.Controller.height*0.5f - _config.Controller.radius);
            Vector3 origin = new Vector3(_config.Controller.transform.position.x, leg, _config.Controller.transform.position.z);
            Physics.Raycast(origin, Vector3.down, out hit, _config.GroundRay, _config.GroundLayer, QueryTriggerInteraction.Ignore );

            ctx.hit = hit;
        }
    }
}
