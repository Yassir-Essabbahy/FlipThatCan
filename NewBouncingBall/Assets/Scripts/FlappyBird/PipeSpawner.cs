using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    public GameObject pipePairPrefab;
    public float spawnEvery = 1.2f;
    public float spawnX = 10f;

    [Header("Gap position random")]
    public float minY = -2f;
    public float maxY = 2f;

    float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnEvery)
        {
            timer = 0f;
            Spawn();
        }
    }

    void Spawn()
    {
        float y = Random.Range(minY, maxY);
        Vector3 pos = new Vector3(spawnX, y, 0f);
        Instantiate(pipePairPrefab, pos, Quaternion.identity);
    }
}
