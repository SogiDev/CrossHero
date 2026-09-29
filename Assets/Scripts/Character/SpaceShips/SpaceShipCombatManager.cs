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

            if (crystal != null && IsTargetInRange(crystal.gameObject))
            {
                target = crystal.gameObject;
            }
            else
            {
                FindTargetInRange("Turret");
            }

            if (target != null)
            {
                targetPosition = target.transform.position;
                AttackTarget();
            }
        }
    }
}