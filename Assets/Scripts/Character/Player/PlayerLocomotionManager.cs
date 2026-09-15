using UnityEngine;

namespace DS
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        [Header("Systems")]
        private PlayerInputManager inputManager;
        private PlayerManager player;


        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            // Get Scripts
            inputManager = PlayerInputManager.Instance;
            player = GetComponent<PlayerManager>();
        }

        internal override void HandleMovement()
        {
            moveAmount = new Vector2(inputManager.movementInput.x, inputManager.movementInput.y);

            currentSpeed = player.isSprinting ? baseSpeed * 1.5f : player.isCrouching ? baseSpeed * 0.5f : baseSpeed;

            rigidBody.linearVelocityX = moveAmount.x * Time.deltaTime * currentSpeed * 100;

        }
    }
}