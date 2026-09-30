using NUnit.Framework.Internal;
using UnityEngine;

namespace DS
{

    public class SpaceShipLocomotionManager : CharacterLocomotionManager
    {
        private SpaceShipCombatManager combatManager;
        private Ray2D forwardRay;
        public float searchRadius = 10.0f;
        public Vector2 moveDirection = new (1, 0);

        protected override void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            character = GetComponent<SpaceShipManager>();
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            base.Start();
            rigidBody.gravityScale = 0;
            
            combatManager = GetComponent<SpaceShipCombatManager>();
            GetComponent<SpriteRenderer>().flipX = moveDirection.x < 0;

            searchRadius = combatManager.GetSearchRadius();

        }

        internal override void HandleMovement()
        {
            moveAmount = moveDirection;

            moveAmount *= AdjustToEntity();

            character.currentSpeed = character.isSprinting ? character.BaseSpeed * 1.5f :
                character.isCrouching ? character.BaseSpeed * 0.5f :
                character.BaseSpeed;

            rigidBody.linearVelocityX = moveAmount.x * Time.deltaTime * character.currentSpeed * 100;
            rigidBody.linearVelocityY = 0;
        }

        private float AdjustToEntity()
        {
            if (Physics2D.OverlapCircle(transform.position, searchRadius, LayerMask.GetMask("Entity")) is var hit)
            {
                if (hit.gameObject.TryGetComponent<Crystal>(out var crystal))
                {
                    return 0.01f;
                }
                else if (hit.gameObject.CompareTag("Turret"))
                {
                    return 0.5f;
                }
            }

            return 1.0f;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, searchRadius);
        }

    }
}