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

        internal override void HandleMovement()
        {
            base.HandleMovement();
            moveAmount = new Vector2(inputManager.movementInput.x, inputManager.movementInput.y);
        }

    }
}