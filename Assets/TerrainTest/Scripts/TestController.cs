using UnityEngine;

public class TestController : MonoBehaviour
{
    public GameObject projectilePrefab; // 포탄 프리팹을 Inspector에 연결

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 클릭 위치를 월드 좌표로 변환
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // 2D이므로 z를 0으로 설정

            // 마우스 클릭 위치에 포탄 생성
            Instantiate(projectilePrefab, mousePosition, Quaternion.identity);
        }
    }
}