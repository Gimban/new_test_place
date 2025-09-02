using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float explosionRadius = 3f;
    public GameObject explosionEffect; // 폭발 효과를 위한 Particle System 프리팹

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Terrain")
        {
            // TerrainGenerator 스크립트 찾기
            TerrainGenerator terrain = collision.gameObject.GetComponent<TerrainGenerator>();
            if (terrain != null)
            {
                terrain.DestroyTerrain(transform.position, explosionRadius);
            }

            // 폭발 효과 생성 및 포탄 오브젝트 삭제
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}