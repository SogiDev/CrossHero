using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DS
{
    public class Crystal : MonoBehaviour
    {
        public float health = 10000;

        [Header("Zone Data")]
        [SerializeField] private float zoneOffet = -4;
        [SerializeField] private int spacing = 2;
        [SerializeField] private GameObject zonePrefab;
        private ZoneCreator[] zones;

        private void FixedUpdate()
        {
            if (health <= 0)
            {
                StartCoroutine(WorldManager.Instance.CompleteGame());
                PlayerUI.Instance.CompleteGame(false);
                PlayerUI.Instance.ShowResults();
            }
        }

        public IEnumerator Finish()
        {
            yield return null;
        }

        internal void CreateZone()
        {
            // Calculate Furthest Zone
            float distance = 0;
            List<ZoneCreator> list = new List<ZoneCreator>();
            foreach (var zone in FindObjectsByType<ZoneCreator>())
            {
                var length = (zone.transform.position - transform.position);
                distance = length.magnitude > distance ? length.magnitude : distance;
                list.Add(zone);
            }

            Vector3 position = transform.position + new Vector3((-distance) + (zoneOffet * spacing), 0);
            var newZone = Instantiate(zonePrefab, position, Quaternion.identity, null);
            newZone.name = "Zone";

            list.Add(newZone.GetComponent<ZoneCreator>());
            zones = list.ToArray();
        }

        #region Health
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<Projectile>(out var projectile))
            {
                TakeDamage(projectile.damage);
                Destroy(projectile.gameObject);
            }
            else if (other.gameObject.TryGetComponent<SpaceShipManager>(out var character))
            {
                TakeDamage(character.currentDamage);
                Destroy(character.gameObject);
            }

        }
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.gameObject.TryGetComponent<Projectile>(out var projectile))
            {
                TakeDamage(projectile.damage);
                Destroy(projectile.gameObject);
            }
            else if (other.collider.gameObject.TryGetComponent<SpaceShipManager>(out var character))
            {
                TakeDamage(character.currentDamage);
                Destroy(character.gameObject);
            }

        }

        private void TakeDamage(float damage)
        {
            var dps = Mathf.Abs(damage);
            health -= dps;
        }

        #endregion


    }
}