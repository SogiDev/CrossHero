using UnityEngine;

namespace DS
{
    public class PlayerUI : MonoBehaviour
    {

        public static PlayerUI Instance;

        [SerializeField] internal RoundUI roundUI;
        [SerializeField] internal StoreUI storeUI;
        [SerializeField] internal TurretUI turretUI;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ShowStore(TurretSurface turretSurface)
        {
            storeUI.gameObject.SetActive(true);
            storeUI.SetPurchaseButton(turretSurface);
        }
    }
}