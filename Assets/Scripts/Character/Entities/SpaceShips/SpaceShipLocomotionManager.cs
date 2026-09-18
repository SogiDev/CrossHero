using UnityEngine;

namespace DS
{
    public class SpaceShipLocomotionManager : CharacterLocomotionManager
    {
        private Ray2D forwardRay;

        protected override void Awake()
        {
            base.Awake();
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            base.Start();
            rigidBody.gravityScale = 0;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            forwardRay.origin = transform.position + transform.right;
            forwardRay.direction = transform.position + transform.right * 10.0f;
        }

        internal override void HandleMovement()
        {

            moveAmount.x = 1;
            
            if (Physics2D.Raycast(forwardRay.origin, forwardRay.direction, 20, LayerMask.GetMask("Turret")) is var turret && turret.collider != null)
            {
                // Stop and Move at Half Speed
                moveAmount.x = 0.5f;
            }
            
            character.currentSpeed = character.isSprinting ? character.BaseSpeed * 1.5f : character.isCrouching ? character.BaseSpeed * 0.5f : character.BaseSpeed;
            rigidBody.linearVelocityX = moveAmount.x * Time.deltaTime * character.currentSpeed * 100;
            rigidBody.linearVelocityY = 0;
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, forwardRay.direction);
        }

    }
}