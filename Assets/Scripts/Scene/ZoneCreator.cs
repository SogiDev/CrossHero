using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS
{
    public class ZoneCreator : MonoBehaviour
    {
        [Header("Area Settings")]
        [SerializeField] private Vector2 spawnMin = new(-3, -3);
        [SerializeField] private Vector2 spawnMax = new(3, 3);

        [SerializeField] private GameObject turretSurfacePrefab;
        [SerializeField] private GameObject station;
        [SerializeField] private int platformsCount = 3;

        private GameObject[] platforms ;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // Add To Surface List
            List<GameObject> list = new List<GameObject>();
            
            for (int i = 0; i < platformsCount; i++)
            {
                list.Add(SpawnPlatform());
            }

            platforms = list.ToArray();
        }

        private GameObject SpawnPlatform()
        {
            Vector3 position = new Vector2(
                Random.Range(spawnMin.x, spawnMax.x),
                Random.Range(spawnMin.y, spawnMax.y)
            );
            position += transform.position;

            var surface = Instantiate(turretSurfacePrefab, position, Quaternion.identity, transform);
            return surface;
        }

        public void SpawnStation()
        {
            station.SetActive(true);
        }

        private void OnDrawGizmosSelected()
        {
            // Vector2 Spawn Area
            Gizmos.color = Color.yellow;

            // Gizmos Spawn Area
            Gizmos.DrawLine(new Vector3(spawnMin.x, spawnMin.y) + transform.position,
                new Vector3(spawnMin.x, spawnMax.y) + transform.position);
            Gizmos.DrawLine(new Vector3(spawnMin.x, spawnMax.y) + transform.position,
                new Vector3(spawnMax.x, spawnMax.y) + transform.position);
            Gizmos.DrawLine(new Vector3(spawnMax.x, spawnMax.y) + transform.position,
                new Vector3(spawnMax.x, spawnMin.y) + transform.position);
            Gizmos.DrawLine(new Vector3(spawnMin.x, spawnMin.y) + transform.position,
                new Vector3(spawnMax.x, spawnMin.y) + transform.position);
        }

    }
}