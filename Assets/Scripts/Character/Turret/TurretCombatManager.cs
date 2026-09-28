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
            if (target != null) { AttackTarget(target); }
        }
        public void SetTurret(TurretData turretData)
        {
            data = turretData;
            score = Mathf.RoundToInt(turretData.cost * 0.05f);

            baseDamage = turretData.baseDamage;
            projectileSpeed = data.baseProjectileSpeed;
            attackType = data.attackType;
            projectileSprite = data.projectileImage;

            switch (data.attackType)
            {
                case AttackType.CLOSE:
                    closeAttackRange = data.searchRadius;;
                    break;
                case AttackType.LASER:
                    projectileAttackRange = data.searchRadius;
                    break;
                case AttackType.PROJECTILE:
                    projectileAttackRange = data.searchRadius;
                    break;
                case AttackType.SUPPORT:
                    supportRange = data.searchRadius;
                    break;
            }
        }

        private void AttackTarget(GameObject target = null)
        {
            switch (data.attackType)
            {
                case AttackType.CLOSE:
                    closeAttackTimer = data.baseTimer;
                    StartCoroutine(CloseAttack());
                    break;
                case AttackType.LASER:
                    laserAttackTimer = data.baseTimer;
                    StartCoroutine(LaserAttack(target));
                    break;
                case AttackType.PROJECTILE: 
                    projectileAttackTimer = data.baseTimer;
                    StartCoroutine(ProjectileAttack(target.transform.position));
                    break;
                case AttackType.SUPPORT: 
                    supportTimer = data.baseTimer;
                    StartCoroutine(Support());
                    break;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            WorldManager.Instance.playerData.AddScore(score / -2);
        }
    }
}