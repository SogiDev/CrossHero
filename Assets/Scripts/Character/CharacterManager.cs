using UnityEngine;

namespace DS
{
    [RequireComponent(typeof(CharacterLocomotionManager))]
    [RequireComponent(typeof(CharacterAnimationManager))]
    [RequireComponent(typeof(CharacterSoundManager))]
    [RequireComponent(typeof(CharacterCombatManager))]
    public class CharacterManager : MonoBehaviour
    {
        [Header("Systems")]
        protected CharacterLocomotionManager characterLocomotionManager;
        protected CharacterAnimationManager characterAnimationManager;
        protected CharacterCombatManager characterCombatManager;
        protected CharacterSoundManager characterSoundManager;
        protected SpriteRenderer spriteRenderer;

        [Header("States")]
        internal bool isSprinting = false, canSprint = true;
        internal bool isWalking = false, canMove = true;
        internal bool isCrouching = false, canCrouch = false;
        internal bool isJumping = false, canJump = true;
        internal bool isAttacking = false, canAttack = true;
        internal bool isGrounded = false;

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
        protected int score = 1;
        public int Score => score;


        protected virtual void Awake()
        {
            characterLocomotionManager = GetComponent<CharacterLocomotionManager>();
            characterAnimationManager = GetComponent<CharacterAnimationManager>();
            characterCombatManager = GetComponent<CharacterCombatManager>();
            characterSoundManager = GetComponent<CharacterSoundManager>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        protected virtual void Start()
        {

        }

        protected virtual void FixedUpdate()
        {
            if (health <= 0)
            {
                Destroy(gameObject, 1f);
                canMove = false;
                canJump = false;
                canAttack = false;
            }

            HandleMovement();
            HandleGrounded();
        }
        protected virtual void OnDestroy()
        {
            canMove = false;
            canJump = false;
            canAttack = false;

            if (score >= 0)
            {
                WorldManager.Instance.playerData.AddScore(score);
            }

        }

        #region Movement
        protected virtual void HandleGrounded()
        {
            if (!canMove) { return; }
            characterLocomotionManager.HandleGrounded();
            characterAnimationManager.HandleGrounded(isGrounded);
        }

        protected virtual void HandleMovement()
        {
            if (!canMove) { return; }
            characterLocomotionManager.HandleMovement();
            characterAnimationManager.HandleMovement();
        }

        internal virtual void HandleJump()
        {
            if (!canJump) { return; }
            characterLocomotionManager.HandleJump();
            characterAnimationManager.HandleJump();
        }
#endregion

        #region Combat
        internal virtual void HandleCloseAttack()
        {
            if (!canAttack) { return; }
            if (isAttacking) { return; }
            isAttacking = true;
            StartCoroutine(characterCombatManager.CloseAttack());
            characterAnimationManager.HandleCloseAttack();
        }
        public void TakeDamage(float damage, GameObject caller = null)
        {

            if (caller != null)
            {
                Debug.Log(caller.name + " Did " + damage, caller);
            }

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

        public void SetStat(float hp, float eng, float sp, float dmg, int scr = 0)
        {
            if (canSetStat)
            {
                health = maxHealth = Mathf.RoundToInt(hp);
                energy = maxEnergy = Mathf.RoundToInt(eng);
                currentSpeed = baseSpeed = Mathf.RoundToInt(sp);
                characterCombatManager.UpdateStats(dmg);
                score = scr;
                canSetStat = false;
            }
            else
            {
                Debug.LogWarning("Can't Set Stat", gameObject);
                return;
            }

        }
        #endregion
    }
}