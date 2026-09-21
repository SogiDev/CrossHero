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
        private bool isRoundActive = false;
        public bool IsRoundActive => isRoundActive;
        public bool autoStart = false;
        public bool autoRoundActive = false;

        [Header("Spawned Entities")]
        [SerializeField] private bool canSpawn = true;
        internal int entityCount = 10;
        [SerializeField] private GameObject[] spawnedEntities;

        public int WaveCount { get; private set; } = 0;
        public float WaveScale { get; private set; } = 1;

        [Header("SpaceShips")]
        [SerializeField] private GameObject spaceShipPrefab;
        [SerializeField] private Sprite[] spaceShips;


        public void FixedUpdate()
        {
            // Order Matters Here !!! 
            if (canSpawn && isRoundActive) { StartCoroutine(SpawnEntity()); }

            UpdateEntityList();

            if (entityCount <= 0 && spawnedEntities.Length <= 0) { 
                isRoundActive = false; 
                PlayerUI.Instance.ShowRound();
                autoRoundActive = false;
            }

            if (!isRoundActive && autoStart && !autoRoundActive) { StartCoroutine(StartRound()); }
        }

        private readonly WaitForSeconds roundTimer = new(1);
        public IEnumerator AutoStartRound()        
        {
            autoRoundActive = true;
            entityCount = Random.Range(1, 10) * WaveCount;
            isRoundActive = true;
            yield return roundTimer;
            WaveCount++;
            StartCoroutine(SpawnEntity());
            WorldManager.Instance.playerData.currentRound.wave = WaveCount;
        }
        public IEnumerator StartRound()        
        {
            entityCount = Random.Range(1, 10) * WaveCount;
            isRoundActive = true;
            yield return roundTimer;
            WaveCount++;
            StartCoroutine(SpawnEntity());
            WorldManager.Instance.playerData.currentRound.wave = WaveCount;
        }


        private void UpdateEntityList()
        {

            List<GameObject> list = new List<GameObject>();

            for (int i = 0; i < spawnedEntities.Length; i++)
            {
                if (spawnedEntities[i] != null)
                {
                    list.Add(spawnedEntities[i]);
                }
            }
            spawnedEntities = list.ToArray();

            if (spawnedEntities.Length <= 0 && entityCount <= 0)
            {
                isRoundActive = false;
                autoRoundActive = false;
                if (!autoStart) { PlayerUI.Instance.ShowRound(); }
            }
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
            List<GameObject> list =  new (spawnedEntities);
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
                var hp = Random.Range(1, 10);
                var sp = (10 - hp) * 3;
                var dmg = Random.Range(1, 10);
                var eng = 10 - dmg;

                character.SetStat(
                    hp * (WaveCount * WaveScale) * 10,
                    eng * (WaveCount * WaveScale) * 100,
                    sp * (WaveCount * WaveScale),
                    dmg * (WaveCount * WaveScale));
            }

            yield return new WaitForSeconds(timer);
            timer = Random.Range(0.1f, 2);
            canSpawn = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.gray3;
            Gizmos.DrawCube(transform.position, new Vector3((spawnMax.x - spawnMin.x), (spawnMax.y - spawnMin.y)));
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