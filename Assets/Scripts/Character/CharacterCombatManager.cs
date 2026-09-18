using System.Collections;
using UnityEngine;
namespace DS
{
    public class CharacterCombatManager : MonoBehaviour
    {
        protected CharacterManager character;
        protected SpriteRenderer spriteRenderer;

        [Header("Targert Info")]
        protected Vector2 targetPosition;
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
            currentDamage = baseDamage;
            forwardPosition = spriteRenderer.flipX ? transform.position - transform.right * attackOffset
             : transform.position + transform.right * attackOffset;
        }
        
        // Search For Target In Range
        // Assign Target To Entity

        // Close Combat Attack
        internal virtual IEnumerator CloseAttack()
        {
            if (character.isAttacking) yield break;
            Debug.DrawRay(forwardPosition, transform.right, Color.blue);

            if (Physics2D.CircleCast(forwardPosition, closeAttackRange, transform.right) is var hit && hit.collider != null)
            {
                if (hit.collider.gameObject == gameObject)
                {
                    Debug.Log("Hit Self", gameObject);
                    character.isAttacking = false;
                    yield break;
                }
                
                if (hit.collider.gameObject.TryGetComponent<CharacterManager>(out var entity))
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
        internal virtual IEnumerator ProjectileAttack()
        {
            if (character.isAttacking) yield break;
            var direction = targetPosition - new Vector2(transform.position.x, transform.position.y);

            // Create Projectile
            character.isAttacking = true;
            var clone = Instantiate(projectile, forwardPosition, Quaternion.identity, null);

            // RigidBody Add Force
            if (clone.TryGetComponent<Rigidbody2D>(out var rigidbody))
            {
                rigidbody.AddForce(direction * projectileSpeed, ForceMode2D.Impulse);
            }

            if (clone.TryGetComponent<Projectile>(out var bullet))
            {
                bullet.sender = gameObject;
            }

            Destroy(clone, 3.0f);
            yield return new WaitForSeconds(projectileAttackTimer);
            character.isAttacking = false;
        }

        [SerializeField] private float laserDamageOffset = 0.1f;
        internal virtual IEnumerator LaserAttack()
        {
            if (character.isAttacking) yield break;
            var direction = targetPosition - new Vector2(transform.position.x, transform.position.y);

            var length = Mathf.Sqrt( Mathf.Sqrt(direction.x) + Mathf.Sqrt(direction.y) );
            
            if (Physics2D.Raycast(forwardPosition, direction, length) is var hit && hit.collider != null)
            {
                if (hit.collider.gameObject.TryGetComponent<CharacterManager>(out var entity))
                {
                    entity.TakeDamage(baseDamage * laserDamageOffset);
                    character.isAttacking = true;
                    yield return new WaitForSeconds(laserAttackTimer * laserDamageOffset);
                    character.isAttacking = false;
                    yield break;
                }
            }

            character.isAttacking = false;
            yield break;
        }

        // Support
        internal virtual IEnumerator Support()
        {
            if (character.isAttacking) yield break;
            Debug.Log("Support Other Characters");

            yield break;
        }

        protected virtual void OnDrawGizmosSelected()
        {
            // Draw Close Attack
            Gizmos.color = Color.coral;
            Gizmos.DrawWireSphere(forwardPosition, closeAttackRange);

            // Draw Ranged Attack
            Gizmos.color = Color.darkMagenta;
            Gizmos.DrawLine(forwardPosition, targetPosition);

            Gizmos.color = Color.violetRed;
            Gizmos.DrawWireSphere(forwardPosition, projectileAttackRange);

            // Draw Support Attack
            Gizmos.color = Color.floralWhite;
            Gizmos.DrawWireSphere(forwardPosition, supportRange);

        }
    }
}