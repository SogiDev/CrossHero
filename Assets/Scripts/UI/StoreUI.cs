using UnityEngine;
using UnityEngine.UI;

namespace DS
{
    public class StoreUI : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TurretDetailsUI detailsUI;
        [SerializeField] private TurretUI turretUI;
        [SerializeField] private Button purchaseButton;

        [Header("Turret Data")]
        private TurretSurface turretPlacement;
        private TurretData currentData;

        private void Start()
        {
            if (detailsUI == null) { detailsUI = GetComponentInChildren<TurretDetailsUI>(); }
            if (turretUI == null) { turretUI = GetComponentInChildren<TurretUI>(); }
        }

        private void OnEnable()
        {
            purchaseButton.onClick.AddListener(() => BuyTurret());
            var allTurrets = WorldManager.Instance.GetTurretData();
            turretUI.availableTurrets = allTurrets;
            detailsUI.UpdateDetails(allTurrets[0]);
        }

        private void OnDisable()
        {
            purchaseButton.onClick.RemoveListener(() => BuyTurret());
        }

        internal void SetPurchaseButton(TurretSurface turretSurface)
        {
            turretPlacement = turretSurface;
        }

        internal void SetData(TurretData data)
        {
            currentData = data;
            detailsUI.UpdateDetails(data);
        }

        private bool BuyTurret()
        {
            PlayerUI.Instance.storeUI.gameObject.SetActive(false);
            if (turretPlacement.isPurchased) { return false; }
            var playerData = WorldManager.Instance.playerData;
            
            if (playerData.currentRound.score >= currentData.cost)
            {
                playerData.currentRound.score -= currentData.cost;
                turretPlacement.BuildTurret(currentData);
                return true;
            }
            return false;
        }
    }
}