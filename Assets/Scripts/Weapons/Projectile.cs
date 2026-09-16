using UnityEngine;

namespace DS
{
    [RequireComponent(typeof(ParticleSystem))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class Projectile : MonoBehaviour
    {
        private Rigidbody2D rigidbody;

        [SerializeField] public float damage;
        public GameObject sender;
        private Vector2 moveAmount = new (1, 0);
        public float speed = 5;

        private void Update()
        {
            rigidbody.linearVelocity = moveAmount * (Time.deltaTime * speed);
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
            if (collision.gameObject.TryGetComponent<CharacterManager>(out var character))
            {
                character.TakeDamage(damage);
            }
        }


    }
}
