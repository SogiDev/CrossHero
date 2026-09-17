using UnityEngine;

namespace DS
{
    [RequireComponent(typeof(ParticleSystem))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class Projectile : MonoBehaviour
    {
        private Rigidbody2D rigidbody;
        private ParticleSystem particleSystem;

        [SerializeField] public float damage;
        public GameObject sender;
        private Vector2 moveAmount = new (1, 0);
        public float speed = 5;

        public void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            particleSystem = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            //rigidbody.linearVelocity = moveAmount * (Time.deltaTime * speed);
            rigidbody.linearVelocityX = moveAmount.x * (Time.deltaTime * speed * 500);
            rigidbody.linearVelocityY = moveAmount.y * (Time.deltaTime * speed * 500);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.gameObject == sender) { return; }

            if (collision.collider.gameObject.TryGetComponent<CharacterManager>(out var character))
            {
                character.TakeDamage(damage);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == sender) { return; }
            if (collision.gameObject.CompareTag(sender.tag)) { return; }
            if (collision.gameObject.layer == sender.layer) { return; }
            
            if (collision.gameObject.TryGetComponent<CharacterManager>(out var character))
            {
                character.TakeDamage(damage);
            }
        }


    }
}
