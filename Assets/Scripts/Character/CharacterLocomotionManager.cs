using UnityEngine;

namespace DS
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterLocomotionManager : MonoBehaviour
    {

        protected Rigidbody2D rigidBody;
        protected Vector2 moveAmount = Vector2.zero;

        [Header("States")]
        protected bool isSprinting;
        protected bool isCrouching;
        protected bool isJumping;

        [Header("Status")]
        [SerializeField] protected int baseSpeed = 25;
        protected float currentSpeed = 5;
        public float CurrentSpeed => currentSpeed;
        [SerializeField] protected float jumpForce = 10f;



        protected virtual void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
        }
        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {

        }

        protected virtual void HandleMovement()
        {
            currentSpeed = isSprinting ? baseSpeed * 1.5f : isCrouching ? baseSpeed * 0.5f : baseSpeed;
            rigidBody.linearVelocityX = moveAmount.x * Time.deltaTime * currentSpeed * 100;

        }

        public virtual void HandleJump()
        {
            if (isJumping) return;

            isJumping = true;
            rigidBody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }

    }
}