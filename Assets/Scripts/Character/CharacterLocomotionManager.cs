using UnityEngine;

namespace DS
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterLocomotionManager : MonoBehaviour
    {

        private CharacterManager character;
        protected Rigidbody2D rigidBody;
        protected Vector2 moveAmount = Vector2.zero;

        [Header("Status")]
        [SerializeField] protected float jumpForce = 10f;

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
            character.currentSpeed = character.isSprinting ? character.BaseSpeed * 1.5f : character.isCrouching ? character.BaseSpeed * 0.5f : character.BaseSpeed;
            rigidBody.linearVelocityX = moveAmount.x * Time.deltaTime * character.currentSpeed * 100;
            rigidBody.linearVelocityY = moveAmount.y * Time.deltaTime * character.currentSpeed * 100;
        }


        internal void HandleGrounded()
        {
            character.isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, LayerMask.GetMask("Environment"));
            if (character.isGrounded) { character.isJumping = false; }
        }
        internal virtual void HandleJump()
        {
            if (character.isJumping) return;
            if (!character.isGrounded) return;

            character.isJumping = true;
            rigidBody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}