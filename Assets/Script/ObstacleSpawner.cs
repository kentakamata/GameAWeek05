using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject blockPrefab; // 1段分のブロック
    public float spawnInterval = 2f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnObstacle();
            timer = 0;
        }
    }

    void SpawnObstacle()
    {
        // ランダムで1〜4段にする
        int height = Random.Range(1, 5);

        for (int i = 0; i < height; i++)
        {
            Vector3 pos = transform.position + new Vector3(0, i * 1f, 0);
            Instantiate(blockPrefab, pos, Quaternion.identity);
        }
    }
}
