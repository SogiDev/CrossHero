using UnityEngine;
using System.Collections;

namespace DS
{
    public class Crystal : MonoBehaviour
    {


        public float health = 10000;

        private void FixedUpdate()
        {
            if (health <= 0)
            {
                StartCoroutine(WorldManager.Instance.CompleteGame());
                PlayerUI.Instance.CompleteGame(false);
                PlayerUI.Instance.ShowResults();
            }
        }

        public IEnumerator Finish()
        {
            yield return null;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<Projectile>(out var projectile))
            {
                TakeDamage(projectile.damage);
                Destroy(projectile.gameObject);
            }
            else if (other.gameObject.TryGetComponent<SpaceShipManager>(out var character))
            {
                TakeDamage(character.currentDamage);
                Destroy(character.gameObject);
            }

        }
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.gameObject.TryGetComponent<Projectile>(out var projectile))
            {
                TakeDamage(projectile.damage);
                Destroy(projectile.gameObject);
            }
            else if (other.collider.gameObject.TryGetComponent<SpaceShipManager>(out var character))
            {
                TakeDamage(character.currentDamage);
                Destroy(character.gameObject);
            }

        }

        private void TakeDamage(float damage)
        {
            var dps = Mathf.Abs(damage);
            health -= damage;
        }
        

    }
}