using UnityEngine;

namespace DS
{

    [RequireComponent(typeof(Rigidbody2D))]
    public class TurretCombatManager : CharacterCombatManager
    {

        [Header("Turret")]
        [SerializeField] private TurretData data;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Update()
        {
            base.Update();
            
            if (data == null) return;

            if (FindTargetInRange() is var hit && hit != null)
            {
                target = hit;
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