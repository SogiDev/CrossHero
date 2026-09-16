using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileObject", menuName = "Scriptable Objects/ProjectileObject")]
public class ProjectileObject : ScriptableObject
{
    public Texture2D texture;
    public float damage;

    public bool isExplosive;

}
