using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    [Header("씬 설정")]
    public string classSelectionSceneName = "ClassSelection";
    public string gameSceneName = "GameScene";
    public string characterSpawnSceneName = "CharacterSpawn";

    [Header("데이터 유지 설정")]
    public bool preserveClassData = true;
    public bool preserveCharacterData = true;

    private void Awake()
    {
        // 싱글톤 패턴
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 씬이 로드될 때마다 데이터 확인
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"씬 로드됨: {scene.name}");
        
        // 게임 씬이 로드되면 캐릭터 스폰
        if (scene.name == gameSceneName || scene.name == characterSpawnSceneName)
        {
            StartCoroutine(SpawnCharactersAfterDelay());
        }
    }

    private System.Collections.IEnumerator SpawnCharactersAfterDelay()
    {
        // 씬 로드 후 잠시 대기 (다른 오브젝트들이 초기화될 시간)
        yield return new WaitForSeconds(0.1f);
        
        // CharacterSpawner 찾기
        CharacterSpawner spawner = FindObjectOfType<CharacterSpawner>();
        if (spawner != null)
        {
            spawner.RefreshCharacters();
        }
        else
        {
            Debug.LogWarning("CharacterSpawner를 찾을 수 없습니다. 캐릭터가 스폰되지 않을 수 있습니다.");
        }
    }

    // 클래스 선택 씬으로 이동
    public void GoToClassSelection()
    {
        LoadScene(classSelectionSceneName);
    }

    // 게임 씬으로 이동
    public void GoToGameScene()
    {
        LoadScene(gameSceneName);
    }

    // 캐릭터 스폰 씬으로 이동
    public void GoToCharacterSpawnScene()
    {
        LoadScene(characterSpawnSceneName);
    }

    private void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("씬 이름이 설정되지 않았습니다!");
            return;
        }

        Debug.Log($"씬 전환: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }

    // 선택된 클래스 데이터 확인
    public bool HasSelectedClasses()
    {
        if (MultiClassManager.Instance != null)
        {
            return MultiClassManager.Instance.HasSelectedClasses();
        }
        return false;
    }

    public List<ClassData> GetSelectedClasses()
    {
        if (MultiClassManager.Instance != null)
        {
            return MultiClassManager.Instance.GetSelectedClasses();
        }
        return new List<ClassData>();
    }

    // 선택된 클래스 개수 확인
    public int GetSelectedClassCount()
    {
        if (MultiClassManager.Instance != null)
        {
            return MultiClassManager.Instance.GetSelectedCount();
        }
        return 0;
    }

    // 선택 완료 여부 확인
    public bool IsSelectionComplete()
    {
        if (MultiClassManager.Instance != null)
        {
            return MultiClassManager.Instance.IsSelectionComplete();
        }
        return false;
    }

    // 씬 전환 전 데이터 검증
    public bool ValidateDataBeforeTransition()
    {
        if (!HasSelectedClasses())
        {
            Debug.LogWarning("선택된 클래스가 없습니다. 클래스 선택 씬으로 이동합니다.");
            GoToClassSelection();
            return false;
        }

        if (!IsSelectionComplete())
        {
            Debug.LogWarning("3개의 클래스를 모두 선택해야 합니다. 클래스 선택 씬으로 이동합니다.");
            GoToClassSelection();
            return false;
        }

        return true;
    }

    // 안전한 씬 전환 (데이터 검증 포함)
    public void SafeTransitionToGame()
    {
        if (ValidateDataBeforeTransition())
        {
            GoToGameScene();
        }
    }

    // 데이터 초기화
    public void ClearAllData()
    {
        if (MultiClassManager.Instance != null)
        {
            MultiClassManager.Instance.ClearSelection();
        }
        Debug.Log("모든 데이터가 초기화되었습니다.");
    }

    // 현재 씬 이름 가져오기
    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    // 특정 씬이 로드되었는지 확인
    public bool IsSceneLoaded(string sceneName)
    {
        return SceneManager.GetActiveScene().name == sceneName;
    }
}
