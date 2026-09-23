using UnityEngine;
using UnityEngine.UI;

namespace DS
{
    public class StoreUI : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TurretDetailsUI turretDetailsUI;
        [SerializeField] private TurretUI turretUI;
        [SerializeField] private Button turretPurchaseButton;
        [SerializeField] internal GameObject turretStore, crystalStore;

        [Header("Turret Data")]
        private TurretSurface turretPlacement;
        private TurretData currentData;

        [Header("Crystal Data")]
        private Crystal crystalData;
        [SerializeField] private TurretDetailsUI crystalDetailsUI;
        [SerializeField] private Button zonePurchaseButton;
        [SerializeField] private int zoneCost = 10000;


        private void Start()
        {
            if (turretDetailsUI == null) { turretDetailsUI = GetComponentInChildren<TurretDetailsUI>(); }
            if (crystalDetailsUI == null) { crystalDetailsUI = GetComponentInChildren<TurretDetailsUI>(); }
            if (turretUI == null) { turretUI = GetComponentInChildren<TurretUI>(); }
        }

        private void OnEnable()
        {
            turretPurchaseButton.onClick.AddListener(() => BuyTurret());
            
            var allTurrets = WorldManager.Instance.GetTurretData();
            turretUI.availableTurrets = allTurrets;
            turretDetailsUI.UpdateDetails(allTurrets[0]);

            zonePurchaseButton.onClick.AddListener(() => BuyZone());
        }

        private void OnDisable()
        {
            turretPurchaseButton.onClick.RemoveListener(() => BuyTurret());
            zonePurchaseButton.onClick.RemoveListener(() => BuyZone());
        }


        #region Turrets Store
        internal void SetPurchaseButton(TurretSurface turretSurface)
        {
            turretPlacement = turretSurface;
        }

        internal void SetData(TurretData data)
        {
            currentData = data;
            turretDetailsUI.UpdateDetails(data);
        }

        private bool BuyTurret()
        {
            PlayerUI.Instance.storeUI.gameObject.SetActive(false);
            if (turretPlacement.isPurchased) { return false; }
            var playerData = WorldManager.Instance.playerData;
            
            if (playerData.currentRound.score >= currentData.cost)
            {
                playerData.currentRound.AddScore(-currentData.cost);
                turretPlacement.BuildTurret(currentData);
                return true;
            }
            return false;
        }

        #endregion

        #region Crystal Store

        internal void SetData(Crystal crystal)
        {
            crystalData = crystal;
            crystalDetailsUI.UpdateDetails(crystal, zoneCost);
        }

        private void BuyZone()
        {
            var zones = FindObjectsByType<ZoneCreator>();
            var cost = zones.Length * zoneCost;

            PlayerUI.Instance.storeUI.gameObject.SetActive(false);

            var playerData = WorldManager.Instance.playerData;

            if (playerData.currentRound.score >= cost)
            {
                playerData.currentRound.AddScore(-cost);
                crystalData.CreateZone();
                return;
            }

            Debug.Log("Not Enough To Buy Zone", gameObject);
            return;
        }

        #endregion

    }
}