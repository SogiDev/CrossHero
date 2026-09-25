using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace DS
{

    [RequireComponent(typeof(AudioSource))]
    public class CharacterSoundManager : MonoBehaviour
    {
        
        private float activeVolume;
        private AudioSource source;
        [SerializeField] private AudioClip closeAttackSound;
        [SerializeField] private AudioClip projectileAttackSound;
        [SerializeField] private AudioClip laserAttackSound;
 
        private void Awake()
        {
            source = GetComponent<AudioSource>();
        }

        private void FixedUpdate()
        {
            
        }

        public void PlayAttack(AttackType type)
        {
            source.volume = AudioManager.Instance.GetVolume(AudioManager.AudioType.SFX);
            switch (type)
            {
                case AttackType.CLOSE:
                    source.clip = closeAttackSound;
                    break;
                case AttackType.PROJECTILE:
                    source.clip = projectileAttackSound;
                    break;
                case AttackType.LASER:
                    source.clip = laserAttackSound;
                    break;
                default:
                    return;
            }
            source.Play();
        }

    }
}