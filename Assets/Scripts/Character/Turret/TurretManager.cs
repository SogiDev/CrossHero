using UnityEngine;

namespace DS
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(TurretCombatManager))]
    public class TurretManager : CharacterManager
    {

        private TurretCombatManager turretCombatManager;

        protected override void Awake()
        {
            turretCombatManager = GetComponent<TurretCombatManager>();
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            
        }
    }
}