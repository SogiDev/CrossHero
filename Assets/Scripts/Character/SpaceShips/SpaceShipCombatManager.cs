using UnityEditor;
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


            if (target == null)
            {
                if (FindTargetInRange(crystal.gameObject))
                {
                    target = crystal.gameObject;
                    character.canMove = false;
                    character.isCrouching = false;
                    character.isSprinting = false;
                }
                else if (FindTargetInRange() is var turret && turret != null)
                {
                    character.canMove = true;
                    character.isCrouching = true;
                    character.isSprinting = false;
                }
                else
                {
                    character.canMove = true;
                    character.isSprinting = true;
                    character.isCrouching = false;
                }
            }

            character.canMove = target == crystal.gameObject;

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