using UnityEngine;

public class FootBlock : MonoBehaviour
{
    public float moveLeftSpeed = 8f;
    private bool moveLeft = false;

    private void Update()
    {
        if (moveLeft)
        {
            transform.position += Vector3.left * moveLeftSpeed * Time.deltaTime;

            if (transform.position.x < -15f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Obstacle レイヤー以外は無視
        if (collision.gameObject.layer != LayerMask.NameToLayer("Obstacle"))
            return;

        // 足場も左へ流すモードにする
        moveLeft = true;

        // Obstacle を左へ動かすスクリプトを追加（最初は停止）
        MoveLeftTogether m = collision.gameObject.GetComponent<MoveLeftTogether>();
        if (m == null)
        {
            m = collision.gameObject.AddComponent<MoveLeftTogether>();
        }

        // 足場のスピードをそのまま渡す
        m.speed = moveLeftSpeed;
    }
}
