using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHitCheck : MonoBehaviour
{
    public Vector2 bodySize = new Vector2(1f, 1f);
    public LayerMask obstacleLayer;

    private bool isGameOver = false;

    void Update()
    {
        if (isGameOver) return;

        // ======== 足元の判定（ゲームオーバーにしない）========
        Vector3 footPos = transform.position + new Vector3(0, -bodySize.y * 0.55f, 0);
        Vector2 footSize = new Vector2(bodySize.x * 0.9f, bodySize.y * 0.2f);

        Collider2D footHit = Physics2D.OverlapBox(footPos, footSize, 0, obstacleLayer);
        if (footHit != null)
        {
            return;
        }

        // ======== 右側に側面判定を置く ========
        Vector3 rightPos = transform.position + new Vector3(bodySize.x * 0.55f, 0, 0);
        Vector2 sideSize = new Vector2(bodySize.x * 0.2f, bodySize.y * 0.9f);

        Collider2D rightHit = Physics2D.OverlapBox(rightPos, sideSize, 0, obstacleLayer);

        if (rightHit != null)
        {
            Debug.Log("右側面ヒット → GAME OVER");
            GameOver();
        }
    }

    private void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;
        StartCoroutine(RestartAfterDelay(3f));
    }

    private System.Collections.IEnumerator RestartAfterDelay(float sec)
    {
        float timer = 0;
        while (timer < sec)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDrawGizmosSelected()
    {
        // 足元（青）
        Gizmos.color = Color.blue;
        Vector3 footPos = transform.position + new Vector3(0, -bodySize.y * 0.55f, 0);
        Gizmos.DrawWireCube(footPos, new Vector2(bodySize.x * 0.9f, bodySize.y * 0.2f));

        // 右側（赤）
        Gizmos.color = Color.red;
        Vector3 rightPos = transform.position + new Vector3(bodySize.x * 0.55f, 0, 0);
        Gizmos.DrawWireCube(rightPos, new Vector2(bodySize.x * 0.2f, bodySize.y * 0.9f));
    }
}
