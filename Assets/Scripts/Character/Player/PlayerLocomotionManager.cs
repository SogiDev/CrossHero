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

            player.currentSpeed = player.isSprinting ? player.baseSpeed * 1.5f : player.isCrouching ? player.baseSpeed * 0.5f : player.baseSpeed;

            rigidBody.linearVelocityX = moveAmount.x * Time.deltaTime * player.currentSpeed * 100;

        }
    }
}