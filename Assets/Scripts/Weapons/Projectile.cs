using UnityEngine;

namespace DS
{
    [RequireComponent(typeof(ParticleSystem))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(AudioSource))]
    public class Projectile : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] public float damage;
        public GameObject sender;
        public GameObject target;
        private int score = 10;

        [Header("Clips")]
        public AudioClip[] sounds;
        [SerializeField] private AudioSource source;

        private void Start()
        {
            source = GetComponent<AudioSource>();
            source.clip = sounds[Random.Range(0, sounds.Length)];
            source.volume = AudioManager.Instance.GetVolume(AudioManager.AudioType.GAMEPLAY_SFX);
            source.Play();
        }

        private void FixedUpdate()
        {
            LookAtTarget();
        }
        
        private void LookAtTarget()
        {
            if (target == null) { return; }
            transform.LookAt(target.transform);
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            DealDamage(collision.collider.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            DealDamage(collision.gameObject);
        }

        private void DealDamage(GameObject obj)
        {
            if (sender != null) {
                if (obj.CompareTag(sender.tag)) { return; }
                if (obj.layer == sender.layer) { return; }
            }

            if (obj.TryGetComponent<CharacterManager>(out var character))
            {
                character.TakeDamage(damage);

                if (sender != null)
                {
                    if (sender.CompareTag("Turret") || sender.CompareTag("Player"))
                    {
                        WorldManager.Instance.playerData.AddScore(score);
                    }
                }
                Destroy(gameObject);
                return;
            }

            if (obj.TryGetComponent<Crystal>(out var crystal))
            {
                if (sender.CompareTag("Turret")) { return; }
                crystal.health -= damage;
            }
        }
    }
}
