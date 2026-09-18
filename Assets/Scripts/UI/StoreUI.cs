using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DS
{
    public class StoreUI : MonoBehaviour
    {

        [SerializeField] private GameObject holderPrefab;
        [SerializeField] private GameObject contentArea;

        [Header("Turret Data")]
        private TurretSurface turretPlacement;
        private TurretData currentData;
        [SerializeField] private TurretDisplay[] turretHolders = new TurretDisplay[1];

        [Serializable]
        private struct TurretDisplay
        {
            public GameObject display;
            public TurretData turret;
        }


        [Header("Details")]
        [SerializeField] private Image displayTurretImage;
        [SerializeField] private TMP_Text displayTurretName;
        [SerializeField] private TMP_Text displayTurretAttackType;
        [SerializeField] private TMP_Text displayTurretCost;
        [SerializeField] private TMP_Text displayTurretMaxHealth;
        [SerializeField] private TMP_Text displayTurretMaxEnergy;
        [SerializeField] private TMP_Text displayTurretBaseDamage;
        [SerializeField] private TMP_Text displayTurretBaseProjectileSpeed;
        [SerializeField] private Button purchaseButton;

        private void Start()
        {
            UpdateTurretList();
            ShowTurretData(turretHolders[0].turret);
        }

        private void OnEnable()
        {
            purchaseButton.onClick.AddListener(() => BuyTurret());
        }

        private void OnDisable()
        {
            purchaseButton.onClick.RemoveListener(() => BuyTurret());
            
        }

        internal void SetPurchaseButton(TurretSurface turretSurface)
        {
            turretPlacement = turretSurface;
        }

        public void UpdateTurretList()
        {
            var allTurretData = WorldManager.Instance.GetTurretData();
            allTurretData = WorldManager.Instance.GetTurretData();

            List<TurretDisplay> holderList = new List<TurretDisplay>();

            // Apply Turret Data
            for (int i = 0; i < allTurretData.Length; i++)
            {
                var currentTurret = allTurretData[i];
                TurretDisplay newDisplay;

                newDisplay.display = Instantiate(holderPrefab, contentArea.transform);
                newDisplay.display.name = "Holder for " + currentTurret.turretName;
                newDisplay.turret = currentTurret;

                if (newDisplay.display.TryGetComponent<Image>(out var image))
                {
                    image.sprite = currentTurret.turretImage;
                }
                
                if (newDisplay.display.TryGetComponent<Button>(out var button))
                {
                    button.onClick.AddListener(() => ShowTurretData(newDisplay.turret));
                }

                holderList.Add(newDisplay);
            }
            turretHolders = holderList.ToArray();

        }

        private void ShowTurretData(TurretData data)
        {
            displayTurretImage.sprite = data.turretImage;
            displayTurretName.text = data.turretName;
            displayTurretAttackType.text = data.attackType.ToString();
            displayTurretCost.text = data.cost.ToString();
            displayTurretMaxHealth.text = data.maxHealth.ToString();
            displayTurretMaxEnergy.text = data.maxEnergy.ToString();
            displayTurretBaseDamage.text = data.baseDamage.ToString();
            displayTurretBaseProjectileSpeed.text = data.baseProjectileSpeed.ToString();
            currentData = data;
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