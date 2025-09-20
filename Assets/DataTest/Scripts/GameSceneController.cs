using UnityEngine;
using System.Collections.Generic;

public class GameSceneController : MonoBehaviour
{
    [Header("Game Scene Settings")]
    public string classSelectionSceneName = "ClassSelection";
    public bool autoSpawnCharacters = true;
    public bool showDebugInfo = true;

    [Header("UI References")]
    public GameObject backToSelectionButton;
    public GameObject characterInfoPanel;
    public UnityEngine.UI.Text characterCountText;

    private CharacterSpawner characterSpawner;
    private List<GameObject> spawnedCharacters = new List<GameObject>();

    private void Start()
    {
        InitializeGameScene();
        
        if (autoSpawnCharacters)
        {
            SpawnCharacters();
        }
        
        SetupUI();
    }

    private void InitializeGameScene()
    {
        Debug.Log("GameScene 초기화 시작");
        
        // CharacterSpawner 찾기
        characterSpawner = FindObjectOfType<CharacterSpawner>();
        if (characterSpawner == null)
        {
            Debug.LogWarning("CharacterSpawner를 찾을 수 없습니다. 기본 CharacterSpawner를 생성합니다.");
            CreateDefaultCharacterSpawner();
        }

        // 선택된 클래스 확인
        if (MultiClassManager.Instance != null)
        {
            List<ClassData> selectedClasses = MultiClassManager.Instance.GetSelectedClasses();
            Debug.Log($"선택된 클래스 수: {selectedClasses.Count}");
            
            if (selectedClasses.Count == 0)
            {
                Debug.LogWarning("선택된 클래스가 없습니다. 클래스 선택 씬으로 돌아갑니다.");
                GoBackToClassSelection();
                return;
            }
        }
        else
        {
            Debug.LogError("MultiClassManager를 찾을 수 없습니다!");
        }
    }

    private void CreateDefaultCharacterSpawner()
    {
        GameObject spawnerObj = new GameObject("CharacterSpawner");
        characterSpawner = spawnerObj.AddComponent<CharacterSpawner>();
        
        // 기본 설정
        characterSpawner.spawnRadius = 5f;
        characterSpawner.spawnInCircle = true;
        
        // 스폰 중심점 설정
        GameObject centerObj = new GameObject("SpawnCenter");
        centerObj.transform.position = Vector3.zero;
        characterSpawner.spawnCenter = centerObj.transform;
    }

    private void SpawnCharacters()
    {
        if (characterSpawner == null)
        {
            Debug.LogError("CharacterSpawner가 없습니다!");
            return;
        }

        Debug.Log("캐릭터 스폰 시작");
        
        // 캐릭터 스폰
        characterSpawner.SpawnCharacters();
        
        // 스폰된 캐릭터들 가져오기
        spawnedCharacters = characterSpawner.GetSpawnedCharacters();
        
        Debug.Log($"총 {spawnedCharacters.Count}개의 캐릭터가 스폰되었습니다.");
        
        // 각 캐릭터 정보 출력
        for (int i = 0; i < spawnedCharacters.Count; i++)
        {
            CharacterInfo charInfo = spawnedCharacters[i].GetComponent<CharacterInfo>();
            if (charInfo != null)
            {
                Debug.Log($"캐릭터 {i + 1}: {charInfo.GetCharacterName()}");
            }
        }
    }

    private void SetupUI()
    {
        // 뒤로가기 버튼 설정
        if (backToSelectionButton != null)
        {
            UnityEngine.UI.Button backButton = backToSelectionButton.GetComponent<UnityEngine.UI.Button>();
            if (backButton != null)
            {
                backButton.onClick.AddListener(GoBackToClassSelection);
            }
        }

        // 캐릭터 정보 패널 설정
        if (characterInfoPanel != null)
        {
            characterInfoPanel.SetActive(showDebugInfo);
        }

        // 캐릭터 개수 텍스트 업데이트
        UpdateCharacterCountText();
    }

    private void UpdateCharacterCountText()
    {
        if (characterCountText != null)
        {
            int count = spawnedCharacters.Count;
            characterCountText.text = $"스폰된 캐릭터: {count}개";
        }
    }

    public void GoBackToClassSelection()
    {
        Debug.Log("클래스 선택 씬으로 돌아갑니다.");
        
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.GoToClassSelection();
        }
        else
        {
            Debug.LogError("SceneTransitionManager를 찾을 수 없습니다!");
        }
    }

    public void RefreshCharacters()
    {
        if (characterSpawner != null)
        {
            Debug.Log("캐릭터 새로고침");
            characterSpawner.RefreshCharacters();
            spawnedCharacters = characterSpawner.GetSpawnedCharacters();
            UpdateCharacterCountText();
        }
    }

    public void ClearCharacters()
    {
        if (characterSpawner != null)
        {
            Debug.Log("캐릭터 제거");
            characterSpawner.ClearSpawnedCharacters();
            spawnedCharacters.Clear();
            UpdateCharacterCountText();
        }
    }

    // 외부에서 호출할 수 있는 메서드들
    public List<GameObject> GetSpawnedCharacters()
    {
        return new List<GameObject>(spawnedCharacters);
    }

    public int GetCharacterCount()
    {
        return spawnedCharacters.Count;
    }

    public CharacterInfo GetCharacterByIndex(int index)
    {
        if (index >= 0 && index < spawnedCharacters.Count)
        {
            return spawnedCharacters[index].GetComponent<CharacterInfo>();
        }
        return null;
    }

    // 디버그용 메서드
    [ContextMenu("Show Character Info")]
    public void ShowCharacterInfo()
    {
        Debug.Log("=== 캐릭터 정보 ===");
        for (int i = 0; i < spawnedCharacters.Count; i++)
        {
            CharacterInfo charInfo = spawnedCharacters[i].GetComponent<CharacterInfo>();
            if (charInfo != null)
            {
                Debug.Log($"캐릭터 {i + 1}: {charInfo.GetCharacterName()} (체력: {charInfo.currentHealth}/{charInfo.maxHealth})");
            }
        }
    }

    [ContextMenu("Test Character Movement")]
    public void TestCharacterMovement()
    {
        if (spawnedCharacters.Count > 0)
        {
            CharacterInfo charInfo = spawnedCharacters[0].GetComponent<CharacterInfo>();
            if (charInfo != null)
            {
                Vector2 randomPosition = new Vector2(
                    Random.Range(-5f, 5f),
                    Random.Range(-5f, 5f)
                );
                charInfo.MoveTo(randomPosition);
                Debug.Log($"캐릭터를 {randomPosition}로 이동시킵니다.");
            }
        }
    }
}
