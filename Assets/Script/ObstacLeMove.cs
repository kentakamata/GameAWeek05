using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    public float speed = 3f;
    public float acceleration = 0.1f;   // 毎秒スピード上昇
    private float lifeTime = 10f;       // 画面外処理

    private void Update()
    {
        speed += acceleration * Time.deltaTime;

        transform.position += Vector3.left * speed * Time.deltaTime;

        // 左に消えたら削除
        if (transform.position.x < -15f)
            Destroy(gameObject);

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
            Destroy(gameObject);
    }
}
