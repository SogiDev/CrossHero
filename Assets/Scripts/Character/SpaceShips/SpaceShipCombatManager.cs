using UnityEngine;

namespace DS
{
    public class SpaceShipCombatManager : CharacterCombatManager
    {
        private Crystal crystal;
        protected override void Awake()
        {
            character = GetComponent<SpaceShipManager>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected override void Start()
        {
            base.Start();
            crystal = FindAnyObjectByType<Crystal>();
        }
        protected override void Update()
        {
            base.Update();



            if (FindTargetInRange(crystal.gameObject) is var hit && target == null)
            {
                target = crystal.gameObject;
                character.canMove = false;
            }
            else if (FindTargetInRange() is var turret && turret != null)
            {
                character.canMove = true;
            }



            if (target != null && character.canAttack)
            {
                AttackTarget(target);
            }
        }

        private void AttackTarget(GameObject target)
        {
            switch (attackType)
            {
                case AttackType.CLOSE:
                    StartCoroutine(CloseAttack());
                    break;
                case AttackType.LASER:
                    StartCoroutine(LaserAttack(target));
                    break;
                case AttackType.PROJECTILE:
                    StartCoroutine(ProjectileAttack(target.transform.position));
                    break;
                case AttackType.SUPPORT:
                    StartCoroutine(Support());
                    break;
            }
        }
    }
}