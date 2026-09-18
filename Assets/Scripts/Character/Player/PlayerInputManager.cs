using UnityEngine;
using UnityEngine.SceneManagement;

namespace DS
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager Instance { get; private set; }

        public PlayerManager player;
        public PlayerUI playerUI;
        private PlayerControls playerControls;

        [Header("Camera Movement Input")]
        [SerializeField] private Vector2 cameraInput;
        public float cameraVerticalInput, cameraHorizontalInput;

        [Header("Player Movement Values")]
        public Vector2 movementInput { get; private set; }
        public float moveAmount;

        [Header("Player Action Input")]
        public bool sprintInput;
        public bool jumpInput, attackInput, crouchInput, interactInput, reloadInput;

        [Header("Player UI Input")]
        public bool pauseInput;
        public bool notepadInput;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            SceneManager.activeSceneChanged += OnSceneChanged;

            if (player == null)
            {
                player = FindAnyObjectByType<PlayerManager>();
            }
            if (playerUI == null)
            {
                playerUI = FindAnyObjectByType<PlayerUI>();
            }

            SetInputEnabled(player != null);
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                SceneManager.activeSceneChanged -= OnSceneChanged;
                Instance = null;
            }
        }

        private void OnSceneChanged(Scene oldScene, Scene newScene)
        {
            player = FindAnyObjectByType<PlayerManager>();
            if (player != null)
            {
                //player.inputManager = this;
                //player.GetComponent<PlayerLocomotionManager>().inputManager = this;
            }

            SetInputEnabled(player != null);
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                // Default
                playerControls.Player.Move.performed += context => movementInput = context.ReadValue<Vector2>();
                playerControls.Player.Move.canceled += context => movementInput = Vector2.zero;
                playerControls.Player.Look.performed += context => cameraInput = context.ReadValue<Vector2>();
                playerControls.Player.Look.canceled += context => cameraInput = Vector2.zero;

                // Movement
                playerControls.Player.Jump.performed += context => jumpInput = true;
                playerControls.Player.Sprint.performed += context => sprintInput = true;
                playerControls.Player.Sprint.canceled += context => sprintInput = false;
                playerControls.Player.Crouch.performed += context => crouchInput = true;
                playerControls.Player.Crouch.canceled += context => crouchInput = false;
                
                // Combat
                playerControls.Player.Attack.performed += context => attackInput = true;
                //playerControls.Player.Attack.canceled += context => attackInput = false;
                playerControls.Player.Interact.performed += context => interactInput = true;
                playerControls.Player.Interact.canceled += context => interactInput = false;

                // UI Inputs
                //playerControls.Player.Pause.performed += context => pauseInput = true;

            }

            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls?.Disable();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!enabled || playerControls == null)
            {
                return;
            }

            if (hasFocus)
            {
                playerControls.Enable();
            }
            else
            {
                playerControls.Disable();
            }
        }

        public void HandleAllInputs()
        {

            // Player Controller
            HandleMovementInput();
            HandleCameraInput();
            HandleJumpInput();
            HandleInteractInput();

            HandleAttack();
            
            if (player == null) { return; }
        }

        private void SetInputEnabled(bool shouldEnable)
        {
            if (shouldEnable)
            {
                enabled = true;
                playerControls?.Enable();
            }
            else
            {
                playerControls?.Disable();
            }
        }

        private void HandleMovementInput()
        {
            moveAmount = Mathf.Clamp01(Mathf.Abs(movementInput.x) + Mathf.Abs(movementInput.y));

            if (player != null)
            {
                player.isSprinting = sprintInput && moveAmount > 0.5f;
            }
        }

        private void HandleCameraInput()
        {
            cameraVerticalInput = cameraInput.y;
            cameraHorizontalInput = cameraInput.x;
        }

        private void HandleJumpInput()
        {
            if (jumpInput)
            {
                jumpInput = false;
                player.HandleJump();
            }
        }

        private void HandleAttack()
        {
            if (attackInput)
            {
                attackInput = false;
                player.HandleCloseAttack();
            }
        }

        private void HandlePauseInput()
        {
            if (pauseInput)
            {
                //PlayerUI.Instance.EnableSettingsUI();
                pauseInput = false;
            }
        }

        private void HandleInteractInput()
        {
            if (interactInput)
            {
                player.Interact();
            }
        }

    }
}