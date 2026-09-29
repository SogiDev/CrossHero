using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS
{
    public class Spawner : MonoBehaviour
    {

        private RoundManager roundManager;

        [Header("Area Settings")]
        [SerializeField] private Vector2 spawnMin = new(-5, -5);
        [SerializeField] private Vector2 spawnMax = new(5, 5);
        [Range(1, 3)]
        public float spawnTimer;

        [Header("Spawned Entities")]
        [SerializeField] private bool canSpawn = true;
        public GameObject[] spawnedEntities { get; private set; } = new GameObject[1];

        [SerializeField] private Vector2 healthClamp = Vector2.one, energyClamp = Vector2.one, speedClamp = Vector2.one, damageClamp = Vector2.one;

        [Header("SpaceShips")]
        [SerializeField] private GameObject spaceShipPrefab;
        [SerializeField] private Sprite[] spaceShips;
        [SerializeField] private Sprite[] projectiles;
        public Vector2 moveDirection = new (1, 0);

        [Header("Round Data")]
        private float roundCount;
        private float roundScale;

        private void Start()
        {
            roundManager = RoundManager.Instance;
        }

        private void FixedUpdate()
        {
            
            if (RoundManager.Instance.isRoundActive && canSpawn) { StartCoroutine(SpawnEntity()); }
            
            UpdateEntityList();

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
            roundCount = RoundManager.Instance.roundCount;
            roundScale = RoundManager.Instance.roundScale;
            if (roundManager.entityCount <= 0) {
                yield break;
            }

            canSpawn = false;
            roundManager.entityCount -= 1;

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
            if (entity.TryGetComponent<SpaceShipManager>(out var character))
            {
                var hp = Random.Range(healthClamp.x, healthClamp.y);
                var eng = Random.Range(energyClamp.x, energyClamp.y);
                var sp = Random.Range(speedClamp.x, speedClamp.y);
                var dmg = Random.Range(damageClamp.x, damageClamp.y);
                int score = Mathf.RoundToInt(hp + sp + dmg + eng);

                character.SetStat(
                    hp * (roundCount * roundScale),
                    eng * (roundCount * roundScale),
                    sp,
                    dmg * (roundCount * roundScale),
                    score
                );

            }

            if (entity.TryGetComponent<SpaceShipCombatManager>(out var combatManager))
            {
                combatManager.projectileSprite = projectiles[Random.Range(0, projectiles.Length)];
                combatManager.attackType = RandomType();
            }

            if (entity.TryGetComponent<SpaceShipLocomotionManager>(out var locomotionManager))
            {
                locomotionManager.moveDirection = moveDirection;
            }

            yield return new WaitForSeconds(spawnTimer);
            spawnTimer = Random.Range(0.5f, 3.0f);
            canSpawn = true;
        }

        private AttackType RandomType()
        {
            float rng = Random.Range(0.1f, 1.0f);
            return rng >= 0.5f ? AttackType.PROJECTILE : AttackType.LASER;
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