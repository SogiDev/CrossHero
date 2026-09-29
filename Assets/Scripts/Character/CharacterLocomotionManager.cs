using UnityEngine;

namespace DS
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterLocomotionManager : MonoBehaviour
    {

        protected CharacterManager character;
        protected Rigidbody2D rigidBody;
        [SerializeField] protected Vector2 moveAmount = Vector2.zero;

        [Header("Status")]
        [SerializeField] protected float jumpForce = 10f;
        [SerializeField] protected float groundedDistance = 0.2f;
        [SerializeField] private LayerMask groundLayer;
        

        protected virtual void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            character = GetComponent<CharacterManager>();
        }
        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {
            HandleGrounded();
        }

        internal virtual void HandleMovement()
        {
            if (!character.canMove) { return; }
            character.currentSpeed = character.isSprinting ? character.BaseSpeed * 1.5f : character.isCrouching ? character.BaseSpeed * 0.5f : character.BaseSpeed;
            rigidBody.linearVelocityX = moveAmount.x * Time.deltaTime * character.currentSpeed * 100;
            rigidBody.linearVelocityY = moveAmount.y * Time.deltaTime * character.currentSpeed * 100;
        }


        internal void HandleGrounded()
        {
            if (!character.canMove) { return; }
            character.isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundedDistance, LayerMask.GetMask("Environment"));
            if (character.isGrounded) { character.isJumping = false; }
        }
        internal virtual void HandleJump()
        {
            if (!character.canJump) { return; }
            if (!character.isGrounded) return;
            if (character.isJumping) return;

            character.isJumping = true;
            rigidBody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.aquamarine;
            //Gizmos.DrawRay(transform.position, Vector2.down);
            Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * groundedDistance));
        }
    }
}