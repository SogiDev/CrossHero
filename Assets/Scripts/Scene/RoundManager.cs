using System.Collections;
using UnityEngine;

namespace DS
{
    [RequireComponent(typeof(WorldManager))]
    public class RoundManager : MonoBehaviour
    {
        public static RoundManager Instance { get; private set; }
        private WorldManager worldManager;
        private Spawner[] spawners;

        [Header("Settings")]
        public bool autoStart = false;
        private bool canStart = true;
        public float roundTimer = 5.0f;
        public bool isRoundActive { get; private set; }

        [Header("Data")]
        public int roundCount = 0;
        public float roundScale = 1;
        public int entityCount = 1;
        public GameData.RoundData currentRoundData;

        [Header("Entities")]
        private GameObject[] spawnedEntities;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            worldManager = GetComponent<WorldManager>();
        }

        private void Start()
        {
            currentRoundData = worldManager.playerData.currentRound;
            autoStart = PlayerPrefs.GetInt("Auto Start", 0) == 0 ? false : true;
            roundScale = PlayerPrefs.GetFloat("Wave Scale", 1);
            roundCount = 0;
        }

        private void Update()
        {
            if (isRoundActive && CheckForEntities() <= 0)
            {
                isRoundActive = false;
            }

            if (autoStart && !isRoundActive && canStart) { StartCoroutine(StartRound()); }

        }

        private int CheckForEntities()
        {
            // Number Of Entities Left to Spawn
            int entities = 0;
            foreach (var spawn in spawners)
            {
                entities += spawn.spawnedEntities.Length;
            }

            return entities;
        }

        public IEnumerator StartRound()
        {
            spawners = FindObjectsByType<Spawner>();

            isRoundActive = true;
            canStart = false;

            roundCount++;
            entityCount = Random.Range(5, 10) * roundCount;

            currentRoundData.wave = roundCount;
            yield return new WaitForSeconds(roundTimer);
            canStart = true;
        }

    }
}