using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 클래스 선택 UI를 쉽게 설정할 수 있도록 도와주는 헬퍼 스크립트
/// Unity Inspector에서 UI 요소들을 쉽게 연결할 수 있습니다.
/// </summary>
public class ClassSelectorUI : MonoBehaviour
{
    [Header("UI Canvas 설정")]
    [Tooltip("클래스 선택 UI가 표시될 Canvas")]
    public Canvas classSelectorCanvas;

    [Header("UI 요소들")]
    [Tooltip("클래스 버튼들이 배치될 부모 오브젝트")]
    public Transform classButtonContainer;
    
    [Tooltip("클래스 버튼 프리팹")]
    public GameObject classButtonPrefab;
    
    [Tooltip("선택된 클래스 이름을 표시할 텍스트")]
    public Text selectedClassText;
    
    [Tooltip("클래스 설명을 표시할 텍스트")]
    public Text classDescriptionText;
    
    [Tooltip("확인 버튼")]
    public Button confirmButton;
    
    [Tooltip("취소 버튼")]
    public Button cancelButton;

    [Header("스타일 설정")]
    [Tooltip("선택된 버튼의 색상")]
    public Color selectedButtonColor = Color.yellow;
    
    [Tooltip("기본 버튼 색상")]
    public Color defaultButtonColor = Color.white;

    private ClassSelector classSelector;

    private void Start()
    {
        SetupClassSelector();
    }

    private void SetupClassSelector()
    {
        // ClassSelector 컴포넌트 가져오기 또는 추가
        classSelector = GetComponent<ClassSelector>();
        if (classSelector == null)
        {
            classSelector = gameObject.AddComponent<ClassSelector>();
        }

        // UI 참조 설정
        classSelector.classButtonContainer = classButtonContainer;
        classSelector.classButtonPrefab = classButtonPrefab;
        classSelector.selectedClassText = selectedClassText;
        classSelector.classDescriptionText = classDescriptionText;
        classSelector.confirmButton = confirmButton;
        classSelector.cancelButton = cancelButton;

        // ClassManager가 없으면 생성
        if (ClassManager.Instance == null)
        {
            GameObject managerObj = new GameObject("ClassManager");
            managerObj.AddComponent<ClassManager>();
        }

        // 이벤트 연결
        classSelector.OnClassSelected += OnClassSelected;
    }

    private void OnClassSelected(ClassData selectedClass)
    {
        Debug.Log($"UI에서 클래스 선택됨: {selectedClass.className}");
        
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

    // 현재 선택된 클래스 정보 가져오기
    public ClassData GetCurrentSelectedClass()
    {
        return ClassManager.Instance?.GetSelectedClass();
    }

    private void OnDestroy()
    {
        if (classSelector != null)
        {
            classSelector.OnClassSelected -= OnClassSelected;
        }
    }
}
