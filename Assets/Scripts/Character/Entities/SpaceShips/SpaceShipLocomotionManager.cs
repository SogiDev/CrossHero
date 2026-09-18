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

            forwardRay.direction = transform.position + transform.right;
            forwardRay.direction = transform.position + transform.right * 20;
        }

        internal override void HandleMovement()
        {

            moveAmount.x = 1;
            moveAmount.y = 0;
            /*
            // Check if any object is infront of us
            if (Physics2D.Raycast(forwardRay.origin, forwardRay.direction, 20, LayerMask.GetMask("Environment")) is var hit)
            {
                if (hit.collider == null) return;
                if (hit.distance <= 5)
                {
                    moveAmount.y = 5.0f;
                }
            }
            else
            {
                moveAmount.y = 0.0f;
            }
            */
            
            
            if (Physics2D.Raycast(forwardRay.origin, forwardRay.direction, 20, LayerMask.GetMask("Turret")) is var turret)
            {
                // Stop and Move at Half Speed
                moveAmount.x *= 0.5f;
            }

            base.HandleMovement();
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, forwardRay.direction);
        }

    }
}