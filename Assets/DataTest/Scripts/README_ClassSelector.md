# 클래스 선택창 설정 가이드

ClassData를 이용한 클래스 선택 시스템을 Unity에서 설정하는 방법입니다.

## 생성된 스크립트들

### 단일 클래스 선택 (기존)
1. **ClassSelector.cs** - 클래스 선택 UI의 핵심 로직
2. **ClassManager.cs** - 클래스 데이터를 관리하는 싱글톤 매니저
3. **ClassSelectorUI.cs** - UI 설정을 도와주는 헬퍼 스크립트

### 다중 클래스 선택 (새로운 기능)
1. **MultiClassSelector.cs** - 3개 클래스 선택 UI의 핵심 로직
2. **MultiClassManager.cs** - 다중 클래스 데이터를 관리하는 싱글톤 매니저
3. **MultiClassSelectorUI.cs** - 다중 클래스 UI 설정을 도와주는 헬퍼 스크립트

## Unity에서 설정하는 방법

### 1. ClassData ScriptableObject 생성

1. Project 창에서 우클릭
2. Create → Scriptable Objects → ClassData 선택
3. 클래스 이름과 설명 입력
4. 여러 개의 ClassData를 생성하여 다양한 클래스 만들기

### 2. UI Canvas 설정

1. Hierarchy에서 우클릭 → UI → Canvas 생성
2. Canvas 이름을 "ClassSelectorCanvas"로 변경

### 3. UI 요소들 생성

Canvas 하위에 다음 UI 요소들을 생성하세요:

#### 3.1 클래스 버튼 컨테이너
- Empty GameObject 생성 → 이름: "ClassButtonContainer"
- RectTransform 설정:
  - Width: 400, Height: 300
  - Vertical Layout Group 컴포넌트 추가
  - Content Size Fitter 컴포넌트 추가 (Vertical Fit: Preferred Size)

#### 3.2 클래스 버튼 프리팹
- UI → Button 생성
- Button 이름을 "ClassButtonPrefab"으로 변경
- Button 하위의 Text 컴포넌트 확인
- 이 오브젝트를 Project 창으로 드래그하여 프리팹 생성
- Hierarchy에서 원본 삭제

#### 3.3 정보 표시 텍스트들
- UI → Text 생성 → 이름: "SelectedClassText"
- UI → Text 생성 → 이름: "ClassDescriptionText"
- 각각 적절한 위치에 배치

#### 3.4 버튼들
- UI → Button 생성 → 이름: "ConfirmButton"
- UI → Button 생성 → 이름: "CancelButton"
- 각 버튼의 Text를 "확인", "취소"로 설정

### 4. 스크립트 연결

1. Canvas에 **ClassSelectorUI** 스크립트 추가
2. Inspector에서 다음 요소들을 연결:
   - Class Button Container: ClassButtonContainer 오브젝트
   - Class Button Prefab: 생성한 버튼 프리팹
   - Selected Class Text: SelectedClassText 오브젝트
   - Class Description Text: ClassDescriptionText 오브젝트
   - Confirm Button: ConfirmButton 오브젝트
   - Cancel Button: CancelButton 오브젝트

### 5. ClassData 설정

1. 생성한 ClassData들을 Resources/ClassData 폴더에 저장
2. ClassSelectorUI의 Inspector에서 Available Classes에 ClassData들을 추가

### 6. ClassManager 설정

1. 빈 GameObject 생성 → 이름: "ClassManager"
2. **ClassManager** 스크립트 추가
3. Inspector에서 All Class Data에 ClassData들을 추가

## 사용 방법

### 코드에서 사용하기

```csharp
// 선택된 클래스 가져오기
ClassData selectedClass = ClassManager.Instance.GetSelectedClass();

// 클래스 이름 가져오기
string className = ClassManager.Instance.GetSelectedClassName();

// 클래스 설명 가져오기
string description = ClassManager.Instance.GetSelectedClassDescription();

// UI 표시/숨기기
ClassSelectorUI ui = FindObjectOfType<ClassSelectorUI>();
ui.ShowClassSelector();
ui.HideClassSelector();
```

### 이벤트 처리

```csharp
public class GameController : MonoBehaviour
{
    private void Start()
    {
        ClassSelector selector = FindObjectOfType<ClassSelector>();
        if (selector != null)
        {
            selector.OnClassSelected += OnClassSelected;
        }
    }

    private void OnClassSelected(ClassData selectedClass)
    {
        Debug.Log($"게임에서 클래스 선택됨: {selectedClass.className}");
        // 게임 로직 처리
    }
}
```

## 다중 클래스 선택 시스템 (3개 클래스)

### 새로운 기능들

- ✅ **3개 클래스까지 선택 가능**
- ✅ **선택된 클래스들을 순서대로 표시**
- ✅ **이미 선택된 클래스 재선택 시 취소**
- ✅ **3개 클래스 모두 선택해야만 확인 버튼 활성화**
- ✅ **모두 지우기 버튼으로 선택 초기화**
- ✅ **선택된 클래스 개수에 따른 버튼 상태 관리**

### Unity에서 다중 클래스 선택 UI 설정

#### 1. UI Canvas 설정
1. Hierarchy에서 우클릭 → UI → Canvas 생성
2. Canvas 이름을 "MultiClassSelectorCanvas"로 변경

#### 2. UI 요소들 생성

##### 2.1 선택된 클래스 표시 영역 (상단)
- Empty GameObject 생성 → 이름: "SelectedClassesContainer"
- RectTransform 설정:
  - Width: 600, Height: 100
  - Horizontal Layout Group 컴포넌트 추가
  - Content Size Fitter 컴포넌트 추가 (Horizontal Fit: Preferred Size)

##### 2.2 선택된 클래스 슬롯 프리팹
- UI → Button 생성
- Button 이름을 "SelectedClassSlotPrefab"으로 변경
- Button 하위의 Text 컴포넌트 확인
- 이 오브젝트를 Project 창으로 드래그하여 프리팹 생성
- Hierarchy에서 원본 삭제

##### 2.3 클래스 버튼 컨테이너
- Empty GameObject 생성 → 이름: "ClassButtonContainer"
- RectTransform 설정:
  - Width: 600, Height: 400
  - Grid Layout Group 컴포넌트 추가 (Cell Size: 150x50, Spacing: 10x10)

##### 2.4 버튼들
- UI → Button 생성 → 이름: "ConfirmButton" (텍스트: "확인")
- UI → Button 생성 → 이름: "CancelButton" (텍스트: "취소")
- UI → Button 생성 → 이름: "ClearAllButton" (텍스트: "모두 지우기")

#### 3. 스크립트 연결
1. Canvas에 **MultiClassSelectorUI** 스크립트 추가
2. Inspector에서 다음 요소들을 연결:
   - Class Button Container: ClassButtonContainer 오브젝트
   - Class Button Prefab: 생성한 버튼 프리팹
   - Selected Classes Container: SelectedClassesContainer 오브젝트
   - Selected Class Slot Prefab: 생성한 슬롯 프리팹
   - Confirm Button: ConfirmButton 오브젝트
   - Cancel Button: CancelButton 오브젝트
   - Clear All Button: ClearAllButton 오브젝트

### 사용 방법

#### 코드에서 사용하기

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

#### 이벤트 처리

```csharp
public class GameController : MonoBehaviour
{
    private void Start()
    {
        MultiClassSelector selector = FindObjectOfType<MultiClassSelector>();
        if (selector != null)
        {
            selector.OnClassesSelected += OnClassesSelected;
        }
    }

    private void OnClassesSelected(List<ClassData> selectedClasses)
    {
        Debug.Log($"게임에서 클래스들 선택됨: {selectedClasses.Count}개");
        for (int i = 0; i < selectedClasses.Count; i++)
        {
            Debug.Log($"{i + 1}. {selectedClasses[i].className}");
        }
        // 게임 로직 처리
    }
}
```

## 추가 기능

- 런타임에 클래스 데이터 추가/제거 가능
- 버튼 하이라이트 기능
- 선택 취소 기능
- 싱글톤 패턴으로 어디서든 접근 가능
- **다중 클래스 선택 및 순서 관리**
- **선택 검증 시스템**

## 주의사항

- ClassData는 Resources/ClassData 폴더에 저장해야 자동 로드됩니다
- UI 요소들은 반드시 Inspector에서 연결해야 합니다
- ClassManager/MultiClassManager는 씬에 하나만 있어야 합니다
- **3개 클래스를 모두 선택해야만 확인 버튼이 활성화됩니다**
