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
                if (FindTargetInRange())
                {
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

            if (target != null && character.canAttack)
            {
                AttackTarget("Turret");
            }
        }
    }
}