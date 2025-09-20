# 클래스 선택 시스템 (Class Selection System)

Unity 2D 게임을 위한 완전한 클래스 선택 및 캐릭터 스폰 시스템입니다.

## 🎯 주요 기능

- ✅ **3개 클래스까지 선택 가능**
- ✅ **선택된 클래스들을 순서대로 표시**
- ✅ **이미 선택된 클래스 재선택 시 취소**
- ✅ **3개 클래스 모두 선택해야만 확인 버튼 활성화**
- ✅ **MultiClassManager에서 클래스 자동 로드**
- ✅ **Resources/ClassData 폴더에서 자동 클래스 감지**
- ✅ **자동 2D 캐릭터 스폰 시스템**
- ✅ **씬 전환 시 데이터 유지**
- ✅ **2D 캐릭터 상태 관리 (체력, 공격, 이동)**
- ✅ **마우스 클릭으로 캐릭터 선택**
- ✅ **2D 물리 시스템 지원 (Rigidbody2D)**
- ✅ **2D 애니메이션 시스템 지원**
- ✅ **2D UI 시스템 (이름 태그, 체력바)**
- ✅ **다양한 2D 배치 옵션 (원형, 일직선, 그리드)**
- ✅ **클래스 특수 능력 시스템**
- ✅ **클래스 색상 테마 시스템**

## 📁 프로젝트 구조

```
Scripts/
├── README.md                           # 메인 문서 (이 파일)
├── SETUP_GUIDE.md                      # 설정 가이드
├── USAGE_GUIDE.md                      # 사용법 가이드
├── API_REFERENCE.md                    # API 레퍼런스
├── ClassData.cs                        # 클래스 데이터 ScriptableObject
├── MultiClassSelector.cs               # 3개 클래스 선택 UI 핵심 로직
├── MultiClassManager.cs                # 다중 클래스 데이터 관리
├── MultiClassSelectorUI.cs             # 다중 클래스 UI 설정 헬퍼
├── CharacterData.cs                    # 2D 캐릭터 데이터 ScriptableObject
├── CharacterSpawner.cs                 # 2D 캐릭터 스폰 및 배치 시스템
├── CharacterInfo.cs                    # 2D 캐릭터 상태 관리
├── CharacterUI2D.cs                    # 2D 캐릭터용 UI 시스템
└── SceneTransitionManager.cs           # 씬 전환 및 데이터 유지
```

## 🚀 빠른 시작

### 1. 기본 설정
1. **ClassData 생성**: `Create → Scriptable Objects → ClassData`
2. **Resources/ClassData 폴더에 저장**
3. **MultiClassSelectorUI 설정**: Inspector에서 `Use Manager Classes` 체크
4. **CharacterSpawner 설정**: 2D 캐릭터 프리팹 연결

### 2. 사용 흐름
```
클래스 선택 씬 → 3개 클래스 선택 → 게임 씬 → 자동 캐릭터 스폰
```

### 3. 간단한 사용법
```csharp
// 클래스 선택 완료 후 게임 씬으로 이동
SceneTransitionManager.Instance.SafeTransitionToGame();

// 선택된 클래스들 가져오기
List<ClassData> selectedClasses = MultiClassManager.Instance.GetSelectedClasses();

// 캐릭터 스폰 (자동)
CharacterSpawner spawner = FindObjectOfType<CharacterSpawner>();
List<GameObject> characters = spawner.GetSpawnedCharacters();
```

## 📚 문서

- **[설정 가이드](SETUP_GUIDE.md)** - 상세한 Unity 설정 방법
- **[사용법 가이드](USAGE_GUIDE.md)** - 코드 사용법 및 예제
- **[API 레퍼런스](API_REFERENCE.md)** - 모든 클래스와 메서드 설명

## 🎮 시스템 개요

### 클래스 선택 시스템
- **MultiClassSelector**: 3개 클래스 선택 UI
- **MultiClassManager**: 클래스 데이터 관리
- **자동 로드**: Resources 폴더에서 ClassData 자동 감지

### 캐릭터 스폰 시스템
- **CharacterSpawner**: 2D 캐릭터 자동 스폰
- **CharacterInfo**: 개별 캐릭터 상태 관리
- **CharacterUI2D**: 2D 캐릭터 UI (이름 태그, 체력바)

### 씬 전환 시스템
- **SceneTransitionManager**: 씬 간 데이터 유지
- **자동 검증**: 3개 클래스 선택 완료 확인

## ⚙️ 요구사항

- Unity 2020.3 이상
- 2D 프로젝트 설정
- Resources 폴더 구조

## 🔧 주요 설정

1. **ClassData**: Resources/ClassData/ 폴더에 저장
2. **UI 연결**: Inspector에서 UI 요소들 연결
3. **자동 로드**: `Use Manager Classes` 체크
4. **2D 설정**: Sprite, Rigidbody2D, Collider2D 사용

## 📝 라이선스

이 프로젝트는 MIT 라이선스 하에 제공됩니다.

## 🤝 기여

버그 리포트나 기능 제안은 이슈로 등록해주세요.

---

**더 자세한 내용은 각 가이드 문서를 참조하세요!**
