using UnityEngine;
using UnityEngine.UI;

namespace DS
{
    public class MainMenu : MonoBehaviour
    {
        public static MainMenu Instance;

        private SettingsUI settingsUI;
        [Header("Buttons")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button optionsButton, exitButton;


        private void Awake()
        {
            
        }

        private void Start()
        {
            if (WorldManager.Instance == null)
            {
                Debug.LogError("No WorldManager - volume sliders disabled", gameObject);
                return;
            }

        }
        private void OnEnable()
        {
            // Load Buttons
            playButton.onClick.AddListener(LoadGamePlay);
            playButton.onClick.AddListener(() => WorldManager.Instance.StartGame());
            exitButton.onClick.AddListener(Exit);
        }

        private void OnDisable()
        {
            playButton.onClick.RemoveListener(LoadGamePlay);
            exitButton.onClick.RemoveListener(Exit);

        }

        private void LoadGamePlay()
        {
            if (WorldManager.Instance != null) { StartCoroutine(WorldManager.Instance.LoadScene(1)); }
        }

        public void Exit()
        {
            // If running in the Unity Editor
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        // If running as a built standalone application
        Application.Quit();
#endif
        }
    }
}