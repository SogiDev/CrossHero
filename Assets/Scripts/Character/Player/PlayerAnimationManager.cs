using UnityEngine;

namespace DS
{
    public class PlayerAnimationManager : CharacterAnimationManager
    {
        private PlayerInputManager inputManager;

        protected override void Start()
        {
            // Get Scripts
            inputManager = PlayerInputManager.Instance;
        }

        protected override void Update()
        {
            HandleMovement();
        }

        internal void HandleSprint(bool sprint)
        {
            isSprinting = sprint;
        }

        protected override void HandleMovement()
        {
            moveAmount = new Vector2(inputManager.movementInput.x, inputManager.movementInput.y);

            base.HandleMovement();
        }

    }
}