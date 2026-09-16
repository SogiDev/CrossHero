using UnityEngine;

namespace DS
{

    enum TurretAttackType
    {
        TURRET_NONE,
        TURRET_CLOSE,
        TURRET_PROJECTILE,
        TURRET_LASER,
        TURRET_SUPPORT
    }

    [RequireComponent(typeof(Rigidbody2D))]
    public class TurretCombatManager : CharacterCombatManager
    {

        [Header("Turret")]
        [SerializeField] private float searchRadius = 20;
        [SerializeField] private TurretAttackType attackType;

        

        protected override void Update()
        {
            
            if (SearchInRange() is var target && target != null)
            {
                targetPosition = target.transform.position;
                AttackTarget(target);
            }

        }

        private void AttackTarget(GameObject target)
        {
            switch (attackType)
            {
                case (TurretAttackType.TURRET_CLOSE): 
                    StartCoroutine(CloseAttack());
                    break;
                case (TurretAttackType.TURRET_LASER): 
                    StartCoroutine(LaserAttack());
                    break;
                case (TurretAttackType.TURRET_PROJECTILE): 
                    StartCoroutine(ProjectileAttack());
                    break;
                case (TurretAttackType.TURRET_SUPPORT): 
                    StartCoroutine(Support());
                    break;

            }
        }

        private GameObject SearchInRange()
        {

            if (Physics2D.CircleCast(transform.position, searchRadius, forwardPosition) is var hit && hit.collider != null)
            {
                if (hit.distance <= searchRadius)
                {
                    return hit.collider.gameObject;
                }
            }
            return null;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, searchRadius);
        }

    }
}