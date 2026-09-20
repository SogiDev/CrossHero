using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DS {

    [RequireComponent(typeof(Camera))]
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera Instance { get; private set; }

        internal PlayerManager player;
        public Camera camera { get; private set; }

        [Header("Camera Flags")]
        [SerializeField] internal bool lockX = false;
        [SerializeField] internal bool lockY = false;
        [SerializeField] internal Vector2 lockPosition = Vector2.zero;

        [Header("Mouse Info")]
        public Vector2 MouseScreenPosition { get; private set; }
        public Vector2 MouseWorldPosition { get; private set;}

        [SerializeField] private Transform cameraPivotTransform;


        [Header("Follow Settings")]
        [SerializeField] private float cameraSmoothSpeed = 8f;
        [SerializeField] private Vector3 cameraTargetOffset = new (0f, 0.0f, -1.0f);
        [SerializeField] private float cameraDistance = 5f;
        [SerializeField] private float minimumCameraDistance = 0.5f;


        private float currentCameraDistance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject.transform.root);
            }
            else
            {
                Destroy(gameObject);
            }

            transform.SetParent(null);
            camera = GetComponent<Camera>();

            currentCameraDistance = Mathf.Clamp(cameraDistance, minimumCameraDistance, cameraDistance);
            SetCameraLocalPosition(currentCameraDistance);

        }

        private void Start()
        {
            player = PlayerManager.Instance;
        }

        private void Update()
        {
            MouseScreenPosition = Mouse.current.position.ReadValue();
            MouseWorldPosition = camera.ScreenToWorldPoint(MouseScreenPosition);
        }

        private void LateUpdate()
        {
            if (player == null) return;

            HandleFollowTarget();
            if (lockX) { transform.position = new Vector3(lockPosition.x, transform.position.y, transform.position.z); };
            if (lockY) { transform.position = new Vector3(transform.position.x, lockPosition.y, transform.position.z); };
        }

        public void HandleAllCameraActions()
        {
            if (player == null || camera == null || cameraPivotTransform == null)
            {
                return;
            }

            //HandleFollowTarget();
            transform.position = player.transform.position + cameraTargetOffset;

        }
        private void HandleFollowTarget()
        {
            var targetPosition = player.transform.position + cameraTargetOffset;
            var followBlend = 1f - Mathf.Exp(-cameraSmoothSpeed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, targetPosition, followBlend);
        }

        private void SetCameraLocalPosition(float distance)
        {
            if (camera == null)
            {
                return;
            }

            camera.transform.localPosition = new Vector3(0f, 0f, -distance);
        }
    }
}