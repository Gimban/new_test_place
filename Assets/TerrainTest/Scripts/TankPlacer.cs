using UnityEngine;

public class TankPlacer : MonoBehaviour
{
    public TerrainGenerator terrainGenerator;
    public float initialXPosition = 5f; // 탱크를 배치할 X 좌표

    void Start()
    {
        // TerrainGenerator 스크립트가 존재하는지 확인
        if (terrainGenerator == null)
        {
            Debug.LogError("TerrainGenerator 스크립트가 연결되지 않았습니다.");
            return;
        }

        // 지형 생성 스크립트가 완료될 때까지 기다림
        // 지형이 Start()에서 생성되므로, Start()에서 호출해도 괜찮습니다.
        PlaceTank();
    }

    private void PlaceTank()
    {
        // 지정된 X 좌표에서의 지형 높이를 가져옵니다.
        float terrainHeight = terrainGenerator.GetTerrainHeight(initialXPosition);

        // 탱크의 새로운 위치를 설정합니다.
        // y 위치는 지형 높이에 탱크의 크기 절반을 더하여 지형 위에 놓이도록 합니다.
        // 현재 탱크의 collider가 원점(0,0)에 있다면, transform.position.y에 collider.bounds.extents.y를 더해주면 됩니다.
        Vector3 newPosition = new Vector3(initialXPosition, terrainHeight + 0.5f, transform.position.z);
        transform.position = newPosition;
    }
}