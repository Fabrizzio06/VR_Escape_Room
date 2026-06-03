using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Random Spawns")]
    public GameObject[] randomPrefabs;
    public int randomCount = 30;

    [Header("Unique Spawns")]
    public GameObject[] uniquePrefabs;

    [Header("Settings")]
    public float spawnRadius = 2f;
    public float dropHeight = 4f;

    void Start()
    {
        // Unique items spawn at base center so the pile buries them
        foreach (var prefab in uniquePrefabs)
        {
            Vector3 pos = transform.position + new Vector3(
                Random.Range(-spawnRadius * 0.25f, spawnRadius * 0.25f),
                0f,
                Random.Range(-spawnRadius * 0.25f, spawnRadius * 0.25f)
            );
            Instantiate(prefab, pos, Random.rotation);
        }

        // Random items drop from above to pile on top
        for (int i = 0; i < randomCount; i++)
        {
            GameObject prefab = randomPrefabs[Random.Range(0, randomPrefabs.Length)];
            Vector2 circle = Random.insideUnitCircle * spawnRadius;
            Vector3 pos = transform.position + new Vector3(circle.x, dropHeight, circle.y);
            Instantiate(prefab, pos, Random.rotation);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
        Gizmos.color = new Color(1f, 1f, 0f, 0.15f);
        Gizmos.DrawSphere(transform.position, spawnRadius);
    }
}