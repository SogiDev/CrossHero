using TMPro;
using UnityEngine;

namespace DS
{
    public class TurretSurface : MonoBehaviour
    {
        [SerializeField] private Sprite[] designOptions;

        [Header("Cost")]
        public bool isPurchased { get; private set; } = false;

        [Header("Turret")]
        [SerializeField] private GameObject turretPrefab;
        [SerializeField] private GameObject currentTurrent;
        private void Start()
        {
            if (transform.parent.TryGetComponent<SpriteRenderer>(out var renderer))
            {
                renderer.sprite = designOptions[Random.Range(0, designOptions.Length)];
            }
        }

        private void FixedUpdate()
        {
            isPurchased = currentTurrent != null;
        }

        public void BuildTurret(TurretData data)
        {
            if (isPurchased) return;

            var playerData = WorldManager.Instance.playerData;
            playerData.AddScore(data.score);

            //var offsetPosition = transform.position + new Vector3(0, 1.25f + (turretPrefab.transform.localScale.y / 4));
            var offsetPosition = transform.position;
            var clone = Instantiate(turretPrefab, offsetPosition, Quaternion.identity, gameObject.transform.parent);
            
            playerData.currentRound.turretsPlaced++;
            clone.GetComponent<TurretManager>().SetTurret(data);

            clone.GetComponent<TurretCombatManager>().SetTurret(data);
            currentTurrent = clone;
            isPurchased = true;
        }

    }
}