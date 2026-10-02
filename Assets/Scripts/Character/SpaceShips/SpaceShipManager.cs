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

        public GameObject crystalTarget { get; private set; }

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
            crystalTarget = FindAnyObjectByType<Crystal>().gameObject;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        protected override void HandleMovement()
        {
            spaceShipLocomotionManager.HandleMovement();
        }
        protected override void HandleGrounded()
        {
            
        }

    }
}