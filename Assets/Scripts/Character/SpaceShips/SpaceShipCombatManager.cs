using UnityEngine;

namespace DS
{
    public class SpaceShipCombatManager : CharacterCombatManager
    {
        private SpaceShipManager manager;
        protected override void Awake()
        {
            manager = GetComponent<SpaceShipManager>();
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
        }
        protected override void Update()
        {
            if (target == null)
            {
                if (manager.crystalTarget != null && FindTargetInRange(manager.crystalTarget))
                {
                    target = manager.crystalTarget;
                }
                else
                {
                    FindTargetInRange();
                }
                return;
            }

            if (target != null)
            {
                targetPosition = target.transform.position;
                AttackTarget();
                return;
            }
        }
    }
}