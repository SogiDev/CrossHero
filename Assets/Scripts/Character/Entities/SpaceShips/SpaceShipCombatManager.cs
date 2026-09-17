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

            targetPosition = transform.right + transform.position;

            StartCoroutine(ProjectileAttack());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer != LayerMask.GetMask("Turrets")) { return; }
            Debug.Log("Destroy Turrets", gameObject);
        }

    }
}