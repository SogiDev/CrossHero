using UnityEngine;

namespace DS
{
    [RequireComponent(typeof(PlayerAnimationManager))]
    [RequireComponent(typeof(PlayerLocomotionManager))]
    [RequireComponent(typeof(PlayerCombatManager))]
    public class PlayerManager : CharacterManager
    {
        public static PlayerManager Instance;

        [Header("Systems")]
        [SerializeField] private PlayerInputManager inputManager;
        internal PlayerLocomotionManager playerLocomotionManager;
        internal PlayerAnimationManager playerAnimationManager;
        internal PlayerCombatManager playerCombatManager;
        private bool isInteracting = false;
        private bool infiniteHealth = true;

        [SerializeField] private float interactRange = 0.5f;

        protected override void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            base.Awake();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimationManager = GetComponent<PlayerAnimationManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            base.Awake();
            inputManager = PlayerInputManager.Instance;
            PlayerCamera.Instance.player = this;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            inputManager.HandleAllInputs();
            if (infiniteHealth) { health = maxHealth;}
        }

        internal void Interact()
        {
            if (isInteracting) return;

            // Check For Objects In Area

            if (Physics2D.OverlapCircle(transform.position, interactRange, LayerMask.GetMask("Purchasable")) is var collider && collider != null)
            {
                if (collider.TryGetComponent<TurretSurface>(out var turretSurface))
                {
                    PlayerUI.Instance.ShowStore(turretSurface);
                    return;
                }

                if (collider.TryGetComponent<Crystal>(out var crystal))
                {
                    PlayerUI.Instance.ShowStore(crystal);
                    return;
                }
            }
        }

        protected void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.greenYellow;
            Gizmos.DrawWireSphere(transform.position, interactRange);
        }
    }
}