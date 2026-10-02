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
                    closeAttackTimer = data.baseTimer;
                    break;
                case AttackType.LASER:
                    projectileAttackRange = data.searchRadius;
                    laserAttackTimer = data.baseTimer;
                    break;
                case AttackType.PROJECTILE:
                    projectileAttackRange = data.searchRadius;
                    projectileAttackTimer = data.baseTimer;
                    break;
                case AttackType.SUPPORT:
                    supportRange = data.searchRadius;
                    supportTimer = data.baseTimer;
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