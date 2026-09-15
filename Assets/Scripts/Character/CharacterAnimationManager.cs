using UnityEditor.Animations;
using UnityEngine;

namespace DS
{
    [RequireComponent (typeof(Animator))]
    public class CharacterAnimationManager : MonoBehaviour
    {
        private CharacterManager character;
        [Header("Systems")]
        public SpriteRenderer spriteRenderer { get; private set; }
        public Animator animator { get; private set; }
        protected Vector2 moveAmount = Vector2.zero;


        protected virtual void Awake()
        {
            // Get Local Dependancies
            character = GetComponent<CharacterManager> ();
            animator = GetComponent<Animator> ();
            spriteRenderer = GetComponent<SpriteRenderer> ();
        }

        protected virtual void Start()
        {
            // Get Global Dependancies
        }

        protected virtual void Update()
        {
            animator.SetBool("IsWalking", moveAmount.x != 0);
            animator.SetBool("IsJumping", character.isJumping);
        }

        public virtual void HandleCloseAttack()
        {
            animator.SetTrigger("CloseAttack");
        }
        public virtual void HandleRangedAttack()
        {
            animator.SetTrigger("RangedAttack");
        }
        public virtual void HandleSupportAttack()
        {
            animator.SetTrigger("SupportAttack");
        }

        internal virtual void HandleMovement()
        {
            character.isWalking = moveAmount.x != 0;

            if (character.isWalking)
            {
                spriteRenderer.flipX = moveAmount.x < 0;
            }
        }

        internal virtual void HandleGrounded(bool grounded)
        {
            animator.SetBool("IsGrounded", grounded);
        }

        internal void HandleJump()
        {
            animator.Play("Jump");
        }

    }
}