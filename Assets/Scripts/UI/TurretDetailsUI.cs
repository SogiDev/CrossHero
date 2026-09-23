using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DS
{

    public class TurretDetailsUI : MonoBehaviour
    {
        public void UpdateDetails(TurretData turretData)
        {
            displayTurretImage.sprite = turretData.turretImage;
            displayTurretName.text = turretData.turretName;
            displayTurretAttackType.text = turretData.attackType.ToString();
            displayTurretCost.text = turretData.cost.ToString();
            displayTurretMaxHealth.text = turretData.maxHealth.ToString();
            displayTurretMaxEnergy.text = turretData.maxEnergy.ToString();
            displayTurretBaseDamage.text = turretData.baseDamage.ToString();
            displayTurretBaseProjectileSpeed.text = turretData.baseProjectileSpeed.ToString();
        }
        public void UpdateDetails(Crystal crystal, int value = 10000)
        {
            displayTurretImage.sprite = crystal.gameObject.GetComponent<SpriteRenderer>().sprite;
            displayTurretName.text = "Crystal";

            var zones = FindObjectsByType<ZoneCreator>();

            displayTurretAttackType.text = "Zones: " + zones.Length.ToString();
            var cost = value * zones.Length;
            displayTurretCost.text = cost.ToString();
        }

        [SerializeField] private Image displayTurretImage;
        [SerializeField] private TMP_Text displayTurretName;
        [SerializeField] private TMP_Text displayTurretAttackType;
        [SerializeField] private TMP_Text displayTurretCost;
        [SerializeField] private TMP_Text displayTurretMaxHealth;
        [SerializeField] private TMP_Text displayTurretMaxEnergy;
        [SerializeField] private TMP_Text displayTurretBaseDamage;
        [SerializeField] private TMP_Text displayTurretBaseProjectileSpeed;
    }
}