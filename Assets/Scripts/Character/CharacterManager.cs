using Unity.VisualScripting;
using UnityEngine;

namespace DS
{
    [RequireComponent(typeof(CharacterLocomotionManager))]
    [RequireComponent(typeof(CharacterAnimationManager))]
    public class CharacterManager : MonoBehaviour
    {
        [Header("Systems")]
        protected CharacterLocomotionManager characterLocomotionManager;
        protected CharacterAnimationManager characterAnimationManager;
        protected CharacterCombatManager characterCombatManager;

        [Header("States")]
        internal bool isSprinting;
        internal bool isWalking, isCrouching, isJumping, isGrounded, isAttacking;

        [Header("Status")]
        [SerializeField] private int maxHealth = 10;
        [SerializeField] private float health = 10;
        public float Health => health;

        [SerializeField] private int maxEnergy = 10;
        [SerializeField] private float energy = 10;
        public float Energy => energy;


        protected virtual void Awake()
        {
            characterLocomotionManager = GetComponent<CharacterLocomotionManager>();
            characterAnimationManager = GetComponent<CharacterAnimationManager>();
            characterCombatManager = GetComponent<CharacterCombatManager>();
        }
        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {
            HandleMovement();
            HandleGrounded();
        }

        public void TakeDamage(float damage)
        {
            // Make Sound
            characterAnimationManager.animator.SetTrigger(Animator.StringToHash("OnHit"));
            health -= damage;
            if (health <= 0)
            {
                // Play Death Animation
                // Play Death Sound
                Destroy(gameObject, 3.0f);
            }
        }

        private void HandleGrounded()
        {
            characterLocomotionManager.HandleGrounded();
            characterAnimationManager.HandleGrounded(isGrounded);
        }

        private void HandleMovement()
        {
            characterLocomotionManager.HandleMovement();
            characterAnimationManager.HandleMovement();
        }

        internal virtual void HandleJump()
        {
            characterLocomotionManager.HandleJump();
            characterAnimationManager.HandleJump();
        }

        internal virtual void HandleCloseAttack()
        {
            if (isAttacking) { return; }
            isAttacking = true;
            characterCombatManager.CloseAttack();
            characterAnimationManager.HandleCloseAttack();
        }

    }
}