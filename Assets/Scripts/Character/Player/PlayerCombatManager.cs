using UnityEngine;
using UnityEngine.InputSystem;
namespace DS
{
    public class PlayerCombatManager : CharacterCombatManager
    {

        private PlayerManager player;
        private PlayerInputManager inputManager;
        private PlayerCamera camera;
        private GameObject empty;

        protected override void Awake()
        {
            player = GetComponent<PlayerManager>();
            character = GetComponent<PlayerManager>();
            inputManager = GetComponent<PlayerInputManager>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            base.Start();
            camera = PlayerCamera.Instance;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            targetPosition = camera.MouseWorldPosition;
        }
    }
}