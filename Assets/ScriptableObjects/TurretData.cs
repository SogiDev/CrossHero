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
        public Sprite projectileImage;
        public AttackType attackType;
        public int cost;
        public int score;

        [Header("Default Stats")]

        public int maxHealth = 10;
        public int maxEnergy = 10;
        public int baseDamage = 10;
        public int baseProjectileSpeed = 10;
        [Range(1, 25)]
        public float searchRadius = 10;
        public float baseTimer = 1f;

    }
}