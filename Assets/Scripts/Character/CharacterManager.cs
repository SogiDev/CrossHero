using System.Runtime.CompilerServices;
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
        [SerializeField] private bool canSetStat  = false;
        public int maxHealth { get; private set;  } = 10;
        public float health = 10;

        public int maxEnergy { get; private set; } = 10;
        public float energy = 10;
        public int baseSpeed { get; private set; } = 1;
        public float currentSpeed = 1;

        public int baseDamage { get; private set; } = 1;
        public float currentDamage = 1;


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

        public void SetStat(float hp, float eng, float sp, float dmg)
        {
            if (canSetStat)
            {
                health = maxHealth = Mathf.RoundToInt(hp);
                energy = maxEnergy = Mathf.RoundToInt(eng);
                currentSpeed = baseSpeed = Mathf.RoundToInt(sp);
                currentDamage = baseDamage = Mathf.RoundToInt(dmg);

                canSetStat = false;
            }
            else
            {
                Debug.LogWarning("Can't Set Stat", gameObject);
                return;
            }

        }

        protected virtual void HandleGrounded()
        {
            characterLocomotionManager.HandleGrounded();
            characterAnimationManager.HandleGrounded(isGrounded);
        }

        protected virtual void HandleMovement()
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