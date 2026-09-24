using UnityEngine;

namespace DS
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(TurretCombatManager))]
    public class TurretManager : CharacterManager
    {
        [SerializeField] private TurretData data;
        private TurretCombatManager turretCombatManager;

        protected override void Awake()
        {
            turretCombatManager = GetComponent<TurretCombatManager>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = true;
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            
        }


        public void SetTurret(TurretData turretData) 
        { 
            if (data != null) { return; }

            data = turretData;

            name = turretData.turretName;
            spriteRenderer.sprite = turretData.turretImage;
            data = turretData;
            maxHealth = data.maxHealth;
            health = data.maxHealth;
            maxEnergy = data.maxEnergy;
            energy = data.maxEnergy;
            baseSpeed = data.baseProjectileSpeed;
            score = Mathf.RoundToInt(data.cost * 0.1f);
        }
        public TurretData GetTurret() { return data; }

    }
}