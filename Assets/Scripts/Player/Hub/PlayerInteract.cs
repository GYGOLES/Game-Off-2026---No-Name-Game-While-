using System;
using Assets.Scripts.Inputmanager;
using UnityEngine;
namespace Assets.Scripts.Player.Hub
{    
    public class PlayerInteract : IDisposable
    {
        private readonly PlayerConfig _config;
        private readonly PlayerCarry _carry;
        private readonly PlayerHand _hand;

        private readonly IPlayerInput _input;
        public PlayerInteract(PlayerConfig config, PlayerCarry carry, IPlayerInput input, PlayerHand hand)
        {
            _config = config;
            _input = input;
            _carry = carry;
            _hand = hand;

            _input.Interact += Interact;
        }

        public void Dispose()
        {
            if(_input == null)return;

            _input.Interact -= Interact;
        }

        void Interact(InputPhase input)
        {
            switch (input)
            {
                case InputPhase.started : Use(); break;
            }
        }

        private void Use()
        {
            if(OnInteract(out RaycastHit hit))
            {
                IInteract interact = hit.transform.GetComponentInParent<IInteract>();
                if(interact == null)return;

                interact.Interact(_carry, _hand);
            }
        }

        public bool OnInteract(out RaycastHit hit)
        {
            Ray ray = _config.Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            return Physics.Raycast(ray, out  hit, _config.InteractRay, _config.InteractLayer, QueryTriggerInteraction.Ignore);
        }
    }
}
