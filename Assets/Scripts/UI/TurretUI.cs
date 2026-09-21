using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace DS
{
    public class TurretUI : MonoBehaviour
    {
        [SerializeField] private StoreUI storeUI;
        [SerializeField] private TurretDetailsUI detailsUI;
        [SerializeField] private GameObject holderPrefab;
        [SerializeField] private GameObject contentArea;
        [SerializeField] private Image display;
        [SerializeField] internal TurretData[] availableTurrets;


        [Serializable]
        private struct TurretDisplay
        {
            public GameObject display;
            public TurretData turret;
        }

        private TurretDisplay[] turretHolders;
        private GameData playerData;

        private void Start()
        {
            if (storeUI == null) { storeUI = GetComponentInParent<StoreUI>(); }

            playerData = WorldManager.Instance.playerData;
            availableTurrets = WorldManager.Instance.GetTurretData();
            UpdateHolders();
        }

        private void UpdateHolders()
        {
            List<TurretDisplay> holderList = new List<TurretDisplay>();

            // Apply Turret Data
            for(int i = 0; i < availableTurrets.Length; i++)
            {
                var currentTurret = availableTurrets[i];

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
                    button.onClick.AddListener( () => UpdateDisplay(newDisplay, true));
                }

                holderList.Add(newDisplay);
                UpdateDisplay(newDisplay, playerData.currentRound.score >= currentTurret.cost);
            }
            turretHolders = holderList.ToArray();
        }

        private void UpdateDisplay(TurretDisplay turretDisplay, bool canPurchase)
        {
            if (storeUI != null) { storeUI.SetData(turretDisplay.turret); }
            if (detailsUI != null) { detailsUI.UpdateDetails(turretDisplay.turret); }

            if (turretDisplay.display.TryGetComponent<Image>(out var image)) {
                image.sprite = turretDisplay.turret.turretImage;
                image.color = !canPurchase ? Color.gray2 : Color.white;
            }

            if (display != null)
            {
                display.sprite = turretDisplay.turret.turretImage;
            }


        }
    }
}