using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 3개 클래스 선택 UI를 쉽게 설정할 수 있도록 도와주는 헬퍼 스크립트
/// Unity Inspector에서 UI 요소들을 쉽게 연결할 수 있습니다.
/// </summary>
public class MultiClassSelectorUI : MonoBehaviour
{
    [Header("UI Canvas 설정")]
    [Tooltip("클래스 선택 UI가 표시될 Canvas")]
    public Canvas classSelectorCanvas;

    [Header("클래스 버튼 영역")]
    [Tooltip("클래스 버튼들이 배치될 부모 오브젝트")]
    public Transform classButtonContainer;
    
    [Tooltip("클래스 버튼 프리팹")]
    public GameObject classButtonPrefab;

    [Header("선택된 클래스 표시 영역")]
    [Tooltip("선택된 클래스 슬롯들이 배치될 부모 오브젝트")]
    public Transform selectedClassesContainer;
    
    [Tooltip("선택된 클래스 슬롯 프리팹")]
    public GameObject selectedClassSlotPrefab;

    [Header("버튼들")]
    [Tooltip("확인 버튼")]
    public Button confirmButton;
    
    [Tooltip("취소 버튼")]
    public Button cancelButton;
    
    [Tooltip("모두 지우기 버튼")]
    public Button clearAllButton;

    [Header("스타일 설정")]
    [Tooltip("선택된 버튼의 색상")]
    public Color selectedButtonColor = Color.green;
    
    [Tooltip("기본 버튼 색상")]
    public Color defaultButtonColor = Color.white;
    
    [Tooltip("비활성화된 버튼 색상")]
    public Color disabledButtonColor = Color.gray;

    [Header("선택 설정")]
    [Tooltip("최대 선택 가능한 클래스 수")]
    public int maxSelections = 3;

    private MultiClassSelector classSelector;

    private void Start()
    {
        SetupClassSelector();
    }

    private void SetupClassSelector()
    {
        // MultiClassSelector 컴포넌트 가져오기 또는 추가
        classSelector = GetComponent<MultiClassSelector>();
        if (classSelector == null)
        {
            classSelector = gameObject.AddComponent<MultiClassSelector>();
        }

        // UI 참조 설정
        classSelector.classButtonContainer = classButtonContainer;
        classSelector.classButtonPrefab = classButtonPrefab;
        classSelector.selectedClassesContainer = selectedClassesContainer;
        classSelector.selectedClassSlotPrefab = selectedClassSlotPrefab;
        classSelector.confirmButton = confirmButton;
        classSelector.cancelButton = cancelButton;
        classSelector.clearAllButton = clearAllButton;

        // 스타일 설정
        classSelector.selectedButtonColor = selectedButtonColor;
        classSelector.defaultButtonColor = defaultButtonColor;
        classSelector.disabledButtonColor = disabledButtonColor;
        classSelector.maxSelections = maxSelections;

        // MultiClassManager가 없으면 생성
        if (MultiClassManager.Instance == null)
        {
            GameObject managerObj = new GameObject("MultiClassManager");
            managerObj.AddComponent<MultiClassManager>();
        }

        // 이벤트 연결
        classSelector.OnClassesSelected += OnClassesSelected;
    }

    private void OnClassesSelected(List<ClassData> selectedClasses)
    {
        Debug.Log($"UI에서 클래스들 선택됨: {selectedClasses.Count}개");
        
        for (int i = 0; i < selectedClasses.Count; i++)
        {
            Debug.Log($"{i + 1}. {selectedClasses[i].className}");
        }
        
        // 여기에 추가적인 UI 업데이트나 애니메이션을 추가할 수 있습니다
        if (classSelectorCanvas != null)
        {
            // 선택 완료 후 UI 숨기기 (선택사항)
            // classSelectorCanvas.gameObject.SetActive(false);
        }
    }

    // 외부에서 UI를 표시/숨기는 메서드들
    public void ShowClassSelector()
    {
        if (classSelectorCanvas != null)
            classSelectorCanvas.gameObject.SetActive(true);
    }

    public void HideClassSelector()
    {
        if (classSelectorCanvas != null)
            classSelectorCanvas.gameObject.SetActive(false);
    }

    // 현재 선택된 클래스들 정보 가져오기
    public List<ClassData> GetCurrentSelectedClasses()
    {
        return MultiClassManager.Instance?.GetSelectedClasses();
    }

    public int GetSelectedCount()
    {
        return MultiClassManager.Instance?.GetSelectedCount() ?? 0;
    }

    public bool IsSelectionComplete()
    {
        return MultiClassManager.Instance?.IsSelectionComplete() ?? false;
    }

    // 선택된 클래스들의 요약 정보 가져오기
    public string GetSelectionSummary()
    {
        return MultiClassManager.Instance?.GetSelectionSummary() ?? "선택된 클래스가 없습니다.";
    }

    // 특정 클래스가 선택되었는지 확인
    public bool IsClassSelected(ClassData classData)
    {
        return MultiClassManager.Instance?.IsClassSelected(classData) ?? false;
    }

    public bool IsClassSelected(string className)
    {
        return MultiClassManager.Instance?.IsClassSelected(className) ?? false;
    }

    // 선택 초기화
    public void ClearAllSelections()
    {
        if (classSelector != null)
        {
            classSelector.ClearAllSelections();
        }
    }

    // 런타임에 클래스 데이터 추가/제거
    public void AddClassData(ClassData classData)
    {
        if (classSelector != null)
        {
            classSelector.AddClassData(classData);
        }
    }

    public void RemoveClassData(ClassData classData)
    {
        if (classSelector != null)
        {
            classSelector.RemoveClassData(classData);
        }
    }

    private void OnDestroy()
    {
        if (classSelector != null)
        {
            classSelector.OnClassesSelected -= OnClassesSelected;
        }
    }
}
