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
        [SerializeField] private float attackOffset = 1.5f;


        [Header("Close Combat Info")]
        [SerializeField] protected int closeCombatRange = 3;
        [SerializeField] protected int closeAttackRange = 2;

        [Header("Ranged Combat Info")]
        [SerializeField] protected GameObject projectile;
        [SerializeField] protected int projectileCombatRange = 8;
        [SerializeField] protected int projectileAttackRange = 8;

        [Header("Support Info")]
        [SerializeField] protected int supportRange = 5;

        [Header("Attack Info")]
        [SerializeField] protected int baseDamage = 1;
         public float currentDamage = 2;
        [SerializeField] protected float attackTimer = 3;

        
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
            forwardPosition = spriteRenderer.flipX ? transform.position - transform.right * attackOffset : transform.position + transform.right * attackOffset;
        }
        
        // Search For Target In Range
        // Assign Target To Entity

        // Close Combat Attack
        internal virtual void CloseAttack()
        {
            var hit = Physics2D.CircleCast(forwardPosition, closeAttackRange, transform.right);
            Debug.DrawRay(forwardPosition, transform.right, Color.blue);

            if (hit && hit.collider.TryGetComponent<CharacterManager>(out CharacterManager entity))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    character.isAttacking = false;
                    return;
                }
                entity.TakeDamage(currentDamage);
            }
            character.isAttacking = false;
        }

        // Projectile Attack
        internal virtual void ProjectileAttack()
        {
            var direction = targetPosition - new Vector2(transform.position.x, transform.position.y);

            StartCoroutine(ShootProjectile(forwardPosition, direction, 100));
            
            // Laser Like Raycast
            //var length = Mathf.Sqrt( Mathf.Sqrt(direction.x) + Mathf.Sqrt(direction.y) );
            //var hit = Physics2D.Raycast(forwardPosition, direction, length);
            character.isAttacking = false;
            
        }

        private IEnumerator ShootProjectile(Vector2 position, Vector2 direction, float force)
        {
            var clone = Instantiate(projectile, position, Quaternion.identity, null);
            if (clone.TryGetComponent<Rigidbody2D>(out var rigidbody))
            {
                rigidbody.AddForce(direction * force, ForceMode2D.Impulse);
            }
            else
            {
                var rigidbody2D = clone.AddComponent<Rigidbody2D>();
                rigidbody2D.AddForce(direction * force, ForceMode2D.Impulse);
            }

            Destroy(clone, 10.0f);

            character.isAttacking = true;

            yield return new WaitForSeconds(attackTimer);

            character.isAttacking = false;
        }

        // Support
        internal virtual void Support()
        {

        }

        private void OnDrawGizmosSelected()
        {
            // Draw Close Attack
            Gizmos.color = Color.coral;
            Gizmos.DrawWireSphere(forwardPosition, closeAttackRange);

            // Draw Ranged Attack
            Gizmos.color = Color.darkMagenta;
            Gizmos.DrawRay(forwardPosition, targetPosition);

            // Draw Support Attack
            Gizmos.color = Color.floralWhite;
            Gizmos.DrawWireSphere(forwardPosition, supportRange);

        }
    }
}