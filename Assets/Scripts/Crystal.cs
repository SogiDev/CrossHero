using UnityEngine;

namespace DS
{
    public class Crystal : MonoBehaviour
    {


        public float health = 10000;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<Projectile>(out var projectile))
            {
                health -= projectile.damage;
                Destroy(projectile.gameObject);
            }

            if (other.gameObject.TryGetComponent<SpaceShipManager>(out var character))
            {
                health -= character.health;
                Destroy(character.gameObject);
            }

        }
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.gameObject.TryGetComponent<Projectile>(out var projectile))
            {
                health -= projectile.damage;
                Destroy(projectile.gameObject);
            }

            if (other.collider.gameObject.TryGetComponent<SpaceShipManager>(out var character))
            {
                health -= character.health;
                Destroy(character.gameObject);
            }

        }
        

    }
}