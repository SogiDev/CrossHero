using TMPro;
using UnityEngine;

namespace DS
{
    public class TurretSurface : MonoBehaviour
    {
        [Header("Cost")]
        private bool isPurchased = false;
        [SerializeField] private int cost = 100;
        public float Cost => cost;

        [Header("Turret")]
        [SerializeField] private GameObject turretPrefab;
        [SerializeField] private TurretData turretData;
        [SerializeField] private TMP_Text costText;


        private void Awake()
        {
            if (turretData == null) { Debug.LogError("No Turret Data", gameObject); }
        }
        private void FixedUpdate()
        {
            costText.text = "$" + cost;
        }

        public bool BuyTurret()
        {
            if (isPurchased) return false;
            

            var playerData = WorldManager.Instance.playerData;
            if (playerData.currentScore >= cost)
            {
                playerData.currentScore -= cost;

                var offsetPosition = transform.position + new Vector3(0, 1.25f + (turretPrefab.transform.localScale.y / 4));
                var clone = Instantiate(turretPrefab, offsetPosition, Quaternion.identity, transform);
                playerData.placedTurrets++;
                clone.GetComponent<TurretManager>().SetTurret(turretData);
                clone.GetComponent<TurretCombatManager>().SetTurret(turretData);
                isPurchased = true;
                return true;
            }

            return false;
        }

    }
}