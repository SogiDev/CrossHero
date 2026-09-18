using UnityEngine;

namespace DS
{
    public class SpaceShipCombatManager : CharacterCombatManager
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
        }
        protected override void Update()
        {
            base.Update();

            targetPosition = (transform.right * projectileAttackRange) + transform.position;
            StartCoroutine(ProjectileAttack(targetPosition));
        }
    }
}