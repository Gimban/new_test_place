# 사용법 가이드 (Usage Guide)

클래스 선택 시스템의 코드 사용법과 예제입니다.

## 📋 목차

1. [기본 사용법](#1-기본-사용법)
2. [클래스 선택 관리](#2-클래스-선택-관리)
3. [캐릭터 스폰 및 관리](#3-캐릭터-스폰-및-관리)
4. [씬 전환 관리](#4-씬-전환-관리)
5. [2D 캐릭터 조작](#5-2d-캐릭터-조작)
6. [UI 관리](#6-ui-관리)
7. [이벤트 처리](#7-이벤트-처리)
8. [고급 사용법](#8-고급-사용법)

---

## 1. 기본 사용법

### 1.1 시스템 초기화
```csharp
public class GameController : MonoBehaviour
{
    private void Start()
    {
        // MultiClassManager가 자동으로 Resources에서 클래스 로드
        // MultiClassSelector가 자동으로 클래스 버튼 생성
        // CharacterSpawner가 자동으로 캐릭터 스폰
    }
}
```

### 1.2 기본 흐름
```csharp
// 1. 클래스 선택 씬에서 3개 클래스 선택
// 2. 확인 버튼 클릭 (자동으로 GameScene 전환)
// 3. GameScene에서 자동으로 캐릭터 스폰
// 4. 캐릭터 조작 및 게임 플레이
```

---

## 2. 클래스 선택 관리

### 2.1 MultiClassManager 사용
```csharp
// 선택된 클래스들 가져오기
List<ClassData> selectedClasses = MultiClassManager.Instance.GetSelectedClasses();

// 선택된 클래스 개수 확인
int count = MultiClassManager.Instance.GetSelectedCount();

// 선택 완료 여부 확인
bool isComplete = MultiClassManager.Instance.IsSelectionComplete();

// 특정 순서의 클래스 가져오기
ClassData firstClass = MultiClassManager.Instance.GetFirstClass();
ClassData secondClass = MultiClassManager.Instance.GetSecondClass();
ClassData thirdClass = MultiClassManager.Instance.GetThirdClass();

// 선택 요약 정보 가져오기
string summary = MultiClassManager.Instance.GetSelectionSummary();
```

### 2.2 MultiClassSelector 사용
```csharp
// MultiClassSelector 컴포넌트 가져오기
MultiClassSelector selector = FindObjectOfType<MultiClassSelector>();

// 현재 선택된 클래스들 가져오기
List<ClassData> currentSelection = selector.GetSelectedClasses();

// 선택된 클래스 개수 확인
int selectedCount = selector.GetSelectedCount();

// 특정 클래스가 선택되었는지 확인
bool isSelected = selector.IsClassSelected(someClassData);

// 클래스 목록 새로고침
selector.RefreshClasses();

// 사용 가능한 클래스 개수 확인
int availableCount = selector.GetAvailableClassCount();
```

### 2.3 클래스 데이터 관리
```csharp
// 런타임에 클래스 추가
ClassData newClass = Resources.Load<ClassData>("ClassData/NewClass");
MultiClassManager.Instance.AddClassData(newClass);

// 런타임에 클래스 제거
MultiClassManager.Instance.RemoveClassData(oldClass);

// 선택 초기화
MultiClassManager.Instance.ClearSelection();
```

---

## 3. 캐릭터 스폰 및 관리

### 3.1 CharacterSpawner 사용
```csharp
// CharacterSpawner 가져오기
CharacterSpawner spawner = FindObjectOfType<CharacterSpawner>();

// 스폰된 캐릭터들 가져오기
List<GameObject> characters = spawner.GetSpawnedCharacters();

// 특정 인덱스의 캐릭터 가져오기
GameObject firstCharacter = spawner.GetCharacterByIndex(0);

// 스폰된 캐릭터 개수 확인
int characterCount = spawner.GetSpawnedCharacterCount();

// 캐릭터 새로고침 (선택된 클래스로 다시 스폰)
spawner.RefreshCharacters();

// 기존 캐릭터들 제거
spawner.ClearSpawnedCharacters();
```

### 3.2 스폰 설정 변경
```csharp
// 스폰 중심점 변경
spawner.SetSpawnCenter(newCenterTransform);

// 스폰 반경 변경
spawner.SetSpawnRadius(10f);

// 배치 방식 변경 (Inspector에서 설정)
// - Spawn In Circle: 원형 배치
// - Spawn In Line: 일직선 배치
// - Spawn In Grid: 그리드 배치
```

---

## 4. 씬 전환 관리

### 4.1 SceneTransitionManager 사용
```csharp
// 클래스 선택 씬으로 이동
SceneTransitionManager.Instance.GoToClassSelection();

// 게임 씬으로 이동 (데이터 검증 포함)
SceneTransitionManager.Instance.SafeTransitionToGame();

// 캐릭터 스폰 씬으로 이동
SceneTransitionManager.Instance.GoToCharacterSpawnScene();
```

### 4.2 데이터 검증
```csharp
// 선택된 클래스 데이터 확인
bool hasClasses = SceneTransitionManager.Instance.HasSelectedClasses();
int classCount = SceneTransitionManager.Instance.GetSelectedClassCount();

// 선택 완료 여부 확인
bool isComplete = SceneTransitionManager.Instance.IsSelectionComplete();

// 현재 씬 이름 확인
string currentScene = SceneTransitionManager.Instance.GetCurrentSceneName();
```

### 4.3 안전한 씬 전환
```csharp
// 데이터 검증 후 씬 전환
if (SceneTransitionManager.Instance.ValidateDataBeforeTransition())
{
    SceneTransitionManager.Instance.GoToGameScene();
}
```

---

## 5. 2D 캐릭터 조작

### 5.1 CharacterInfo 사용
```csharp
// 캐릭터 정보 가져오기
CharacterInfo charInfo = character.GetComponent<CharacterInfo>();
ClassData classData = charInfo.GetClassData();
string charName = charInfo.GetCharacterName();

// 캐릭터 선택/해제
charInfo.SelectCharacter();
charInfo.DeselectCharacter();

// 캐릭터 상태 확인
bool isAlive = charInfo.IsAlive();
bool isMoving = charInfo.isMoving;
bool isAttacking = charInfo.isAttacking;
```

### 5.2 2D 이동
```csharp
// 2D 좌표로 이동
charInfo.MoveTo(new Vector2(5, 3));

// 3D 좌표로 이동 (Z축은 무시)
charInfo.MoveTo(new Vector3(5, 3, 0));

// 물리 기반 이동
charInfo.MoveWithPhysics(new Vector2(1, 0));
```

### 5.3 2D 애니메이션
```csharp
// 애니메이션 재생
charInfo.PlayWalkAnimation();
charInfo.PlayIdleAnimation();
```

### 5.4 전투 시스템
```csharp
// 캐릭터 공격
CharacterInfo target = otherCharacter.GetComponent<CharacterInfo>();
charInfo.Attack(target);

// 데미지 받기
charInfo.TakeDamage(20);

// 체력 회복
charInfo.Heal(10);

// 체력 비율 확인
float healthPercent = charInfo.GetHealthPercentage();
```

### 5.5 특수 능력
```csharp
// 특수 능력 사용 가능 여부 확인
if (charInfo.CanUseSpecialAbility())
{
    // 특수 능력 정보 가져오기
    string abilityInfo = charInfo.GetSpecialAbilityInfo();
    
    // 특수 능력 사용
    charInfo.UseSpecialAbility();
}
```

---

## 6. UI 관리

### 6.1 CharacterUI2D 사용
```csharp
// 캐릭터 UI 가져오기
CharacterUI2D charUI = character.GetComponent<CharacterUI2D>();

// 이름 업데이트
charUI.UpdateName("새로운 이름");

// UI 표시/숨기기
charUI.SetHealthBarVisible(true);
charUI.SetNameTagVisible(false);

// UI 스케일 조정
charUI.SetUIScale(1.5f);

// 색상 변경
charUI.SetNameTagColor(Color.red);
charUI.SetHealthBarColor(Color.green);
```

### 6.2 MultiClassSelectorUI 사용
```csharp
// MultiClassSelectorUI 가져오기
MultiClassSelectorUI selectorUI = FindObjectOfType<MultiClassSelectorUI>();

// UI 표시/숨기기
selectorUI.ShowClassSelector();
selectorUI.HideClassSelector();

// 클래스 목록 새로고침
selectorUI.RefreshClasses();

// 사용 가능한 클래스 개수 확인
int availableCount = selectorUI.GetAvailableClassCount();

// 특정 클래스 사용 가능 여부 확인
bool isAvailable = selectorUI.IsClassAvailable(someClassData);
```

---

## 7. 이벤트 처리

### 7.1 클래스 선택 이벤트
```csharp
public class GameController : MonoBehaviour
{
    private void Start()
    {
        // MultiClassSelector 이벤트 연결
        MultiClassSelector selector = FindObjectOfType<MultiClassSelector>();
        if (selector != null)
        {
            selector.OnClassesSelected += OnClassesSelected;
        }
    }

    private void OnClassesSelected(List<ClassData> selectedClasses)
    {
        Debug.Log($"클래스들 선택됨: {selectedClasses.Count}개");
        for (int i = 0; i < selectedClasses.Count; i++)
        {
            Debug.Log($"{i + 1}. {selectedClasses[i].GetDisplayName()}");
        }
        
        // 게임 로직 처리
        ProcessClassSelection(selectedClasses);
    }

    private void ProcessClassSelection(List<ClassData> classes)
    {
        // 선택된 클래스들에 따른 게임 로직
        // 예: 특별한 보상, 스탯 보너스 등
    }
}
```

### 7.2 캐릭터 이벤트
```csharp
public class CharacterController : MonoBehaviour
{
    private void Start()
    {
        // 캐릭터 선택 이벤트 (마우스 클릭)
        // CharacterInfo의 OnMouseDown()에서 자동 처리
    }

    private void OnMouseDown()
    {
        // 캐릭터 클릭 시 추가 로직
        CharacterInfo charInfo = GetComponent<CharacterInfo>();
        if (charInfo != null)
        {
            Debug.Log($"캐릭터 클릭됨: {charInfo.GetCharacterName()}");
        }
    }
}
```

---

## 8. GameScene 관리

### 8.1 GameSceneController 사용
```csharp
// GameSceneController 가져오기
GameSceneController gameController = FindObjectOfType<GameSceneController>();

// 스폰된 캐릭터들 가져오기
List<GameObject> characters = gameController.GetSpawnedCharacters();

// 캐릭터 개수 확인
int characterCount = gameController.GetCharacterCount();

// 특정 인덱스의 캐릭터 가져오기
CharacterInfo charInfo = gameController.GetCharacterByIndex(0);

// 캐릭터 새로고침
gameController.RefreshCharacters();

// 캐릭터 제거
gameController.ClearCharacters();

// 클래스 선택 씬으로 돌아가기
gameController.GoBackToClassSelection();
```

### 8.2 디버그 기능
```csharp
// 캐릭터 정보 출력 (Context Menu에서도 사용 가능)
gameController.ShowCharacterInfo();

// 캐릭터 이동 테스트 (Context Menu에서도 사용 가능)
gameController.TestCharacterMovement();
```

### 8.3 GameScene 초기화
```csharp
public class CustomGameController : MonoBehaviour
{
    private void Start()
    {
        // GameSceneController가 자동으로 초기화
        // 선택된 클래스 확인
        // 캐릭터 자동 스폰
        // UI 설정
    }
}
```

---

## 9. 고급 사용법

### 8.1 커스텀 클래스 데이터 활용
```csharp
// ClassData의 메서드들 활용
ClassData classData = charInfo.GetClassData();
if (classData != null)
{
    // 클래스 정보 가져오기
    string displayName = classData.GetDisplayName();
    string description = classData.GetDescription();
    string summary = classData.GetClassSummary();
    
    // 스탯 계산
    int damage = classData.CalculateDamage(10);
    int defense = classData.CalculateDefense(15);
    float moveSpeed = classData.GetMoveSpeed();
    
    // 특수 능력 확인
    if (classData.CanUseSpecialAbility())
    {
        string abilityInfo = classData.GetSpecialAbilityInfo();
        charInfo.UseSpecialAbility();
    }
    
    // 클래스 색상 가져오기
    Color primaryColor = classData.GetPrimaryColor();
    Color secondaryColor = classData.GetSecondaryColor();
    Color accentColor = classData.GetAccentColor();
    
    // 애니메이션 확인
    bool hasAnimator = classData.HasAnimationController();
    bool hasSpriteAnim = classData.HasSpriteAnimation();
    int totalSprites = classData.GetTotalSprites();
}
```

### 8.2 동적 클래스 관리
```csharp
public class DynamicClassManager : MonoBehaviour
{
    private void Start()
    {
        // 런타임에 새로운 클래스 추가
        AddNewClassAtRuntime();
        
        // 기존 클래스 수정
        ModifyExistingClass();
        
        // 클래스 목록 새로고침
        RefreshAllSelectors();
    }

    private void AddNewClassAtRuntime()
    {
        // 새로운 ClassData 생성
        ClassData newClass = ScriptableObject.CreateInstance<ClassData>();
        newClass.className = "NewClass";
        newClass.health = 120;
        newClass.attack = 15;
        
        // 매니저에 추가
        MultiClassManager.Instance.AddClassData(newClass);
        
        // UI 새로고침
        MultiClassSelectorUI selectorUI = FindObjectOfType<MultiClassSelectorUI>();
        if (selectorUI != null)
        {
            selectorUI.RefreshClasses();
        }
    }

    private void ModifyExistingClass()
    {
        // 기존 클래스 수정
        List<ClassData> allClasses = MultiClassManager.Instance.GetAllClasses();
        foreach (ClassData classData in allClasses)
        {
            if (classData.className == "Warrior")
            {
                classData.health += 20; // 체력 증가
                classData.attack += 5;  // 공격력 증가
            }
        }
    }

    private void RefreshAllSelectors()
    {
        // 모든 MultiClassSelector 새로고침
        MultiClassSelector[] selectors = FindObjectsOfType<MultiClassSelector>();
        foreach (MultiClassSelector selector in selectors)
        {
            selector.RefreshClasses();
        }
    }
}
```

### 8.3 커스텀 스폰 로직
```csharp
public class CustomSpawner : MonoBehaviour
{
    public void SpawnCharactersInCustomPattern()
    {
        CharacterSpawner spawner = FindObjectOfType<CharacterSpawner>();
        if (spawner == null) return;

        List<ClassData> selectedClasses = MultiClassManager.Instance.GetSelectedClasses();
        
        // 커스텀 배치 패턴
        for (int i = 0; i < selectedClasses.Count; i++)
        {
            Vector3 customPosition = CalculateCustomPosition(i);
            SpawnCharacterAtPosition(selectedClasses[i], customPosition, i);
        }
    }

    private Vector3 CalculateCustomPosition(int index)
    {
        // 커스텀 위치 계산 로직
        float angle = (360f / 3) * index * Mathf.Deg2Rad;
        float radius = 5f;
        return new Vector3(
            Mathf.Cos(angle) * radius,
            Mathf.Sin(angle) * radius,
            0
        );
    }

    private void SpawnCharacterAtPosition(ClassData classData, Vector3 position, int index)
    {
        // 커스텀 스폰 로직
        GameObject characterPrefab = classData.GetCharacterPrefab();
        if (characterPrefab != null)
        {
            GameObject character = Instantiate(characterPrefab, position, Quaternion.identity);
            // 추가 설정...
        }
    }
}
```

---

## 🎯 실전 예제

### 예제 1: 간단한 클래스 선택 게임
```csharp
public class SimpleClassGame : MonoBehaviour
{
    private void Start()
    {
        // 클래스 선택 이벤트 연결
        MultiClassSelector selector = FindObjectOfType<MultiClassSelector>();
        if (selector != null)
        {
            selector.OnClassesSelected += StartGame;
        }
    }

    private void StartGame(List<ClassData> selectedClasses)
    {
        Debug.Log("게임 시작!");
        
        // 선택된 클래스들로 게임 시작
        foreach (ClassData classData in selectedClasses)
        {
            Debug.Log($"선택된 클래스: {classData.GetDisplayName()}");
        }
        
        // 게임 씬으로 전환
        SceneTransitionManager.Instance.SafeTransitionToGame();
    }
}
```

### 예제 2: 캐릭터 전투 시스템
```csharp
public class BattleSystem : MonoBehaviour
{
    private List<CharacterInfo> characters = new List<CharacterInfo>();

    private void Start()
    {
        // 스폰된 캐릭터들 가져오기
        CharacterSpawner spawner = FindObjectOfType<CharacterSpawner>();
        if (spawner != null)
        {
            List<GameObject> characterObjects = spawner.GetSpawnedCharacters();
            foreach (GameObject character in characterObjects)
            {
                CharacterInfo charInfo = character.GetComponent<CharacterInfo>();
                if (charInfo != null)
                {
                    characters.Add(charInfo);
                }
            }
        }
    }

    public void StartBattle()
    {
        // 전투 시작
        if (characters.Count >= 2)
        {
            CharacterInfo attacker = characters[0];
            CharacterInfo target = characters[1];
            
            attacker.Attack(target);
        }
    }
}
```

---

**다음 단계**: [API 레퍼런스](API_REFERENCE.md)를 참조하여 모든 클래스와 메서드를 확인하세요!
