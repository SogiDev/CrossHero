using UnityEditor;
using UnityEngine;

namespace DS
{

    [RequireComponent(typeof(Rigidbody2D))]
    public class TurretCombatManager : CharacterCombatManager
    {

        [Header("Turret")]

        [SerializeField] private TurretData data;

        public float readSearch = 0;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Update()
        {
            base.Update();
            
            if (data == null) return;

            readSearch = data.searchRadius;
            SearchInRange();

            if (target)
            {
                targetPosition = target.transform.position;
                AttackTarget(target);
            }

        }
        public void SetTurret(TurretData turretData)
        {
            data = turretData;
            score = turretData.score;
            projectileSpeed = data.baseProjectileSpeed;
            attackType = data.attackType;
        }

        private void AttackTarget(GameObject target)
        {
            switch (data.attackType)
            {
                case AttackType.CLOSE:
                    closeAttackRange = data.searchRadius;
                    closeAttackTimer = data.baseTimer;
                    StartCoroutine(CloseAttack());
                    break;
                case AttackType.LASER:
                    projectileAttackRange = data.searchRadius;
                    laserAttackTimer = data.baseTimer;
                    StartCoroutine(LaserAttack(target));
                    break;
                case AttackType.PROJECTILE: 
                    projectileAttackRange = data.searchRadius;
                    projectileAttackTimer = data.baseTimer;
                    StartCoroutine(ProjectileAttack(target.transform.position));
                    break;
                case AttackType.SUPPORT: 
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
            if (Physics2D.CircleCast(transform.position, data.searchRadius, forwardPosition) is var hit)
            {
                if (!hit.collider.gameObject.CompareTag("Entity")) { target = null; return false;}
                
                target = IsTargetInRange(hit.collider.gameObject) ? hit.collider.gameObject : null;
                return true;
            }
            return false;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            WorldManager.Instance.playerData.currentRound.score -= score / 2;
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