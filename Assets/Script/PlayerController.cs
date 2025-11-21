using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpStep = 1f;

    [Header("足場プレハブ")]
    public GameObject footPrefab;
    public float footOffsetY = -1f;

    [Header("疑似重力設定")]
    public float gravity = 10f;
    public float groundCheckDistance = 0.8f;

    private float velocityY = 0f;

    public float minY = -2.5f;
    public float maxY = 1.5f;

    private void Update()
    {
        HandleVerticalMove();
        ApplyGravity();
        GroundCheck();
        ClampPosition();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpAndSpawnFoot();
        }
    }

    private void HandleVerticalMove()
    {
        float v = Input.GetAxisRaw("Vertical");
        transform.position += new Vector3(0, v * moveSpeed * Time.deltaTime, 0);
    }

    private void ApplyGravity()
    {
        velocityY -= gravity * Time.deltaTime;

        if (velocityY < -10f)
            velocityY = -10f;

        transform.position += new Vector3(0, velocityY * Time.deltaTime, 0);
    }

    private void GroundCheck()
    {
        Vector2 checkSize = new Vector2(0.9f, 0.1f);
        Vector2 checkPos = (Vector2)transform.position + new Vector2(0f, -0.4f);

        Collider2D hit = Physics2D.OverlapBox(
            checkPos,
            checkSize,
            0f,
            LayerMask.GetMask("Foot")
        );

        if (hit != null)
        {
            float groundY = hit.bounds.max.y;
            float offset = 0.02f;

            transform.position = new Vector3(
                transform.position.x,
                groundY + offset,
                transform.position.z
            );

            velocityY = 0f;
        }
    }

    // ★ ジャンプ + 足場を必ず「1.5の倍数の位置」に生成
    private void JumpAndSpawnFoot()
    {
        // プレイヤーを1段ジャンプさせる
        transform.position += new Vector3(0, jumpStep, 0);

        // スナップ許可位置（プレイヤーと足場共通）
        float[] allowedY = { -2.5f, -1.5f, -0.5f, 0.5f,1.5f,2.5f
        
        };

        //-------------------------------------------
        // ★ プレイヤー位置を allowedY にスナップ
        //-------------------------------------------
        float playerSnappedY = allowedY[0];
        float minDiffPlayer = Mathf.Abs(transform.position.y - allowedY[0]);

        for (int i = 1; i < allowedY.Length; i++)
        {
            float diff = Mathf.Abs(transform.position.y - allowedY[i]);
            if (diff < minDiffPlayer)
            {
                minDiffPlayer = diff;
                playerSnappedY = allowedY[i];
            }
        }

        // プレイヤー位置確定
        transform.position = new Vector3(transform.position.x, playerSnappedY, 0);

        //-------------------------------------------
        // ★ 足場の下端 Y にスナップ
        //-------------------------------------------

        // プレイヤーの底面座標
        float playerBottom = GetComponent<SpriteRenderer>().bounds.min.y;

        float footSnappedY = allowedY[0];
        float minDiffFoot = Mathf.Abs(playerBottom - allowedY[0]);

        for (int i = 1; i < allowedY.Length; i++)
        {
            float diff = Mathf.Abs(playerBottom - allowedY[i]);
            if (diff < minDiffFoot)
            {
                minDiffFoot = diff;
                footSnappedY = allowedY[i];
            }
        }

        // 足場を生成
        Vector3 spawnPos = new Vector3(transform.position.x, footSnappedY, 0);
        Instantiate(footPrefab, spawnPos, Quaternion.identity);
    }


    private void ClampPosition()
    {
        Vector3 pos = transform.position;

        if (pos.y < minY)
        {
            pos.y = minY;
            velocityY = 0;
        }

        if (pos.y > maxY)
        {
            pos.y = maxY;
            velocityY = 0;
        }

        transform.position = pos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector2 checkSize = new Vector2(0.9f, 0.1f);
        Vector2 checkPos = (Vector2)transform.position + new Vector2(0f, 0f);
        Gizmos.DrawWireCube(checkPos, checkSize);
    }
}
