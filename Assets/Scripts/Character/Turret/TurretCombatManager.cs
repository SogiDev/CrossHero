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
        [SerializeField] private GameObject target = null;


        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Update()
        {
            base.Update();

            if (target != null && !IsTargetInRange(target)) { target = null;}

            if (target == null) { SearchInRange(); }

            if (target)
            {
                targetPosition = target.transform.position;
                AttackTarget(target);
            }

            // Attack Radius must always be equal to search radius
            closeAttackRange = searchRadius;
            projectileAttackRange = searchRadius;
            supportRange = searchRadius;

        }

        private void AttackTarget(GameObject target)
        {
            switch (attackType)
            {
                case TurretAttackType.TURRET_CLOSE: 
                    StartCoroutine(CloseAttack());
                    break;
                case TurretAttackType.TURRET_LASER: 
                    StartCoroutine(LaserAttack());
                    break;
                case TurretAttackType.TURRET_PROJECTILE: 
                    StartCoroutine(ProjectileAttack());
                    break;
                case TurretAttackType.TURRET_SUPPORT: 
                    StartCoroutine(Support());
                    break;
            }
        }

        private bool IsTargetInRange(GameObject obj)
        {
            var distance = obj.transform.position - transform.position;
            return distance.magnitude <= searchRadius;
        }

        private bool SearchInRange()
        {
            if (Physics2D.CircleCast(transform.position, searchRadius, forwardPosition) is var hit && hit.collider != null)
            {
                if (!hit.collider.gameObject.CompareTag("Entity")) { return false;}

                if (IsTargetInRange(hit.collider.gameObject))
                {
                    target = hit.collider.gameObject;
                    return true;
                }
                else
                {
                    target = null;
                }
            }
            return false;
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, searchRadius);
        }

    }
}