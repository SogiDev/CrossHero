using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS
{
    public class Spawner : MonoBehaviour
    {
        [Header("Area Settings")]
        [SerializeField] private Vector2 spawnMin = new(-5, -5);
        [SerializeField] private Vector2 spawnMax = new(5, 5);
        public float timer = 5;
        private bool isStartingWave = false;

        [Header("Spawned Entities")]
        [SerializeField] private bool canSpawn = true;
        internal int entityCount = 10;
        [SerializeField] private GameObject[] spawnedEntities;

        public int WaveCount { get; private set; } = 1;
        public float WaveScale { get; private set; } = 1;

        [Header("SpaceShips")]
        [SerializeField] private GameObject spaceShipPrefab;
        [SerializeField] private Sprite[] spaceShips;


        public void FixedUpdate()
        {
            if (canSpawn && !isStartingWave) { StartCoroutine(SpawnEntity()); }

            if (entityCount <= 0 && spawnedEntities.Length <= 0 && !isStartingWave) { StartCoroutine(NewWave()); }
        }

        private readonly WaitForSeconds roundTimer = new(5);
        private IEnumerator NewWave()
        {
            isStartingWave = true;
            yield return roundTimer;
            WaveCount++;
            entityCount = Random.Range(1, 10) * WaveCount;
            isStartingWave = false;
            Debug.Log("New Wave Started: " + WaveCount, gameObject);
        }

        private IEnumerator SpawnEntity()
        {
            if (entityCount <= 0) yield break;
            canSpawn = false;
            entityCount -= 1;

            Vector3 position = new Vector2(
                Random.Range(spawnMin.x, spawnMax.x),
                Random.Range(spawnMin.y, spawnMax.y)
                );
            position += transform.position;


            var entity = Instantiate(spaceShipPrefab, position, Quaternion.identity, null);
            // Add To Objects
            List<GameObject> list = new (spawnedEntities);
            list.Add(entity);
            spawnedEntities = list.ToArray();

            // Random Space Ship
            var random = Random.Range(0, spaceShips.Length);
            if (entity.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.sprite = spaceShips[random];
            }
            entity.name = "Ship " + random;

            // Random Stats Based On Wave
            if (entity.TryGetComponent<CharacterManager>(out var character))
            {
                var hp = Random.Range(0, 10);
                var sp = 10 - hp;
                var dmg = Random.Range(0, 10);
                var eng = 10 - dmg;

                character.SetStat(
                    hp * (WaveCount * WaveScale),
                    eng * (WaveCount * WaveScale),
                    sp * (WaveCount * WaveScale),
                    dmg * (WaveCount * WaveScale));
            }

            yield return new WaitForSeconds(timer);
            canSpawn = true;
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