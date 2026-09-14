using UnityEngine;

namespace DS
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        [Header("Systems")]
        private PlayerInputManager inputManager;


        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            // Get Scripts
            inputManager = PlayerInputManager.Instance;
        }

        protected override void Update()
        {
            HandleMovement();
        }

        internal void HandleSprint(bool isSprinting)
        {
            this.isSprinting = isSprinting;
        }

        protected override void HandleMovement()
        {
            moveAmount = new Vector2(inputManager.movementInput.x, inputManager.movementInput.y);

            currentSpeed = isSprinting ? baseSpeed * 1.5f : isCrouching ? baseSpeed * 0.5f : baseSpeed;

            rigidBody.linearVelocityX = moveAmount.x * Time.deltaTime * currentSpeed * 100;

        }

        public override void HandleJump()
        {
            base.HandleJump();
        }

    }
}