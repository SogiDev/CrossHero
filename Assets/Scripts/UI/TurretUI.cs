using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEditor.Rendering;

namespace DS
{
    public class TurretUI : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private StoreUI storeUI;
        [SerializeField] private TurretDetailsUI detailsUI;
        [SerializeField] private GameObject holderPrefab;
        [SerializeField] private GameObject contentArea;
        [SerializeField] private Image display;
        [SerializeField] internal TurretData[] availableTurrets;

        [Header("Context Menu")]
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private bool lockContextX, lockContextY;
        [SerializeField] private Vector2 scrollSize = Vector2.one;
        [SerializeField] private float scrollScale = 0.1f;
        
        [Serializable]
        private struct TurretDisplay
        {
            public GameObject display;
            public TurretData turret;
        }

        [Header("Turrets")]
        private TurretDisplay[] turretHolders;
        private GameData playerData;

        private void Start()
        {
            scrollRect = GetComponent<ScrollRect>();
            playerData = WorldManager.Instance.playerData;
            
            availableTurrets = WorldManager.Instance.GetTurretData();
            UpdateHolders();
        }

        private void FixedUpdate()
        {
            foreach (var holder in turretHolders)
            {
                var canPurchase = playerData.currentRound.score >= holder.turret.cost;
                Color yes = Color.white;
                Color no = Color.grey;

                holder.display.GetComponent<Image>().color = canPurchase ? yes : no;
            }
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
                    button.onClick.AddListener( () => UpdateDisplay(newDisplay, playerData.currentRound.score >= currentTurret.cost));
                    
                    if (storeUI != null)
                    {
                        button.onClick.AddListener(() => UpdateStore(currentTurret));
                    }

                    if (detailsUI != null)
                    {
                        button.onClick.AddListener(() => UpdateDetails(currentTurret));
                    }

                }

                holderList.Add(newDisplay);
                UpdateDisplay(newDisplay, playerData.currentRound.score >= currentTurret.cost);
            }
            turretHolders = holderList.ToArray();

            // Rescale Context Menu
            Vector2 contextSize = new Vector2(
                scrollSize.x * (scrollScale * turretHolders.Length),
                scrollSize.y * (scrollScale * turretHolders.Length))
                + Vector2.one;
            if (lockContextX) { contextSize.x = 1; }
            if (lockContextY) { contextSize.y = 1; }

            scrollRect.content.anchorMax = contextSize;
        }

        private void UpdateDisplay(TurretDisplay turretDisplay, bool canPurchase)
        {
            if (turretDisplay.display.TryGetComponent<Image>(out var image)) {
                image.sprite = turretDisplay.turret.turretImage;
                image.color = !canPurchase ? Color.gray2 : Color.white;
            }

            if (display != null)
            {
                display.sprite = turretDisplay.turret.turretImage;
            }
        }

        private void UpdateStore(TurretData turret)
        {
            storeUI.SetData(turret);
        }

        private void UpdateDetails(TurretData turret)
        {
            
            detailsUI.UpdateDetails(turret);
        }
    }
}