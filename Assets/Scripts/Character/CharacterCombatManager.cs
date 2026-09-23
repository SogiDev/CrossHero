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
        protected SpriteRenderer spriteRenderer;
        protected AttackType attackType;
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
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected virtual void Start()
        {

        }

        // Update is called once per frame
        protected virtual void Update()
        {
            UpdateStats();
            forwardPosition = spriteRenderer.flipX ? transform.position - transform.right * attackOffset
             : transform.position + transform.right * attackOffset;

            if (target != null) { targetPosition = target.transform.position; }
        }

        private void UpdateStats()
        {
            baseDamage = Mathf.RoundToInt(character.BaseDamage);
            currentDamage = baseDamage;
        }

        #region Detect Target
        // Search For Target In Range
        protected GameObject FindTargetInRange()
        {
            var searchRange = attackType == AttackType.LASER || attackType == AttackType.PROJECTILE ? projectileAttackRange :
                attackType == AttackType.CLOSE ? closeAttackRange :
                supportRange;

            if (Physics2D.OverlapCircle(transform.position, searchRange, targetMask) is var hit)
            {
                if (hit == null) { return null; }
                if (hit.gameObject == null) { target = null; return null; }

                target = hit.gameObject;
                return target;

            }

            return null;
        }
        protected bool FindTargetInRange(GameObject obj)
        {
            var searchRange = attackType == AttackType.LASER || attackType == AttackType.PROJECTILE ? projectileAttackRange :
                attackType == AttackType.CLOSE ? closeAttackRange :
                supportRange;

            var distance = obj.transform.position - transform.position;
            var length = distance.magnitude;
            
            return length <= searchRange;
        }
        #endregion

        #region Attacks
        // Close Combat Attack
        internal virtual IEnumerator CloseAttack()
        {
            if (character.isAttacking) yield break;
            if (!character.canAttack) yield break;
            Debug.DrawRay(forwardPosition, transform.right, Color.blue);

            if (FindTargetInRange() is var hit && hit != null)
            {
                if (hit.TryGetComponent<CharacterManager>(out var entity))
                {
                    Debug.Log("Hit Entity: " + entity.name, gameObject);
                    entity.TakeDamage(currentDamage);
                    yield return new WaitForSeconds(closeAttackTimer);
                    character.isAttacking = false;
                    yield break;
                }
            }

            Debug.Log("No Hit", gameObject);
            character.isAttacking = false;
            yield break;

        }

        // Projectile Attack
        internal virtual IEnumerator ProjectileAttack(Vector3 position)
        {
            if (character.isAttacking) yield break;
            if (!character.canAttack) yield break;
            var direction = position - new Vector3(transform.position.x, transform.position.y);

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
                rigidbody.AddForce(direction * projectileSpeed, ForceMode2D.Impulse);
            }

            if (clone.TryGetComponent<Projectile>(out var bullet))
            {
                bullet.sender = gameObject;
                bullet.damage = currentDamage;
            }

            Destroy(clone, 3.0f);
            yield return new WaitForSeconds(projectileAttackTimer);
            character.isAttacking = false;
        }

        [SerializeField] private float laserDamageOffset = 0.5f;
        internal virtual IEnumerator LaserAttack(GameObject enemy)
        {
            if (character.isAttacking) yield break;
            if (!character.canAttack) yield break;
            target = enemy;

            if (FindTargetInRange(enemy))
            {
                enemy.GetComponent<CharacterManager>().TakeDamage(currentDamage * laserDamageOffset);
                WorldManager.Instance.playerData.AddScore(score * 0.1f, gameObject);
                character.isAttacking = true;
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
            Debug.Log("Support Other Characters");

            yield break;
        }
        #endregion

        protected virtual void OnDestroy()
        {

        }

        protected virtual void OnDrawGizmosSelected()
        {
            if (attackType == AttackType.CLOSE)
            {
                // Draw Close Attack
                Gizmos.color = Color.coral;
                Gizmos.DrawWireSphere(forwardPosition, closeAttackRange);
            }
            
            if (attackType == AttackType.LASER)
            {
                // Draw Ranged Attack
                Gizmos.color = Color.darkMagenta;
                Gizmos.DrawLine(forwardPosition, targetPosition);
            }
            
            if (attackType == AttackType.PROJECTILE){
                Gizmos.color = Color.violetRed;
                Gizmos.DrawWireSphere(forwardPosition, projectileAttackRange);
            }
            
            if (attackType == AttackType.SUPPORT){

                // Draw Support Attack
                Gizmos.color = Color.floralWhite;
                Gizmos.DrawWireSphere(forwardPosition, supportRange);
            }

        }
    }
}