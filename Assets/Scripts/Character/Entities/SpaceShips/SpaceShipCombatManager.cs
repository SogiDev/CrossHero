using UnityEngine;

namespace DS
{
    public class SpaceShipCombatManager : CharacterCombatManager
    {
        protected override void Awake()
        {
            character = GetComponent<SpaceShipManager>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected override void Start()
        {
            base.Start();
        }
        protected override void Update()
        {
            base.Update();
            targetPosition = (transform.right * projectileAttackRange) + transform.position;

            if (!character.canAttack) { return; }
            StartCoroutine(ProjectileAttack(targetPosition));
        }
    }
}