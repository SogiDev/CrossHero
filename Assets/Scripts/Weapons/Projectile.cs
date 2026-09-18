using UnityEngine;

namespace DS
{
    [RequireComponent(typeof(ParticleSystem))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] public float damage;
        public GameObject sender;
        private LayerMask senderLayer;
        private string senderTag;
        private int score = 10;

        private void Start()
        {
            if (sender)
            {
                senderLayer = sender.layer;
                senderTag = sender.tag;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.gameObject == sender) { return; }

            if (collision.collider.gameObject.TryGetComponent<CharacterManager>(out var character))
            {
                character.TakeDamage(damage);
            }
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (sender != null)
            {
                if (collision.gameObject == sender) { return; }
            }
            if (collision.gameObject.CompareTag(senderTag)) { return; }
            if (collision.gameObject.layer == senderLayer) { return; }
            
            if (collision.gameObject.TryGetComponent<CharacterManager>(out var character))
            {
                character.TakeDamage(damage);

                if (sender.layer == LayerMask.GetMask("Turret") || sender.layer == LayerMask.GetMask("Player"))
                {
                    WorldManager.Instance.playerData.currentRound.score += score;
                }
            }
            Destroy(gameObject);
        }


    }
}
