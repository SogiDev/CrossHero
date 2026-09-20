using UnityEngine;

namespace DS
{
    [RequireComponent(typeof(SpaceShipLocomotionManager))]
    [RequireComponent(typeof(SpaceShipCombatManager))]
    [RequireComponent(typeof(CircleCollider2D))]
    //[RequireComponent(typeof(SpaceShipAnimationManager))]
    public class SpaceShipManager : CharacterManager
    {
        protected SpaceShipLocomotionManager spaceShipLocomotionManager;
        protected SpaceShipCombatManager spaceShipCombatManager;

        protected override void Awake()
        {
            base.Awake();
            spaceShipLocomotionManager = GetComponent<SpaceShipLocomotionManager>();
            spaceShipCombatManager = GetComponent<SpaceShipCombatManager>();
            characterAnimationManager.enabled = false;
        }

        protected override void Start()
        {
            base.Start();
            characterAnimationManager.animator.enabled = false;
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void HandleMovement()
        {
            if (!canMove) { return; }
            spaceShipLocomotionManager.HandleMovement();
        }
        protected override void HandleGrounded()
        {
            
        }

    }
}