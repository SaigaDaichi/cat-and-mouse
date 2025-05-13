using UnityEngine;
using System.Collections;
using TMPro;
using System;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public GameObject effect;
    public int moveLimit = 5;
    public float moveSpeed = 5f;
    public LayerMask obstacleLayer;
    private bool isMoving = false;
    public Vector2Int currentDirection = Vector2Int.up; // 初期は上向き
    public TextMeshProUGUI moveLimitText;

    void Start()
    {
        if (moveLimitText == null)
        {
            moveLimitText = GameObject.Find("MoveLimitText")?.GetComponent<TextMeshProUGUI>();
        }
    }

    private void Update()
    {
        if (isMoving) return; // 移動中は入力受付しない

        moveLimitText.SetText(Convert.ToString(moveLimit));

        Vector2Int dir = Vector2Int.zero;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            dir = Vector2Int.up;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            currentDirection = dir;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            dir = Vector2Int.down;
            transform.rotation = Quaternion.Euler(0, 0, 180);
            currentDirection = dir;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            dir = Vector2Int.left;
            transform.rotation = Quaternion.Euler(0, 0, 90);
            currentDirection = dir;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) 
        {
            dir = Vector2Int.right;
            transform.rotation = Quaternion.Euler(0, 0, -90);
            currentDirection = dir;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            TryBite();
        }

        if (dir != Vector2.zero)
        {
            Vector2 targetPos = (Vector2)transform.position + dir;

            if (!IsBlocked(targetPos))
            {
                StartCoroutine(MoveTo(targetPos));
            }
        }
    }

    IEnumerator MoveTo(Vector2 targetPos)
    {
        isMoving = true;

        Vector2 start = transform.position;
        float t = 0;
        float duration = 0.15f;

        moveLimit--;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.position = Vector2.Lerp(start, targetPos, t);
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
    }
    bool IsBlocked(Vector3 targetPos)
    {
        Collider2D hit = Physics2D.OverlapPoint(targetPos);
        if (hit != null)
        {
            // ここで判定 or イベント処理！
            IBlocker blocker = hit.GetComponent<IBlocker>();
            if (blocker != null)
            {
                blocker.OnBlocked(this); // ← ここで発火できる！
            }

            return Physics2D.OverlapCircle(targetPos, 0.1f, obstacleLayer) != null;
        }

        return false;
    }

    void TryBite()
    {
        Vector2 biteTarget = (Vector2)transform.position + (Vector2)currentDirection;

        Collider2D hit = Physics2D.OverlapPoint(biteTarget);
        if (hit != null && hit.CompareTag("Cat"))
        {
            Debug.Log("噛んだ");
            Instantiate(effect, biteTarget, Quaternion.identity);

            //白い猫の手を噛んだ場合
            if (hit.GetComponent<CatWhite>())
            {
                StartCoroutine(hit.GetComponent<CatWhite>().Damage());
            }
            //猫の腕を噛んだ時
            if (hit.GetComponent<CatArm>())
            {
                hit.GetComponent<CatArm>().Damage();
            }

            //hit.GetComponent<Cat>()?.OnBitten();
        }
        else
        {
            Debug.Log("猫居ない");
        }
    }

}
