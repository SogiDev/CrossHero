using UnityEngine;

namespace DS
{

    public class SpaceShipLocomotionManager : CharacterLocomotionManager
    {
        private SpaceShipManager manager;
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
            manager = GetComponent<SpaceShipManager>();
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
            if (combatManager.FindTargetInRange())


            if (combatManager.FindTargetInRange() is var target)
            {
                if (target.TryGetComponent<Crystal>(out var crystal))
                {
                    return 0.01f;
                }
                else if (target.CompareTag("Turret"))
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