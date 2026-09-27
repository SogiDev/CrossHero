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
        [Range(1, 3)]
        public float timer;
        private bool isRoundActive = false;
        public bool IsRoundActive => isRoundActive;
        public bool autoStart = false;

        [Header("Spawned Entities")]
        [SerializeField] private bool canSpawn = true;
        internal int entityCount = 10;
        [SerializeField] private GameObject[] spawnedEntities;
        public int WaveCount { get; private set; } = 0;
        public float WaveScale { get; private set; } = 1;

        [SerializeField] private Vector2 healthClamp = Vector2.one, energyClamp = Vector2.one, speedClamp = Vector2.one, damageClamp = Vector2.one;

        [Header("SpaceShips")]
        [SerializeField] private GameObject spaceShipPrefab;
        [SerializeField] private Sprite[] spaceShips;
        [SerializeField] private Sprite[] projectiles;

        private void Start()
        {
            WorldManager.Instance.spawner = this;
            autoStart = PlayerPrefs.GetInt("Auto Start", 0) == 0 ? false : true;
            WaveScale = PlayerPrefs.GetFloat("Wave Scale", 1);
        }

        public void FixedUpdate()
        {
            // Order Matters Here !!! 
            if (canSpawn && isRoundActive) { StartCoroutine(SpawnEntity()); }

            UpdateEntityList();

            if (autoStart && !isRoundActive)
            {
                isRoundActive = true;
                StartCoroutine(StartRound());
            }


            if (spawnedEntities.Length <= 0 && entityCount <= 0)
            {
                isRoundActive = false;
            }
        }

        private readonly WaitForSeconds roundTimer = new(1);
        public IEnumerator StartRound()        
        {
            entityCount = Random.Range(5, 10) * WaveCount;
            isRoundActive = true;
            StartCoroutine(SpawnEntity());
            yield return roundTimer;
            WaveCount++;
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
                var hp = Random.Range(healthClamp.x, healthClamp.y);
                var eng = Random.Range(energyClamp.x, energyClamp.y);
                var sp = Random.Range(speedClamp.x, speedClamp.y);
                var dmg = Random.Range(damageClamp.x, damageClamp.y);
                int score = Mathf.RoundToInt(hp + sp + dmg + eng);

                character.SetStat(
                    hp * (WaveCount * WaveScale),
                    eng * (WaveCount * WaveScale),
                    sp,
                    dmg * (WaveCount * WaveScale),
                    score
                );
            }

            if (entity.TryGetComponent<CharacterCombatManager>(out var characterCombatManager))
            {
                characterCombatManager.projectileSprite = projectiles[Random.Range(0, projectiles.Length)];
                characterCombatManager.attackType = RandomType();
            }

            yield return new WaitForSeconds(timer);
            timer = Random.Range(1, 3);
            canSpawn = true;
        }

        private AttackType RandomType()
        {
            int rng = Random.Range(0, 3);
            switch (rng)
            {
                case (1):
                    return AttackType.CLOSE;
                case (2):
                    return AttackType.PROJECTILE;
                case (3):
                    return AttackType.LASER;
                default:
                    return AttackType.SUPPORT;
            }
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