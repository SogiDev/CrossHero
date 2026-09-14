using UnityEngine;

namespace DS
{
    [RequireComponent (typeof(Animator))]
    public class CharacterAnimationManager : MonoBehaviour
    {
        [Header("Systems")]
        public SpriteRenderer spriteRenderer { get; private set; }
        public Animator animator { get; private set; }
        protected Vector2 moveAmount = Vector2.zero;

        [Header("States")]
        protected bool isSprinting;
        protected bool isWalking;
        protected bool isJumping;

        protected virtual void Awake()
        {
            animator = GetComponent<Animator> ();
            spriteRenderer = GetComponent<SpriteRenderer> ();
        }

        protected virtual void Start()
        {
            
        }

        protected virtual void Update()
        {
            HandleMovement();
        }
        protected virtual void HandleMovement()
        {
            animator.SetBool("IsWalking", moveAmount.x != 0);
            animator.SetBool("IsJumping", isJumping);
            isWalking = moveAmount.x != 0;

            if (isWalking)
            {
                spriteRenderer.flipX = moveAmount.x < 0;
            }
        }

    }
}