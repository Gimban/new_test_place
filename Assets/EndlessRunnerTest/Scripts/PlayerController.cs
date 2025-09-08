using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float forwardSpeed = 5f; // 앞으로 나아가는 속도
    public float sideSpeed = 5f; // 좌우 이동 속도

    void Update()
    {
        // 앞으로 계속 이동
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // 좌우 입력 처리
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * sideSpeed * Time.deltaTime);
    }
}