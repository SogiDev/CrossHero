using UnityEngine;

public class Cloner : MonoBehaviour
{

    [SerializeField] private GameObject duplicate;
    [SerializeField] private Vector3 spawnMin = Vector3.one;
    [SerializeField] private Vector3 spawnMax = Vector3.one;
    [SerializeField] private Vector2 countMin = Vector2.one;
    [SerializeField] private Vector2 countMax = Vector2.one;
    public Color defaultColor = Color.purple;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        for (int x = Mathf.RoundToInt(countMin.x); x < countMax.x; x++)
        {
            for (int y = Mathf.RoundToInt(countMin.y); y < countMax.y; y++)
            {
                var sizeX = spawnMax.x - spawnMin.x;
                var sizeY = spawnMax.y - spawnMin.y;
                var position = new Vector3(x * sizeX, y * sizeY) + transform.position;


                var clone = Instantiate(duplicate, position, Quaternion.identity, gameObject.transform);
                clone.SetActive(true);

                var n = x + y;
                clone.name = duplicate.name + ": " + n.ToString();

                foreach (var renderer in GetComponentsInChildren<SpriteRenderer>())
                {
                    renderer.color = defaultColor;
                }

            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Vector2 Spawn Area
        Gizmos.color = Color.yellow;

        // Gizmos Spawn Area
        Gizmos.DrawLine(new Vector3(spawnMin.x, spawnMin.y) + transform.position,
            new Vector3(spawnMin.x, spawnMax.y) + transform.position);
        Gizmos.DrawLine(new Vector3(spawnMin.x, spawnMax.y) + transform.position,
            new Vector3(spawnMax.x, spawnMax.y) + transform.position);
        Gizmos.DrawLine(new Vector3(spawnMax.x, spawnMax.y) + transform.position,
            new Vector3(spawnMax.x, spawnMin.y) + transform.position);
        Gizmos.DrawLine(new Vector3(spawnMin.x, spawnMin.y) + transform.position,
            new Vector3(spawnMax.x, spawnMin.y) + transform.position);
    }
}
