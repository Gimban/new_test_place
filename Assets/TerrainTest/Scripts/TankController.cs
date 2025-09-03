using UnityEngine;
using System.Linq;

public class TankController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float groundCheckDistance = 1f;
    public float groundOffset = 0.2f;
    public LayerMask groundLayer;
    public TerrainGenerator terrainGenerator;
    public float edgeMargin = 2f;
    public float sleepThreshold = 0.1f;

    private Rigidbody2D rb;
    private float minX, maxX;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.sleepMode = RigidbodySleepMode2D.StartAwake;
        
        if (terrainGenerator != null)
        {
            minX = terrainGenerator.GetTerrainPoints().Min(p => p.x);
            maxX = terrainGenerator.GetTerrainPoints().Max(p => p.x);
        }
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        bool atLeftEdge = (transform.position.x <= minX + edgeMargin) && (horizontalInput < 0);
        bool atRightEdge = (transform.position.x >= maxX - edgeMargin) && (horizontalInput > 0);

        if (atLeftEdge || atRightEdge)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * groundCheckDistance, Color.red);

        if (hit.collider != null)
        {
            Vector3 newPosition = hit.point + hit.normal * groundOffset;
            transform.position = newPosition;

            Vector2 groundNormal = hit.normal;
            Vector2 surfaceRight = new Vector2(groundNormal.y, -groundNormal.x);

            float angle = Mathf.Atan2(groundNormal.y, groundNormal.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // 입력이 없으면 속도를 즉시 0으로 만듭니다.
            if (Mathf.Abs(horizontalInput) < 0.1f)
            {
                rb.linearVelocity = Vector2.zero;
            }
            else
            {
                rb.linearVelocity = surfaceRight * horizontalInput * moveSpeed;
            }

            // 정지 상태일 때 Rigidbody2D를 재우기
            if (rb.linearVelocity.magnitude < sleepThreshold && Mathf.Abs(horizontalInput) < 0.1f)
            {
                rb.Sleep();
            }
        }
        else
        {
            if (terrainGenerator != null)
            {
                float terrainHeight = terrainGenerator.GetTerrainHeight(transform.position.x);
                Vector3 newPosition = new Vector3(transform.position.x, terrainHeight + 0.5f, transform.position.z);
                transform.position = newPosition;
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
}