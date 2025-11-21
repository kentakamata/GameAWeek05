using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    private bool isGameOver = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isGameOver) return;

        // 障害物にぶつかった？
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // 下の当たり判定は無視
            if (gameObject.name == "BottomCheck") return;

            // 左右に当たった → ゲームオーバー
            isGameOver = true;
            StartCoroutine(RestartGame());
        }
    }

    private IEnumerator RestartGame()
    {
        Debug.Log("Game Over!");

        // ゲーム停止させる処理（プレイヤー、足場の移動停止）
        Time.timeScale = 0f;

        // 3秒待つ（現実時間）
        yield return new WaitForSecondsRealtime(3f);

        Time.timeScale = 1f;

        // シーンをリロード
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
