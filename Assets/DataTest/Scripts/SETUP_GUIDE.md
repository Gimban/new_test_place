# 설정 가이드 (Setup Guide)

Unity에서 클래스 선택 시스템을 설정하는 상세한 방법입니다.

## 📋 목차

1. [ClassData ScriptableObject 생성](#1-classdata-scriptableobject-생성)
2. [다중 클래스 선택 UI 설정](#2-다중-클래스-선택-ui-설정)
3. [2D 캐릭터 스폰 시스템 설정](#3-2d-캐릭터-스폰-시스템-설정)
4. [씬 전환 설정](#4-씬-전환-설정)
5. [2D 캐릭터 프리팹 생성](#5-2d-캐릭터-프리팹-생성)
6. [2D UI 프리팹 생성](#6-2d-ui-프리팹-생성)

---

## 1. ClassData ScriptableObject 생성

### 1.1 ClassData 생성
1. Project 창에서 우클릭
2. `Create → Scriptable Objects → ClassData` 선택
3. 파일명을 적절히 설정 (예: `WarriorClass`, `MageClass`)

### 1.2 ClassData 설정
```
클래스 기본 정보:
- Character Name: 클래스 이름
- Character Description: 클래스 설명
- Class Icon: 클래스 아이콘 (선택사항)

2D 캐릭터 설정:
- Character Sprite: 2D 캐릭터 스프라이트
- Character Prefab 2D: 2D 캐릭터 프리팹

클래스 스탯:
- Health: 체력 (기본값: 100)
- Attack: 공격력 (기본값: 10)
- Defense: 방어력 (기본값: 5)
- Speed: 속도 (기본값: 8)
- Mana: 마나 (기본값: 50)

2D 스폰 설정:
- Spawn Offset: 스폰 오프셋
- Spawn Radius: 스폰 반경
- Can Move: 이동 가능 여부
- Can Attack: 공격 가능 여부

2D 애니메이션 설정:
- Animator Controller 2D: 2D 애니메이션 컨트롤러
- Idle Animation Name: 대기 애니메이션 이름
- Walk Animation Name: 걷기 애니메이션 이름
- Attack Animation Name: 공격 애니메이션 이름
- Death Animation Name: 죽음 애니메이션 이름

2D 스프라이트 설정:
- Idle Sprites: 대기 애니메이션 스프라이트 배열
- Walk Sprites: 걷기 애니메이션 스프라이트 배열
- Attack Sprites: 공격 애니메이션 스프라이트 배열
- Animation Speed: 스프라이트 애니메이션 속도

2D 물리 설정:
- Collider Radius: 콜라이더 반경
- Use Rigidbody2D: Rigidbody2D 사용 여부
- Mass: 질량
- Drag: 선형 드래그
- Angular Drag: 각속도 드래그

클래스 특수 능력:
- Has Special Ability: 특수 능력 보유 여부
- Special Ability Name: 특수 능력 이름
- Special Ability Description: 특수 능력 설명
- Special Ability Cooldown: 특수 능력 쿨다운

클래스 색상 테마:
- Primary Color: 기본 색상
- Secondary Color: 보조 색상
- Accent Color: 강조 색상
```

### 1.3 Resources 폴더에 저장
1. `Assets/Resources/ClassData/` 폴더 생성
2. 생성한 ClassData 파일들을 이 폴더에 저장
3. MultiClassManager가 자동으로 로드합니다

---

## 2. 다중 클래스 선택 UI 설정

### 2.1 UI Canvas 설정
1. Hierarchy에서 우클릭 → `UI → Canvas` 생성
2. Canvas 이름을 `"MultiClassSelectorCanvas"`로 변경

### 2.2 UI 요소들 생성

#### 2.2.1 선택된 클래스 표시 영역 (상단)
```
1. Empty GameObject 생성 → 이름: "SelectedClassesContainer"
2. RectTransform 설정:
   - Width: 600, Height: 100
   - Horizontal Layout Group 컴포넌트 추가
   - Content Size Fitter 컴포넌트 추가 (Horizontal Fit: Preferred Size)
```

#### 2.2.2 선택된 클래스 슬롯 프리팹
```
1. UI → Button 생성
2. Button 이름을 "SelectedClassSlotPrefab"으로 변경
3. Button 하위의 Text 컴포넌트 확인
4. 이 오브젝트를 Project 창으로 드래그하여 프리팹 생성
5. Hierarchy에서 원본 삭제
```

#### 2.2.3 클래스 버튼 컨테이너
```
1. Empty GameObject 생성 → 이름: "ClassButtonContainer"
2. RectTransform 설정:
   - Width: 600, Height: 400
   - Grid Layout Group 컴포넌트 추가 (Cell Size: 150x50, Spacing: 10x10)
```

#### 2.2.4 버튼들
```
1. UI → Button 생성 → 이름: "ConfirmButton" (텍스트: "확인")
2. UI → Button 생성 → 이름: "ClearAllButton" (텍스트: "모두 지우기")
```

### 2.3 스크립트 연결
1. Canvas에 **MultiClassSelectorUI** 스크립트 추가
2. Inspector에서 다음 요소들을 연결:
   - Class Button Container: ClassButtonContainer 오브젝트
   - Class Button Prefab: 생성한 버튼 프리팹
   - Selected Classes Container: SelectedClassesContainer 오브젝트
   - Selected Class Slot Prefab: 생성한 슬롯 프리팹
   - Confirm Button: ConfirmButton 오브젝트
   - Clear All Button: ClearAllButton 오브젝트
   - **Use Manager Classes: 체크** (MultiClassManager에서 자동 로드)

---

## 3. 2D 캐릭터 스폰 시스템 설정

### 3.1 CharacterSpawner 설정
1. 빈 GameObject 생성 → 이름: "CharacterSpawner"
2. **CharacterSpawner** 스크립트 추가
3. Inspector에서 2D 설정:
   - Spawn Center: 스폰 중심점 (빈 GameObject)
   - Spawn Radius: 스폰 반경 (기본값: 5)
   - Spawn In Circle: 원형 배치 (기본값: true)
   - Spawn In Line: 일직선 배치
   - Spawn In Grid: 그리드 배치
   - Grid Columns: 그리드 열 수 (기본값: 3)
   - Default Character Prefab 2D: 기본 2D 캐릭터 프리팹
   - Character Sprites: 클래스별 스프라이트 배열
   - Character Colors: 클래스별 색상 배열

### 3.2 SceneTransitionManager 설정
1. 빈 GameObject 생성 → 이름: "SceneTransitionManager"
2. **SceneTransitionManager** 스크립트 추가
3. Inspector에서 씬 이름들 설정:
   - Class Selection Scene Name: "ClassSelection"
   - Game Scene Name: "GameScene"
   - Character Spawn Scene Name: "CharacterSpawn"

---

## 4. GameScene 설정

### 4.1 GameSceneController 설정
1. 빈 GameObject 생성 → 이름: "GameSceneController"
2. **GameSceneController** 스크립트 추가
3. Inspector에서 설정:
   - Class Selection Scene Name: "ClassSelection"
   - Auto Spawn Characters: 체크
   - Show Debug Info: 체크 (선택사항)

### 4.2 GameScene UI 설정 (선택사항)
```
1. UI → Canvas 생성 → 이름: "GameUI"
2. UI → Button 생성 → 이름: "BackButton" (텍스트: "클래스 선택으로 돌아가기")
3. UI → Text 생성 → 이름: "CharacterCountText" (텍스트: "스폰된 캐릭터: 0개")
4. UI → Panel 생성 → 이름: "CharacterInfoPanel" (디버그 정보용)
```

### 4.3 GameScene UI 연결
1. GameSceneController의 Inspector에서 연결:
   - Back To Selection Button: BackButton 오브젝트
   - Character Info Panel: CharacterInfoPanel 오브젝트
   - Character Count Text: CharacterCountText 오브젝트

---

## 5. 씬 전환 설정

### 4.1 씬 생성
1. **ClassSelection 씬**: 클래스 선택 UI
2. **GameScene 씬**: 게임 플레이 (캐릭터 스폰 포함)
3. **CharacterSpawn 씬**: 캐릭터 스폰 전용 (선택사항)

### 4.2 씬 전환 코드
```csharp
// 클래스 선택 완료 후
public void OnClassSelectionComplete()
{
    SceneTransitionManager.Instance.SafeTransitionToGame();
}

// 게임 씬에서 캐릭터 스폰 (자동)
// CharacterSpawner가 자동으로 MultiClassManager에서 데이터를 가져와서 스폰
```

---

## 5. 2D 캐릭터 프리팹 생성

### 5.1 기본 2D 캐릭터 프리팹
1. 2D Object → Sprite 생성
2. **CharacterInfo** 스크립트 추가
3. **CharacterUI2D** 스크립트 추가 (UI용)
4. CircleCollider2D 추가 (마우스 클릭을 위해)
5. Rigidbody2D 추가 (물리 이동용, 선택사항)
6. 프리팹으로 저장

### 5.2 2D 애니메이션 설정
1. 캐릭터에 Animator 컴포넌트 추가
2. Animator Controller 생성
3. Idle, Walk, Attack, Death 애니메이션 설정
4. CharacterData에 Animator Controller 2D 연결

---

## 6. 2D UI 프리팹 생성

### 6.1 이름 태그 프리팹
```
1. UI → Canvas → World Space Canvas 생성
2. UI → Text 추가하여 이름 표시
3. 프리팹으로 저장
```

### 6.2 체력바 프리팹
```
1. UI → Canvas → World Space Canvas 생성
2. UI → Slider 추가하여 체력 표시
3. 프리팹으로 저장
```

---

## ✅ 설정 완료 체크리스트

- [ ] ClassData ScriptableObject 생성 및 Resources/ClassData에 저장
- [ ] MultiClassSelectorUI 설정 및 UI 요소들 연결
- [ ] CharacterSpawner 설정 및 2D 캐릭터 프리팹 연결
- [ ] SceneTransitionManager 설정 및 씬 이름 설정
- [ ] 2D 캐릭터 프리팹 생성 (CharacterInfo, CharacterUI2D 포함)
- [ ] 2D UI 프리팹 생성 (이름 태그, 체력바)
- [ ] Use Manager Classes 체크 (자동 클래스 로드)
- [ ] 씬 전환 코드 추가

---

## 🔧 문제 해결

### 자주 발생하는 문제들

1. **클래스 버튼이 생성되지 않음**
   - Resources/ClassData 폴더에 ClassData 파일이 있는지 확인
   - Use Manager Classes가 체크되어 있는지 확인

2. **캐릭터가 스폰되지 않음**
   - CharacterSpawner의 Default Character Prefab 2D가 설정되어 있는지 확인
   - MultiClassManager에 선택된 클래스가 있는지 확인

3. **UI 요소가 연결되지 않음**
   - Inspector에서 모든 UI 참조가 올바르게 연결되어 있는지 확인
   - 프리팹이 올바르게 생성되었는지 확인

4. **씬 전환이 안됨**
   - SceneTransitionManager의 씬 이름이 올바른지 확인
   - 씬이 Build Settings에 추가되어 있는지 확인

---

**다음 단계**: [사용법 가이드](USAGE_GUIDE.md)를 참조하여 코드 사용법을 학습하세요!
