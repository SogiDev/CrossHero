using Unity.VisualScripting;
using UnityEngine;

namespace DS {
    [RequireComponent(typeof(BoxCollider2D))]
    public class WorldBarrier : MonoBehaviour
{
        private float damage = 5.0f;

        public bool loopPlayer = false;
        [SerializeField] private Vector3 spawner = Vector3.zero;
        [SerializeField] private float spawnerSize = 2.0f;
        
        private Transform player;

        private void Start()
        {
            player = FindAnyObjectByType<PlayerManager>().transform;
        }

        private void Update()
        {

            if (loopPlayer)
            {
                spawner.x = player.position.x;
                spawner.y = 4;
                spawner.z = player.position.z;
            }

            transform.position = new Vector3(
                player.position.x,
                transform.position.y,
                player.position.z
            );
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.gameObject.CompareTag("Player"))
            {
                var playerManager = player.gameObject.GetComponent<PlayerManager>();
                playerManager.TakeDamage(5.0f);
                player.position = spawner;
            }
        }
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                var playerManager = player.gameObject.GetComponent<PlayerManager>();
                playerManager.TakeDamage(5.0f);
                player.position = spawner;
            }
        }

        private void OGizmosSelected()
        {
            Gizmos.color = Color.wheat;
            Gizmos.DrawSphere(spawner, spawnerSize);
        }
    }
}