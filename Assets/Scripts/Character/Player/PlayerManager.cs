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
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            inputManager.HandleAllInputs();
        }
    }
}