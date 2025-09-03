using UnityEngine;
using System.Linq;

public class TankController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float groundOffset = 0.2f;
    public TerrainGenerator terrainGenerator;
    public float edgeMargin = 2f;

    private int currentPointIndex;
    private float minX, maxX;

    void Start()
    {
        // Rigidbody2D는 더 이상 필요 없으므로 제거
        if (GetComponent<Rigidbody2D>() != null)
        {
            Destroy(GetComponent<Rigidbody2D>());
        }

        if (terrainGenerator != null)
        {
            // 지형의 경계 좌표를 가져옵니다.
            minX = terrainGenerator.GetTerrainPoints().Min(p => p.x);
            maxX = terrainGenerator.GetTerrainPoints().Max(p => p.x);

            // 탱크의 초기 위치 설정
            PlaceTank();
        }
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // 이동할 방향 결정
        int direction = Mathf.Sign(horizontalInput) > 0 ? 1 : -1;

        // 입력이 있을 때만 이동
        if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            // 현재 인덱스를 업데이트하여 다음 지점으로 이동
            currentPointIndex += direction;
            
            // 인덱스 경계 확인 및 clamp
            currentPointIndex = Mathf.Clamp(currentPointIndex, 0, terrainGenerator.GetTerrainPoints().Count - 1);
        }
        
        // 탱크 위치 업데이트
        UpdateTankPosition();
    }
    
    // 탱크 위치를 지형의 점에 맞춰 업데이트하는 함수
    void UpdateTankPosition()
    {
        // 현재 인덱스에 해당하는 지형의 점을 가져옵니다.
        Vector2 groundPoint = terrainGenerator.GetTerrainPoints()[currentPointIndex];

        // 탱크의 위치를 그 점에 맞춰 설정
        Vector3 newPosition = new Vector3(groundPoint.x, groundPoint.y + groundOffset, transform.position.z);
        transform.position = newPosition;

        // 탱크의 회전도 지형의 경사에 맞춥니다.
        // 현재 인덱스와 다음 인덱스 점을 이용하여 경사 계산
        if (currentPointIndex < terrainGenerator.GetTerrainPoints().Count - 1)
        {
            Vector2 nextPoint = terrainGenerator.GetTerrainPoints()[currentPointIndex + 1];
            Vector2 directionVector = (nextPoint - groundPoint).normalized;
            float angle = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    // 초기 탱크 위치를 설정하는 함수
    void PlaceTank()
    {
        // terrainPoints 배열에서 처음으로 경계 마진 안에 있는 인덱스를 찾습니다.
        currentPointIndex = 0;
        for (int i = 0; i < terrainGenerator.GetTerrainPoints().Count; i++)
        {
            if (terrainGenerator.GetTerrainPoints()[i].x >= minX + edgeMargin)
            {
                currentPointIndex = i;
                break;
            }
        }
        UpdateTankPosition();
    }
}