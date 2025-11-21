using UnityEngine;

public class MoveLeftTogether : MonoBehaviour
{
    public float speed = 0f;       // 初期は必ず停止
    public float moveSpeed = 5f;   // Obstacleに触れたらこの速度で動く

    // 判定サイズ（足場の BoxCollider2D と同じ大きさにする）
    public Vector2 checkSize = new Vector2(1f, 1f);

    private void Start()
    {
        speed = 0f; // 生成時は必ず停止
    }

    private void Update()
    {
        // Obstacle 触れているかチェック
        CheckObstacleTouch();

        // 移動
        if (speed != 0f)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;

            if (transform.position.x < -15f)
                Destroy(gameObject);
        }
    }

    private void CheckObstacleTouch()
    {
        // Obstacle が重なっているか確認（Rigidbody 不要）
        Collider2D hit = Physics2D.OverlapBox(
            transform.position,
            checkSize,
            0f,
            LayerMask.GetMask("Obstacle")
        );

        // Obstacle に触れたらスピード設定
        if (hit != null)
        {
            speed = moveSpeed;
        }
    }

    // Sceneビューで当たり範囲を可視化
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, checkSize);
    }
}
