using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TerrainGenerator : MonoBehaviour
{
    // Inspector에서 설정할 수 있는 변수들
    public float terrainWidth = 20f; // 지형의 전체 너비
    public float pointSpacing = 1f;  // 지형 점들의 간격
    public float minHeight = -2f;    // 지형의 최소 높이
    public float maxHeight = 2f;     // 지형의 최대 높이

    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;
    private List<Vector2> terrainPoints = new List<Vector2>();
    private int numSurfacePoints; // 지형 표면 점의 개수를 저장할 변수
    private float seed;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();

        seed = Random.Range(0f, 1000f);

        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        // 지형 표면 점 개수 계산
        numSurfacePoints = Mathf.FloorToInt(terrainWidth / pointSpacing) + 1;

        terrainPoints.Clear();

        // 지형 표면 점 생성
        for (int i = 0; i < numSurfacePoints; i++)
        {
            float x = i * pointSpacing;

            // 퍼린 노이즈를 사용하여 y값 생성
            float perlinY = Mathf.PerlinNoise(x * 0.1f + seed, seed);

            // PerlinNoise 값(0.0 ~ 1.0)을 minHeight ~ maxHeight 범위로 스케일링
            float y = perlinY * (maxHeight - minHeight) + minHeight;

            // 지형 점의 y값이 minHeight 아래로 내려가지 않도록 제한
            y = Mathf.Max(y, minHeight);

            terrainPoints.Add(new Vector2(x, y));
        }

        // Line Renderer를 닫힌 형태로 만들기 위해 아래쪽 점들 추가
        float lastX = (numSurfacePoints - 1) * pointSpacing;
        terrainPoints.Add(new Vector2(lastX, minHeight - 5f));
        terrainPoints.Add(new Vector2(0, minHeight - 5f));

        // Line Renderer 설정
        lineRenderer.positionCount = terrainPoints.Count;
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
        lineRenderer.loop = true; // loop=true로 설정하면 마지막 점과 첫 점이 자동으로 연결됩니다.

        // Vector2 리스트를 Vector3 배열로 변환하여 Line Renderer에 설정
        lineRenderer.SetPositions(terrainPoints.Select(p => (Vector3)p).ToArray());
        UpdateCollider();
    }

    void UpdateCollider()
    {
        // Edge Collider 2D 업데이트
        // 표면 점들만 사용하여 콜라이더를 생성합니다.
        edgeCollider.points = terrainPoints.Take(numSurfacePoints).ToArray();
    }

    public void DestroyTerrain(Vector2 explosionPosition, float explosionRadius)
    {
        // 지형 표면 점들만 수정합니다.
        for (int i = 0; i < numSurfacePoints; i++)
        {
            Vector2 point = terrainPoints[i];
            float distance = Vector2.Distance(point, explosionPosition);

            if (distance < explosionRadius)
            {
                // 폭발 중심에 가까울수록 더 많이 파이도록 y값 조정
                float destructionFactor = (explosionRadius - distance) / explosionRadius;
                point.y -= destructionFactor * 2; // 파괴 강도 조절

                // 지형 점의 y값이 minHeight 아래로 내려가지 않도록 제한
                point.y = Mathf.Max(point.y, minHeight); // 이 줄을 추가합니다.

                terrainPoints[i] = point;
            }
        }

        // Line Renderer와 Collider 업데이트
        lineRenderer.SetPositions(terrainPoints.Select(p => (Vector3)p).ToArray());
        UpdateCollider();
    }

    // TerrainGenerator.cs 스크립트에 추가할 함수
    public List<Vector2> GetTerrainPoints()
    {
        return terrainPoints;
    }

    // 주어진 X 좌표에서의 지형 높이를 반환합니다.
    public float GetTerrainHeight(float x)
    {
        // 간단한 보간을 통해 높이 계산
        // 지형 점을 순회하며 주어진 x 값에 해당하는 높이를 찾습니다.
        for (int i = 0; i < numSurfacePoints - 1; i++)
        {
            if (x >= terrainPoints[i].x && x <= terrainPoints[i+1].x)
            {
                float dx = terrainPoints[i+1].x - terrainPoints[i].x;
                float dy = terrainPoints[i+1].y - terrainPoints[i].y;
                float ratio = (x - terrainPoints[i].x) / dx;
                return terrainPoints[i].y + dy * ratio;
            }
        }
        return minHeight;
    }
}