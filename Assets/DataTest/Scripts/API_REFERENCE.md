# API 레퍼런스 (API Reference)

모든 클래스와 메서드의 상세한 설명입니다.

## 📋 목차

1. [ClassData](#classdata)
2. [MultiClassSelector](#multiclassselector)
3. [MultiClassManager](#multiclassmanager)
4. [MultiClassSelectorUI](#multiclassselectorui)
5. [CharacterSpawner](#characterspawner)
6. [CharacterInfo](#characterinfo)
7. [CharacterUI2D](#characterui2d)
8. [SceneTransitionManager](#scenetransitionmanager)
9. [GameSceneController](#gamescenecontroller)

---

## ClassData

2D 캐릭터 데이터를 정의하는 ScriptableObject입니다.

### 주요 속성

| 속성 | 타입 | 설명 |
|------|------|------|
| `className` | string | 클래스 이름 |
| `classDescription` | string | 클래스 설명 |
| `classIcon` | Sprite | 클래스 아이콘 |
| `characterSprite` | Sprite | 2D 캐릭터 스프라이트 |
| `characterPrefab2D` | GameObject | 2D 캐릭터 프리팹 |
| `health` | int | 체력 (기본값: 100) |
| `attack` | int | 공격력 (기본값: 10) |
| `defense` | int | 방어력 (기본값: 5) |
| `speed` | int | 속도 (기본값: 8) |
| `mana` | int | 마나 (기본값: 50) |

### 주요 메서드

#### 정보 가져오기
```csharp
bool IsValid()
// 클래스 데이터가 유효한지 확인

string GetDisplayName()
// 표시용 클래스 이름 반환

string GetDescription()
// 클래스 설명 반환

Sprite GetClassIcon()
// 클래스 아이콘 반환

Sprite GetCharacterSprite()
// 캐릭터 스프라이트 반환

GameObject GetCharacterPrefab()
// 캐릭터 프리팹 반환
```

#### 스탯 계산
```csharp
int CalculateDamage(int baseDamage)
// 기본 데미지에 공격력을 더한 최종 데미지 계산

int CalculateDefense(int incomingDamage)
// 들어오는 데미지에서 방어력을 뺀 실제 데미지 계산

float GetMoveSpeed()
// 이동 속도 반환

float GetHealthPercentage(int currentHealth)
// 현재 체력의 비율 계산
```

#### 특수 능력
```csharp
bool CanUseSpecialAbility()
// 특수 능력 사용 가능 여부 확인

string GetSpecialAbilityInfo()
// 특수 능력 정보 반환
```

#### 색상 관리
```csharp
Color GetPrimaryColor()
// 기본 색상 반환

Color GetSecondaryColor()
// 보조 색상 반환

Color GetAccentColor()
// 강조 색상 반환

void SetColors(Color primary, Color secondary, Color accent)
// 색상들 설정
```

#### 애니메이션
```csharp
bool HasAnimationController()
// 애니메이션 컨트롤러 보유 여부 확인

bool HasSpriteAnimation()
// 스프라이트 애니메이션 보유 여부 확인

int GetTotalSprites()
// 총 스프라이트 개수 반환
```

#### 유틸리티
```csharp
string GetClassSummary()
// 클래스 요약 정보 반환

bool Equals(ClassData other)
// 다른 ClassData와 비교

static bool operator ==(ClassData a, ClassData b)
// 동등성 연산자

static bool operator !=(ClassData a, ClassData b)
// 비동등성 연산자
```

---

## MultiClassSelector

3개 클래스 선택 UI의 핵심 로직을 담당합니다.

### 주요 속성

| 속성 | 타입 | 설명 |
|------|------|------|
| `classButtonContainer` | Transform | 클래스 버튼들이 배치될 부모 오브젝트 |
| `classButtonPrefab` | GameObject | 클래스 버튼 프리팹 |
| `selectedClassesContainer` | Transform | 선택된 클래스 슬롯들이 배치될 부모 오브젝트 |
| `selectedClassSlotPrefab` | GameObject | 선택된 클래스 슬롯 프리팹 |
| `availableClasses` | List<ClassData> | 사용 가능한 클래스 목록 |
| `useManagerClasses` | bool | MultiClassManager에서 클래스 자동 로드 여부 |
| `maxSelections` | int | 최대 선택 가능한 클래스 수 (기본값: 3) |

### 주요 메서드

#### 클래스 선택 관리
```csharp
void ToggleClassSelection(ClassData classData)
// 클래스 선택/해제 토글

void AddClassSelection(ClassData classData)
// 클래스 선택 추가

void RemoveClassSelection(ClassData classData)
// 클래스 선택 제거

void ClearAllSelections()
// 모든 선택 초기화
```

#### UI 관리
```csharp
void RefreshClasses()
// 클래스 목록 새로고침

void CreateClassButtons()
// 클래스 버튼들 생성

void UpdateUI()
// UI 업데이트
```

#### 정보 가져오기
```csharp
List<ClassData> GetSelectedClasses()
// 현재 선택된 클래스들 반환

int GetSelectedCount()
// 선택된 클래스 개수 반환

bool IsClassSelected(ClassData classData)
// 특정 클래스가 선택되었는지 확인

int GetAvailableClassCount()
// 사용 가능한 클래스 개수 반환

bool IsClassAvailable(ClassData classData)
// 특정 클래스가 사용 가능한지 확인
```

#### 클래스 데이터 관리
```csharp
void AddClassData(ClassData classData)
// 클래스 데이터 추가

void RemoveClassData(ClassData classData)
// 클래스 데이터 제거
```

#### 이벤트
```csharp
System.Action<List<ClassData>> OnClassesSelected
// 클래스 선택 완료 이벤트
```

---

## MultiClassManager

다중 클래스 데이터를 관리하는 싱글톤 매니저입니다.

### 주요 속성

| 속성 | 타입 | 설명 |
|------|------|------|
| `allClassData` | List<ClassData> | 모든 클래스 데이터 목록 |

### 주요 메서드

#### 클래스 데이터 관리
```csharp
void SetSelectedClasses(List<ClassData> classes)
// 선택된 클래스들 설정

List<ClassData> GetSelectedClasses()
// 선택된 클래스들 반환

List<ClassData> GetAllClasses()
// 모든 클래스들 반환

ClassData GetClassByName(string className)
// 이름으로 클래스 찾기

void AddClassData(ClassData classData)
// 클래스 데이터 추가

void RemoveClassData(ClassData classData)
// 클래스 데이터 제거

void ClearSelection()
// 선택 초기화
```

#### 상태 확인
```csharp
bool HasSelectedClasses()
// 선택된 클래스가 있는지 확인

bool IsSelectionComplete()
// 선택이 완료되었는지 확인 (3개 선택)

int GetSelectedCount()
// 선택된 클래스 개수 반환
```

#### 특정 순서 클래스
```csharp
ClassData GetClassAt(int index)
// 특정 인덱스의 클래스 반환

ClassData GetFirstClass()
// 첫 번째 클래스 반환

ClassData GetSecondClass()
// 두 번째 클래스 반환

ClassData GetThirdClass()
// 세 번째 클래스 반환
```

#### 정보 가져오기
```csharp
string[] GetSelectedClassNames()
// 선택된 클래스 이름들 반환

string[] GetSelectedClassDescriptions()
// 선택된 클래스 설명들 반환

string GetSelectionSummary()
// 선택 요약 정보 반환

bool IsClassSelected(ClassData classData)
// 특정 클래스가 선택되었는지 확인

bool IsClassSelected(string className)
// 특정 클래스 이름이 선택되었는지 확인
```

---

## MultiClassSelectorUI

다중 클래스 UI 설정을 도와주는 헬퍼 스크립트입니다.

### 주요 속성

| 속성 | 타입 | 설명 |
|------|------|------|
| `classSelectorCanvas` | Canvas | 클래스 선택 UI가 표시될 Canvas |
| `classButtonContainer` | Transform | 클래스 버튼들이 배치될 부모 오브젝트 |
| `classButtonPrefab` | GameObject | 클래스 버튼 프리팹 |
| `selectedClassesContainer` | Transform | 선택된 클래스 슬롯들이 배치될 부모 오브젝트 |
| `selectedClassSlotPrefab` | GameObject | 선택된 클래스 슬롯 프리팹 |
| `useManagerClasses` | bool | MultiClassManager에서 클래스 자동 로드 여부 |

### 주요 메서드

#### UI 관리
```csharp
void ShowClassSelector()
// 클래스 선택 UI 표시

void HideClassSelector()
// 클래스 선택 UI 숨기기

void RefreshClasses()
// 클래스 목록 새로고침
```

#### 정보 가져오기
```csharp
List<ClassData> GetCurrentSelectedClasses()
// 현재 선택된 클래스들 반환

int GetSelectedCount()
// 선택된 클래스 개수 반환

bool IsSelectionComplete()
// 선택이 완료되었는지 확인

string GetSelectionSummary()
// 선택 요약 정보 반환

bool IsClassSelected(ClassData classData)
// 특정 클래스가 선택되었는지 확인

bool IsClassSelected(string className)
// 특정 클래스 이름이 선택되었는지 확인

int GetAvailableClassCount()
// 사용 가능한 클래스 개수 반환

bool IsClassAvailable(ClassData classData)
// 특정 클래스가 사용 가능한지 확인
```

#### 클래스 데이터 관리
```csharp
void AddClassData(ClassData classData)
// 클래스 데이터 추가

void RemoveClassData(ClassData classData)
// 클래스 데이터 제거

void ClearAllSelections()
// 모든 선택 초기화
```

---

## CharacterSpawner

2D 캐릭터 스폰 및 배치 시스템입니다.

### 주요 속성

| 속성 | 타입 | 설명 |
|------|------|------|
| `spawnCenter` | Transform | 스폰 중심점 |
| `spawnRadius` | float | 스폰 반경 |
| `spawnInCircle` | bool | 원형 배치 여부 |
| `spawnInLine` | bool | 일직선 배치 여부 |
| `spawnInGrid` | bool | 그리드 배치 여부 |
| `gridColumns` | int | 그리드 열 수 |
| `defaultCharacterPrefab2D` | GameObject | 기본 2D 캐릭터 프리팹 |
| `characterSprites` | Sprite[] | 클래스별 스프라이트 배열 |
| `characterColors` | Color[] | 클래스별 색상 배열 |

### 주요 메서드

#### 캐릭터 스폰
```csharp
void SpawnCharacters()
// 캐릭터들 스폰

void RefreshCharacters()
// 캐릭터들 새로고침

void ClearSpawnedCharacters()
// 스폰된 캐릭터들 제거
```

#### 정보 가져오기
```csharp
List<GameObject> GetSpawnedCharacters()
// 스폰된 캐릭터들 반환

GameObject GetCharacterByIndex(int index)
// 특정 인덱스의 캐릭터 반환

int GetSpawnedCharacterCount()
// 스폰된 캐릭터 개수 반환
```

#### 설정 변경
```csharp
void SetSpawnCenter(Transform newCenter)
// 스폰 중심점 설정

void SetSpawnRadius(float newRadius)
// 스폰 반경 설정
```

---

## CharacterInfo

2D 캐릭터의 정보와 상태를 관리하는 컴포넌트입니다.

### 주요 속성

| 속성 | 타입 | 설명 |
|------|------|------|
| `classData` | ClassData | 캐릭터의 클래스 데이터 |
| `characterIndex` | int | 캐릭터 인덱스 |
| `characterName` | string | 캐릭터 이름 |
| `isSelected` | bool | 선택 상태 |
| `isMoving` | bool | 이동 상태 |
| `isAttacking` | bool | 공격 상태 |
| `currentHealth` | int | 현재 체력 |
| `maxHealth` | int | 최대 체력 |
| `attack` | int | 공격력 |
| `defense` | int | 방어력 |
| `speed` | int | 속도 |

### 주요 메서드

#### 캐릭터 설정
```csharp
void SetClassData(ClassData data)
// 클래스 데이터 설정

void SetCharacterIndex(int index)
// 캐릭터 인덱스 설정
```

#### 선택 관리
```csharp
void SelectCharacter()
// 캐릭터 선택

void DeselectCharacter()
// 캐릭터 선택 해제
```

#### 체력 관리
```csharp
void TakeDamage(int damage)
// 데미지 받기

void Heal(int amount)
// 체력 회복

void Die()
// 죽음 처리
```

#### 전투
```csharp
void Attack(CharacterInfo target)
// 다른 캐릭터 공격
```

#### 2D 이동
```csharp
void MoveTo(Vector2 targetPosition)
// 2D 좌표로 이동

void MoveTo(Vector3 targetPosition)
// 3D 좌표로 이동 (Z축 무시)

void MoveWithPhysics(Vector2 direction)
// 물리 기반 이동
```

#### 2D 애니메이션
```csharp
void PlayWalkAnimation()
// 걷기 애니메이션 재생

void PlayIdleAnimation()
// 대기 애니메이션 재생
```

#### 특수 능력
```csharp
bool CanUseSpecialAbility()
// 특수 능력 사용 가능 여부 확인

string GetSpecialAbilityInfo()
// 특수 능력 정보 반환

void UseSpecialAbility()
// 특수 능력 사용
```

#### 정보 가져오기
```csharp
ClassData GetClassData()
// 클래스 데이터 반환

string GetCharacterName()
// 캐릭터 이름 반환

int GetCharacterIndex()
// 캐릭터 인덱스 반환

bool IsAlive()
// 생존 여부 확인

float GetHealthPercentage()
// 체력 비율 반환

string GetClassSummary()
// 클래스 요약 정보 반환

Color GetClassPrimaryColor()
// 클래스 기본 색상 반환

Color GetClassSecondaryColor()
// 클래스 보조 색상 반환

Color GetClassAccentColor()
// 클래스 강조 색상 반환
```

---

## CharacterUI2D

2D 캐릭터용 UI 시스템입니다.

### 주요 속성

| 속성 | 타입 | 설명 |
|------|------|------|
| `nameTagPrefab` | GameObject | 이름 태그 프리팹 |
| `healthBarPrefab` | GameObject | 체력바 프리팹 |
| `uiParent` | Transform | UI 부모 오브젝트 |
| `uiOffsetY` | float | 캐릭터 위에 UI 표시할 오프셋 |
| `uiScale` | float | UI 스케일 |

### 주요 메서드

#### UI 관리
```csharp
void UpdateName(string newName)
// 이름 업데이트

void SetHealthBarVisible(bool visible)
// 체력바 표시/숨기기

void SetNameTagVisible(bool visible)
// 이름 태그 표시/숨기기

void SetUIScale(float scale)
// UI 스케일 설정
```

#### 색상 관리
```csharp
void SetNameTagColor(Color color)
// 이름 태그 색상 설정

void SetHealthBarColor(Color color)
// 체력바 색상 설정
```

---

## SceneTransitionManager

씬 전환 시 데이터를 유지하는 매니저입니다.

### 주요 속성

| 속성 | 타입 | 설명 |
|------|------|------|
| `classSelectionSceneName` | string | 클래스 선택 씬 이름 |
| `gameSceneName` | string | 게임 씬 이름 |
| `characterSpawnSceneName` | string | 캐릭터 스폰 씬 이름 |
| `preserveClassData` | bool | 클래스 데이터 유지 여부 |
| `preserveCharacterData` | bool | 캐릭터 데이터 유지 여부 |

### 주요 메서드

#### 씬 전환
```csharp
void GoToClassSelection()
// 클래스 선택 씬으로 이동

void GoToGameScene()
// 게임 씬으로 이동

void GoToCharacterSpawnScene()
// 캐릭터 스폰 씬으로 이동

void SafeTransitionToGame()
// 안전한 게임 씬 전환 (데이터 검증 포함)
```

#### 데이터 관리
```csharp
bool HasSelectedClasses()
// 선택된 클래스가 있는지 확인

List<ClassData> GetSelectedClasses()
// 선택된 클래스들 반환

int GetSelectedClassCount()
// 선택된 클래스 개수 반환

bool IsSelectionComplete()
// 선택이 완료되었는지 확인

void ClearAllData()
// 모든 데이터 초기화
```

#### 검증
```csharp
bool ValidateDataBeforeTransition()
// 씬 전환 전 데이터 검증
```

#### 정보 가져오기
```csharp
string GetCurrentSceneName()
// 현재 씬 이름 반환

bool IsSceneLoaded(string sceneName)
// 특정 씬이 로드되었는지 확인
```

---

## GameSceneController

GameScene에서 캐릭터 스폰과 게임 로직을 관리하는 컨트롤러입니다.

### 주요 속성

| 속성 | 타입 | 설명 |
|------|------|------|
| `classSelectionSceneName` | string | 클래스 선택 씬 이름 |
| `autoSpawnCharacters` | bool | 자동 캐릭터 스폰 여부 |
| `showDebugInfo` | bool | 디버그 정보 표시 여부 |
| `backToSelectionButton` | GameObject | 클래스 선택으로 돌아가기 버튼 |
| `characterInfoPanel` | GameObject | 캐릭터 정보 패널 |
| `characterCountText` | Text | 캐릭터 개수 텍스트 |

### 주요 메서드

#### 캐릭터 관리
```csharp
void SpawnCharacters()
// 캐릭터들 스폰

void RefreshCharacters()
// 캐릭터 새로고침

void ClearCharacters()
// 캐릭터 제거

List<GameObject> GetSpawnedCharacters()
// 스폰된 캐릭터들 반환

int GetCharacterCount()
// 캐릭터 개수 반환

CharacterInfo GetCharacterByIndex(int index)
// 특정 인덱스의 캐릭터 반환
```

#### 씬 관리
```csharp
void GoBackToClassSelection()
// 클래스 선택 씬으로 돌아가기
```

#### UI 관리
```csharp
void UpdateCharacterCountText()
// 캐릭터 개수 텍스트 업데이트
```

#### 디버그 기능
```csharp
void ShowCharacterInfo()
// 캐릭터 정보 출력 (Context Menu)

void TestCharacterMovement()
// 캐릭터 이동 테스트 (Context Menu)
```

---

## 🎯 사용 예제

### 기본 사용법
```csharp
// 클래스 선택 완료 후 게임 씬으로 이동
SceneTransitionManager.Instance.SafeTransitionToGame();

// 선택된 클래스들 가져오기
List<ClassData> selectedClasses = MultiClassManager.Instance.GetSelectedClasses();

// 캐릭터 스폰 (자동)
CharacterSpawner spawner = FindObjectOfType<CharacterSpawner>();
List<GameObject> characters = spawner.GetSpawnedCharacters();
```

### 이벤트 처리
```csharp
// 클래스 선택 이벤트 연결
MultiClassSelector selector = FindObjectOfType<MultiClassSelector>();
selector.OnClassesSelected += OnClassesSelected;

private void OnClassesSelected(List<ClassData> selectedClasses)
{
    Debug.Log($"클래스들 선택됨: {selectedClasses.Count}개");
}
```

### 캐릭터 조작
```csharp
// 캐릭터 정보 가져오기
CharacterInfo charInfo = character.GetComponent<CharacterInfo>();

// 캐릭터 이동
charInfo.MoveTo(new Vector2(5, 3));

// 캐릭터 공격
charInfo.Attack(targetCharacter.GetComponent<CharacterInfo>());
```

---

**이 API 레퍼런스를 참조하여 모든 기능을 활용하세요!**
