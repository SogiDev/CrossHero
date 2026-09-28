using System.Collections;
using UnityEditor;
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

            if  (FindTargetInRange() is var hit && hit != null)
            {
                target = hit;
                targetPosition = target.transform.position;
            }

            if (target != null) { targetPosition = target.transform.position; }
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
        protected virtual GameObject FindTargetInRange(string targetTag = null)
        {
            if (target != null)
            {
                // Check if Target is In distance
                var distance = transform.position - target.transform.position;
                var length = distance.magnitude;

                return length > GetSearchRadius() ? null : target;
            }

            if (Physics2D.OverlapCircle(transform.position, GetSearchRadius(), targetMask) is var hit)
            {
                if (hit == null) { return null; }
                if (hit.gameObject == null) { return null; }
                if (hit.gameObject == gameObject) { return null; }
                if (targetTag != null && !hit.gameObject.CompareTag(targetTag)) { return null; }
                
                target = hit.gameObject;
                return target;

            }

            return null;
        }

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

            if (FindTargetInRange(targetTag) is var hit && hit != null)
            {
                if (hit.TryGetComponent<CharacterManager>(out var entity))
                {
                    currentDamage = baseDamage;
                    entity.TakeDamage(currentDamage);
                    yield return new WaitForSeconds(closeAttackTimer);
                    character.isAttacking = false;
                    yield break;
                }
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

            if (FindTargetInRange(targetTag) == null) { yield break; }

            // Create Projectile
            character.isAttacking = true;
            var clone = Instantiate(projectile, forwardPosition, Quaternion.identity, null);

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

            if (FindTargetInRange(targetTag))
            {
                // Enermy Information
                currentDamage = baseDamage * laserDamageOffset;
                target.GetComponent<CharacterManager>().TakeDamage(currentDamage);
                WorldManager.Instance.playerData.AddScore(score * 0.1f);
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
            Gizmos.color = Color.white;
            float searchRange = Mathf.Max(closeAttackRange, projectileAttackRange, supportRange);
            Gizmos.DrawWireSphere(transform.position, searchRange);

            // Draw Ranged Attack
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, targetPosition);
            
            if (attackType == AttackType.CLOSE)
            {
                // Draw Close Attack
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, closeAttackRange);
            }

            if (attackType == AttackType.PROJECTILE){
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