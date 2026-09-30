using UnityEngine;

namespace DS
{
    public class TurretSpawner : MonoBehaviour
    {

        [Header("Cost")]
        public bool isPurchased { get; private set; } = false;

        [Header("Turret")]
        [SerializeField] private GameObject turretPrefab;
        [SerializeField] private GameObject currentTurrent;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        private void FixedUpdate()
        {
            isPurchased = currentTurrent != null;
            spriteRenderer.enabled = !isPurchased;
        }

        public void BuildTurret(TurretData data)
        {
            if (isPurchased) return;

            var playerData = WorldManager.Instance.playerData;
            playerData.AddScore(data.score * 0.05f);

            //var offsetPosition = transform.position + new Vector3(0, 1.25f + (turretPrefab.transform.localScale.y / 4));
            var offsetPosition = transform.position;
            
            // Spawn Turret
            var clone = Instantiate(turretPrefab, offsetPosition, Quaternion.identity, null);
            clone.transform.localScale = new Vector3(2, 2, 2);

            // Set Turret Data
            clone.GetComponent<TurretManager>().SetTurret(data);
            clone.GetComponent<TurretCombatManager>().SetTurret(data);
            clone.GetComponent<SpriteRenderer>().flipX = FindAnyObjectByType<PlayerManager>().gameObject.GetComponent<SpriteRenderer>().flipX;
            currentTurrent = clone;


            // Set Global Data
            playerData.currentRound.turretsPlaced++;
            isPurchased = true;
        }

    }
}