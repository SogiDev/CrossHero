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
        protected SpriteRenderer spriteRenderer;

        [Header("States")]
        internal bool isSprinting;
        internal bool isWalking, isCrouching, isJumping, isGrounded, isAttacking;

        [Header("Status")]
        [SerializeField] protected bool canSetStat  = false;
        [SerializeField] protected int maxHealth = 10;
        public float MaxHealth => maxHealth;
        public float health = 10;

        [SerializeField] protected int maxEnergy = 10;
        public float MaxEnergy => maxEnergy;
        public float energy = 10;
        [SerializeField] protected int baseSpeed = 1;
        public float BaseSpeed => baseSpeed;
        public float currentSpeed = 1;

        [SerializeField] protected int baseDamage = 1;
        public float BaseDamage => baseDamage;
        public float currentDamage = 1;


        protected virtual void Awake()
        {
            characterLocomotionManager = GetComponent<CharacterLocomotionManager>();
            characterAnimationManager = GetComponent<CharacterAnimationManager>();
            characterCombatManager = GetComponent<CharacterCombatManager>();
            spriteRenderer = GetComponent<SpriteRenderer>();
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
            if (characterAnimationManager != null && characterAnimationManager.enabled)
            {
                characterAnimationManager.animator.SetTrigger(Animator.StringToHash("OnHit"));
            }
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