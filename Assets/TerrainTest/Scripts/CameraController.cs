using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CameraController : MonoBehaviour
{
    public TerrainGenerator terrainGenerator;

    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("카메라 컴포넌트를 찾을 수 없습니다.");
        }
    }

    void Start()
    {
        if (terrainGenerator != null)
        {
            AdjustCameraPosition();
        }
    }

    public void AdjustCameraPosition()
    {
        if (terrainGenerator == null)
        {
            Debug.LogError("TerrainGenerator가 연결되지 않았습니다.");
            return;
        }

        var terrainPoints = terrainGenerator.GetTerrainPoints();
        if (terrainPoints == null || terrainPoints.Count == 0)
        {
            Debug.LogError("지형 점이 생성되지 않았습니다.");
            return;
        }

        // 지형의 X, Y 좌표 범위를 계산합니다.
        float minX = terrainPoints.Min(p => p.x);
        float maxX = terrainPoints.Max(p => p.x);
        float minY = terrainPoints.Min(p => p.y);
        float maxY = terrainPoints.Max(p => p.y);

        // 지형의 너비와 높이를 계산합니다.
        float terrainWidth = maxX - minX;
        float terrainHeight = maxY - minY;

        // 카메라의 Orthographic Size를 지형 너비와 높이에 맞춰 계산합니다.
        // 좌우 여백을 없애기 위해 너비를 기준으로 카메라의 크기를 먼저 결정합니다.
        float orthoSizeByWidth = terrainWidth / (2f * cam.aspect);
        
        // 지형 높이를 기준으로 Orthographic Size를 계산합니다.
        float orthoSizeByHeight = terrainHeight / 2f;

        // 최종 Orthographic Size를 두 값 중 더 큰 값으로 설정합니다.
        cam.orthographicSize = Mathf.Max(orthoSizeByWidth, orthoSizeByHeight);

        // 카메라의 위치를 계산합니다.
        // X 위치는 지형의 중앙으로 설정합니다.
        float cameraX = minX + terrainWidth / 2f;
        
        // Y 위치는 카메라의 가장 아래쪽이 지형의 최저점(minY)에 오도록 설정합니다.
        // minY + cam.orthographicSize
        float cameraY = minY + cam.orthographicSize;
        
        cam.transform.position = new Vector3(cameraX, cameraY, cam.transform.position.z);
    }
}