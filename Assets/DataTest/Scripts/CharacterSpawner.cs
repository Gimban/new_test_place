using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CharacterSpawner : MonoBehaviour
{
    [Header("2D 스폰 설정")]
    public Transform spawnCenter;
    public float spawnRadius = 5f;
    public bool spawnInCircle = true;
    public bool spawnInLine = false;
    public bool spawnInGrid = false;
    public int gridColumns = 3;

    [Header("2D 캐릭터 설정")]
    public GameObject defaultCharacterPrefab2D;
    public Sprite[] characterSprites; // 각 클래스별 스프라이트
    public Color[] characterColors = { Color.red, Color.blue, Color.green }; // 클래스별 색상

    [Header("디버그")]
    public bool showSpawnGizmos = true;

    private List<GameObject> spawnedCharacters = new List<GameObject>();
    private List<ClassData> selectedClasses = new List<ClassData>();

    private void Start()
    {
        // MultiClassManager에서 선택된 클래스들 가져오기
        LoadSelectedClasses();
        
        // 선택된 클래스들이 있으면 스폰
        if (selectedClasses.Count > 0)
        {
            SpawnCharacters();
        }
        else
        {
            Debug.LogWarning("선택된 클래스가 없습니다. 캐릭터를 스폰할 수 없습니다.");
        }
    }

    private void LoadSelectedClasses()
    {
        if (MultiClassManager.Instance != null)
        {
            selectedClasses = MultiClassManager.Instance.GetSelectedClasses();
            Debug.Log($"로드된 선택된 클래스 수: {selectedClasses.Count}");
        }
        else
        {
            Debug.LogError("MultiClassManager를 찾을 수 없습니다!");
        }
    }

    public void SpawnCharacters()
    {
        // 기존 캐릭터들 제거
        ClearSpawnedCharacters();

        if (selectedClasses.Count == 0)
        {
            Debug.LogWarning("스폰할 클래스가 없습니다.");
            return;
        }

        // 스폰 위치 계산
        List<Vector3> spawnPositions = CalculateSpawnPositions(selectedClasses.Count);

        // 각 클래스에 대해 캐릭터 스폰
        for (int i = 0; i < selectedClasses.Count; i++)
        {
            SpawnCharacter(selectedClasses[i], spawnPositions[i], i);
        }

        Debug.Log($"{selectedClasses.Count}개의 캐릭터가 스폰되었습니다.");
    }

    private List<Vector3> CalculateSpawnPositions(int count)
    {
        List<Vector3> positions = new List<Vector3>();
        Vector3 center = spawnCenter != null ? spawnCenter.position : transform.position;

        if (spawnInCircle)
        {
            // 원형으로 배치 (2D)
            for (int i = 0; i < count; i++)
            {
                float angle = (360f / count) * i * Mathf.Deg2Rad;
                Vector3 offset = new Vector3(
                    Mathf.Cos(angle) * spawnRadius,
                    Mathf.Sin(angle) * spawnRadius,
                    0 // 2D이므로 Z축은 0
                );
                positions.Add(center + offset);
            }
        }
        else if (spawnInLine)
        {
            // 일직선으로 배치 (2D)
            float spacing = spawnRadius * 2f / Mathf.Max(1, count - 1);
            for (int i = 0; i < count; i++)
            {
                Vector3 offset = new Vector3(
                    (i - (count - 1) * 0.5f) * spacing,
                    0,
                    0
                );
                positions.Add(center + offset);
            }
        }
        else if (spawnInGrid)
        {
            // 그리드로 배치 (2D)
            int rows = Mathf.CeilToInt((float)count / gridColumns);
            float cellWidth = spawnRadius * 2f / gridColumns;
            float cellHeight = spawnRadius * 2f / rows;
            
            for (int i = 0; i < count; i++)
            {
                int row = i / gridColumns;
                int col = i % gridColumns;
                
                Vector3 offset = new Vector3(
                    (col - (gridColumns - 1) * 0.5f) * cellWidth,
                    (row - (rows - 1) * 0.5f) * cellHeight,
                    0
                );
                positions.Add(center + offset);
            }
        }
        else
        {
            // 기본적으로 원형 배치
            for (int i = 0; i < count; i++)
            {
                float angle = (360f / count) * i * Mathf.Deg2Rad;
                Vector3 offset = new Vector3(
                    Mathf.Cos(angle) * spawnRadius,
                    Mathf.Sin(angle) * spawnRadius,
                    0
                );
                positions.Add(center + offset);
            }
        }

        return positions;
    }

    private void SpawnCharacter(ClassData classData, Vector3 position, int index)
    {
        // 캐릭터 프리팹 결정
        GameObject characterPrefab = GetCharacterPrefab(classData);
        
        if (characterPrefab == null)
        {
            Debug.LogWarning($"클래스 '{classData.className}'에 대한 프리팹을 찾을 수 없습니다. 기본 프리팹을 사용합니다.");
            characterPrefab = defaultCharacterPrefab2D;
        }

        if (characterPrefab == null)
        {
            Debug.LogError("캐릭터 프리팹이 설정되지 않았습니다!");
            return;
        }

        // 캐릭터 인스턴스 생성
        GameObject character = Instantiate(characterPrefab, position, Quaternion.identity);
        spawnedCharacters.Add(character);

        // 캐릭터 이름 설정
        character.name = $"{classData.className}_{index + 1}";

        // 캐릭터 컴포넌트 설정
        SetupCharacter(character, classData, index);

        Debug.Log($"캐릭터 스폰됨: {character.name} at {position}");
    }

    private GameObject GetCharacterPrefab(ClassData classData)
    {
        // ClassData에 2D 프리팹이 설정되어 있으면 사용
        if (classData != null && classData.characterPrefab2D != null)
        {
            return classData.characterPrefab2D;
        }
        
        // 기본 2D 프리팹 사용
        return defaultCharacterPrefab2D;
    }

    private void SetupCharacter(GameObject character, ClassData classData, int index)
    {
        // 캐릭터 정보 컴포넌트 추가
        CharacterInfo characterInfo = character.GetComponent<CharacterInfo>();
        if (characterInfo == null)
        {
            characterInfo = character.AddComponent<CharacterInfo>();
        }
        characterInfo.SetClassData(classData);
        characterInfo.SetCharacterIndex(index);

        // 2D 스프라이트 설정
        SpriteRenderer spriteRenderer = character.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            // ClassData에 스프라이트가 있으면 사용
            if (classData != null && classData.characterSprite != null)
            {
                spriteRenderer.sprite = classData.characterSprite;
            }
            // 또는 클래스별 스프라이트 배열에서 선택
            else if (characterSprites != null && characterSprites.Length > 0)
            {
                int spriteIndex = index % characterSprites.Length;
                spriteRenderer.sprite = characterSprites[spriteIndex];
            }

            // 클래스별 색상 설정
            if (characterColors != null && characterColors.Length > 0)
            {
                int colorIndex = index % characterColors.Length;
                spriteRenderer.color = characterColors[colorIndex];
            }
        }

        // 2D 물리 설정
        if (classData != null && classData.useRigidbody2D)
        {
            Rigidbody2D rb2d = character.GetComponent<Rigidbody2D>();
            if (rb2d == null)
            {
                rb2d = character.AddComponent<Rigidbody2D>();
            }
            rb2d.mass = classData.mass;
            rb2d.linearDamping = classData.drag;
            rb2d.angularDamping = classData.angularDrag;
        }

        // 2D 콜라이더 설정
        CircleCollider2D collider2D = character.GetComponent<CircleCollider2D>();
        if (collider2D == null)
        {
            collider2D = character.AddComponent<CircleCollider2D>();
        }
        if (classData != null)
        {
            collider2D.radius = classData.colliderRadius;
        }

        // 캐릭터 회전 설정 (2D에서는 Z축 회전)
        if (spawnCenter != null)
        {
            Vector3 lookDirection = (spawnCenter.position - character.transform.position).normalized;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            character.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    public void ClearSpawnedCharacters()
    {
        foreach (GameObject character in spawnedCharacters)
        {
            if (character != null)
            {
                DestroyImmediate(character);
            }
        }
        spawnedCharacters.Clear();
    }

    public void RefreshCharacters()
    {
        LoadSelectedClasses();
        SpawnCharacters();
    }

    // 외부에서 호출할 수 있는 메서드들
    public List<GameObject> GetSpawnedCharacters()
    {
        return new List<GameObject>(spawnedCharacters);
    }

    public GameObject GetCharacterByIndex(int index)
    {
        if (index >= 0 && index < spawnedCharacters.Count)
        {
            return spawnedCharacters[index];
        }
        return null;
    }

    public int GetSpawnedCharacterCount()
    {
        return spawnedCharacters.Count;
    }

    // 스폰 위치 변경
    public void SetSpawnCenter(Transform newCenter)
    {
        spawnCenter = newCenter;
    }

    public void SetSpawnRadius(float newRadius)
    {
        spawnRadius = newRadius;
    }

    // 디버그용 기즈모
    private void OnDrawGizmos()
    {
        if (!showSpawnGizmos) return;

        Vector3 center = spawnCenter != null ? spawnCenter.position : transform.position;
        
        // 스폰 중심점 표시
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, 0.5f);

        // 스폰 반경 표시
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(center, spawnRadius);

        // 스폰 위치들 표시
        if (selectedClasses.Count > 0)
        {
            List<Vector3> positions = CalculateSpawnPositions(selectedClasses.Count);
            Gizmos.color = Color.green;
            foreach (Vector3 pos in positions)
            {
                Gizmos.DrawWireCube(pos, Vector3.one * 0.5f);
            }
        }
    }
}
