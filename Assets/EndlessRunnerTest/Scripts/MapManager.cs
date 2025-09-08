using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject[] mapPrefabs; // 맵 조각 프리팹 배열
    private Transform playerTransform; // 플레이어의 Transform
    private float zSpawn = 0; // 맵이 생성될 z축 위치
    private float mapLength = 30f; // 맵 조각의 길이
    private int numberOfMaps = 5; // 미리 생성할 맵 조각의 개수
    private List<GameObject> activeMaps = new List<GameObject>(); // 활성화된 맵 리스트

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        for (int i = 0; i < numberOfMaps; i++)
        {
            SpawnMap(Random.Range(0, mapPrefabs.Length));
        }
    }

    void Update()
    {
        if (playerTransform.position.z > zSpawn - numberOfMaps * mapLength)
        {
            SpawnMap(Random.Range(0, mapPrefabs.Length));
            DeleteMap();
        }
    }

    public void SpawnMap(int mapIndex)
    {
        GameObject go = Instantiate(mapPrefabs[mapIndex], transform.forward * zSpawn, transform.rotation);
        activeMaps.Add(go);
        zSpawn += mapLength;
    }

    private void DeleteMap()
    {
        Destroy(activeMaps[0]);
        activeMaps.RemoveAt(0);
    }
}