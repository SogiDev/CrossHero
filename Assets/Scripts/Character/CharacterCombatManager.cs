using System.Collections;
using UnityEngine;
namespace DS
{
    public enum AttackType
    {
        NONE,
        CLOSE,
        PROJECTILE,
        LASER,
        SUPPORT
    }

    public class CharacterCombatManager : MonoBehaviour
    {
        protected CharacterManager character;
        protected CharacterSoundManager characterSoundManager;
        protected SpriteRenderer spriteRenderer;
        public AttackType attackType;
        protected int score = 100;

        [Header("Targert Info")]
        protected GameObject target;
        protected Vector3 targetPosition;
        [SerializeField] protected LayerMask targetMask;
        [SerializeField] protected string targetTag;
        protected Ray2D targetRay;
        protected Vector2 forwardPosition;
        [SerializeField] protected float attackOffset = 1.5f;

        [Header("Timers")]
        [SerializeField] protected float closeAttackTimer = 1;
        [SerializeField] protected float laserAttackTimer = 1;
        [SerializeField] protected float projectileAttackTimer = 1;
        [SerializeField] protected float supportTimer = 1;


        [Header("Combat Info")]
        // Range of Combat Size
        [SerializeField] protected float closeAttackRange = 2;
        [SerializeField] protected GameObject projectile;
        [SerializeField] protected LineRenderer laser;
        public Sprite projectileSprite;
        // Range of Combat Size
        [SerializeField] protected float projectileAttackRange = 8;
        [SerializeField] protected int projectileSpeed = 8;
        [SerializeField] protected float supportRange = 5;

        [Header("Attack Info")]
        [SerializeField] protected int baseDamage = 1;
         public float currentDamage = 2;

        #region Frame Data
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
            characterSoundManager = GetComponent<CharacterSoundManager>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {
            forwardPosition = spriteRenderer.flipX ? transform.position - transform.right * attackOffset
             : transform.position + transform.right * attackOffset;


            if  (target == null)
            {
                if (FindTargetInRange() is var tg)
                {
                    if (tg == null) return;
                    if (IsTargetInRange(tg))
                    {
                        target = tg;
                    }
                }
            }

            if (target != null) { 
                targetPosition = target.transform.position;
                AttackTarget();
            }
        }

        protected virtual void LateUpdate()
        {
            if (target == null)
            {
                if (laser != null)
                {
                    laser.gameObject.SetActive(false);
                }
            }
        }

        protected virtual void OnDestroy()
        {

        }
        #endregion
        internal void UpdateStats(float damage)
        {
            baseDamage = Mathf.RoundToInt(damage);
        }

        #region Detect Target

        public float GetSearchRadius()
        {
            switch (attackType)
            {
                case AttackType.CLOSE:
                    return closeAttackRange;
                case AttackType.PROJECTILE:
                    return projectileAttackRange;
                case AttackType.LASER:
                    return projectileAttackRange;
                default:
                    return supportRange;
            }

        }
        // Search For Target In Range
        protected virtual GameObject FindTargetInRange()
        {
            if (IsTargetInRange(target)) { return target; } ;

            if (Physics2D.OverlapCircle(transform.position, GetSearchRadius(), targetMask) is var hit)
            {
                // Check For Self or Null
                if (hit == null || hit.gameObject == null || hit.gameObject == gameObject) { return null; }
                
                // Check for Target Tag
                if (targetTag != null && !hit.gameObject.CompareTag(targetTag)) { return null; }
                
                target = hit.gameObject;
                return target;

            }

            target = null;
            return null;
        } 

        protected bool IsTargetInRange(GameObject currentTarget = null, float additive = 0)
        {
            if (currentTarget != null)
            {
                // Check if Target is In distance
                var distance = transform.position - currentTarget.transform.position;
                var length = distance.magnitude;

                return length <= GetSearchRadius() + additive;
            }
            else if (target != null)
            {
                // Check if Target is In distance
                var distance = transform.position - target.transform.position;
                var length = distance.magnitude;

                return length <= GetSearchRadius() + additive;
            }
            return false;

        }

        public GameObject IsTargetInRange() { return FindTargetInRange(); }

        #endregion

        #region Attacks

        protected void AttackTarget(string targetTag = null)
        {

            switch (attackType)
            {
                case AttackType.CLOSE:
                    StartCoroutine(CloseAttack(targetTag));
                    break;
                case AttackType.LASER:
                    StartCoroutine(LaserAttack(targetTag));
                    break;
                case AttackType.PROJECTILE:
                    StartCoroutine(ProjectileAttack(targetTag));
                    break;
                case AttackType.SUPPORT:
                    StartCoroutine(Support());
                    break;
            }
        }

        // Close Combat Attack
        internal virtual IEnumerator CloseAttack(string targetTag = null)
        {
            if (character.isAttacking) yield break;
            if (!character.canAttack) yield break;
            Debug.DrawRay(forwardPosition, transform.right, Color.blue);

            if (target.TryGetComponent<CharacterManager>(out var entity))
            {
                currentDamage = baseDamage;
                entity.TakeDamage(currentDamage);
                yield return new WaitForSeconds(closeAttackTimer);
                character.isAttacking = false;
                yield break;
            }


            character.isAttacking = false;
            yield break;

        }

        // Projectile Attack
        internal virtual IEnumerator ProjectileAttack(string targetTag = null)
        {
            if (character.isAttacking) yield break;
            if (!character.canAttack) yield break;
            var direction = target.transform.position - new Vector3(transform.position.x, transform.position.y);

            // Create Projectile
            character.isAttacking = true;
            var clone = Instantiate(projectile, transform.position, Quaternion.identity, null);

            if (clone.TryGetComponent<Collider2D>(out var collider))
            {
                collider.isTrigger = true;
            }

            // RigidBody Add Force
            if (clone.TryGetComponent<Rigidbody2D>(out var rigidbody))
            {
                rigidbody.AddForce((direction + transform.right) * projectileSpeed, ForceMode2D.Impulse);
            }

            if (clone.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.sprite = projectileSprite;
            }

            if (clone.TryGetComponent<Projectile>(out var bullet))
            {
                bullet.sender = gameObject;
                bullet.target = target;
                currentDamage = baseDamage;
                bullet.damage = currentDamage;
            }

            Destroy(clone, 3.0f);
            yield return new WaitForSeconds(projectileAttackTimer);
            character.isAttacking = false;
        }

        [SerializeField] private float laserDamageOffset = 0.5f;
        internal virtual IEnumerator LaserAttack(string targetTag = null)
        {
            if (character.isAttacking) yield break;
            if (!character.canAttack) yield break;
            if (laser == null)
            {
                Debug.LogError("No Laser Object Detected", gameObject);
                yield break;
            }

            if (IsTargetInRange(target))
            {
                // Enermy Information
                currentDamage = baseDamage * laserDamageOffset;
                
                if (target.TryGetComponent<CharacterManager>(out var entity))
                {
                    target.GetComponent<CharacterManager>().TakeDamage(currentDamage);
                    WorldManager.Instance.playerData.AddScore(score * 0.1f);
                }

                if (target.TryGetComponent<Crystal>(out var crystal))
                {
                    crystal.health -= currentDamage;
                    WorldManager.Instance.playerData.RemoveScore(score * 0.01f);
                }
                
                character.isAttacking = true;

                // Laser Object
                laser.gameObject.SetActive(true);
                laser.positionCount = 2;
                laser.SetPosition(0, transform.position);
                laser.SetPosition(1, target.transform.position);

                yield return new WaitForSeconds(laserAttackTimer * 0.1f);
                character.isAttacking = false;
                yield break;
            }
            
            character.isAttacking = false;
            yield break;
        }

        // Support
        internal virtual IEnumerator Support()
        {
            if (character.isAttacking) yield break;
            if (!character.canAttack) yield break;
            //Debug.Log("Support Other Characters");

            yield break;
        }
        #endregion

        protected virtual void OnDrawGizmosSelected()
        {
            // Draw Ranged Attack
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, targetPosition);
            
            if (attackType == AttackType.CLOSE)
            {
                // Draw Close Attack
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, closeAttackRange);
            }

            if (attackType == AttackType.PROJECTILE || attackType == AttackType.LASER){
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, projectileAttackRange);
            }
            
            if (attackType == AttackType.SUPPORT){

                // Draw Support Attack
                Gizmos.color = Color.purple;
                Gizmos.DrawWireSphere(transform.position, supportRange);
            }

        }
    }
}