using UnityEngine;

namespace DS
{

    public enum TurretAttackType
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

        [SerializeField] private TurretData data;
        [SerializeField] private GameObject target = null;


        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Update()
        {
            base.Update();

            if (data == null) return;

            if (target != null && !IsTargetInRange(target)) { target = null;}

            if (target == null) { SearchInRange(); }

            if (target)
            {
                targetPosition = target.transform.position;
                AttackTarget(target);
            }

        }
        public void SetTurret(TurretData turretData)
        {
            if (data != null) { return; }

            data = turretData;
            projectileSpeed = data.baseProjectileSpeed;
        }

        private void AttackTarget(GameObject target)
        {
            switch (data.attackType)
            {
                case TurretAttackType.TURRET_CLOSE:
                    closeAttackRange = data.searchRadius;
                    closeAttackTimer = data.baseTimer;
                    StartCoroutine(CloseAttack());
                    break;
                case TurretAttackType.TURRET_LASER:
                    projectileAttackRange = data.searchRadius;
                    laserAttackTimer = data.baseTimer;
                    StartCoroutine(LaserAttack());
                    break;
                case TurretAttackType.TURRET_PROJECTILE: 
                    projectileAttackRange = data.searchRadius;
                    projectileAttackTimer = data.baseTimer;
                    StartCoroutine(ProjectileAttack());
                    break;
                case TurretAttackType.TURRET_SUPPORT: 
                    supportRange = data.searchRadius;
                    supportTimer = data.baseTimer;
                    StartCoroutine(Support());
                    break;
            }
        }

        private bool IsTargetInRange(GameObject obj)
        {
            var distance = obj.transform.position - transform.position;
            return distance.magnitude <= data.searchRadius;
        }

        private bool SearchInRange()
        {
            if (Physics2D.CircleCast(transform.position, data.searchRadius, forwardPosition) is var hit && hit.collider != null)
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
            if (data != null)
            {
                Gizmos.DrawWireSphere(transform.position, data.searchRadius);
            }
        }

    }
}