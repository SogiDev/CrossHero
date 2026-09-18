using Unity.Loading;
using UnityEngine;

namespace DS
{
    [CreateAssetMenu(fileName = "TurretData", menuName = "Scriptable Objects/TurretData")]
    public class TurretData : ScriptableObject
    {
        // Look Into Conent Directories

        [Header("Turret Info")]
        public string turretName = "Turret";
        public Sprite turretImage;
        public AttackType attackType;
        public int cost;

        [Header("Default Stats")]

        public int maxHealth = 10;
        public int maxEnergy = 10;
        public int baseDamage = 10;
        public int baseProjectileSpeed = 10;
        public float searchRadius = 20;
        public float baseTimer = 1f;

    }
}