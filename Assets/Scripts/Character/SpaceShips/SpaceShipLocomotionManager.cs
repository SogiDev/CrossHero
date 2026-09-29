using UnityEngine;

namespace DS
{
    public class SpaceShipLocomotionManager : CharacterLocomotionManager
    {
        private SpaceShipCombatManager combatManager;
        private Ray2D forwardRay;
        [SerializeField] private float slowDownRange = 20;
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
            //searchRadius = combatManager.GetSearchRadius();
        }

        // Update is called once per frame
        protected override void Update()
        {
            forwardRay.origin = transform.position + transform.right;
            forwardRay.direction = transform.position + transform.right * 10.0f;
        }

        internal override void HandleMovement()
        {
            moveAmount = moveDirection;
            moveAmount *=
                (Physics2D.OverlapCircle(transform.position, searchRadius, LayerMask.GetMask("Entity")) is var hit && hit.gameObject.CompareTag("Turret")) ? 0.5f
                : combatManager.IsTargetInRange("Crystal") ? 0.01f
                : 1.0f;

            /*
            if (Physics2D.OverlapCircle(transform.position, searchRadius, LayerMask.GetMask("Entity")) is var hit)
            {
                if (hit == null) { return; }
                if (hit.gameObject == null) { return; }

                moveAmount.x = hit.gameObject.CompareTag("Turret") ? 0.5f : 1.0f;
            }
            if (combatManager.IsTargetInRange("Crystal"))
            {
                moveAmount.x *= 0.01f;
            }
            else
            {
                moveAmount.x *= 1.0f;
            }
            */
            character.currentSpeed = character.isSprinting ? character.BaseSpeed * 1.5f :
                character.isCrouching ? character.BaseSpeed * 0.5f :
                character.BaseSpeed;

            rigidBody.linearVelocityX = moveAmount.x * Time.deltaTime * character.currentSpeed * 100;
            rigidBody.linearVelocityY = 0;
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, searchRadius);
        }

    }
}