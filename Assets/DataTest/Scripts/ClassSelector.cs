using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ClassSelector : MonoBehaviour
{
    [Header("UI References")]
    public Transform classButtonContainer;
    public GameObject classButtonPrefab;
    public Text selectedClassText;
    public Text classDescriptionText;
    public Button confirmButton;
    public Button cancelButton;

    [Header("Class Data")]
    public List<ClassData> availableClasses = new List<ClassData>();

    private ClassData selectedClass;
    private List<GameObject> classButtons = new List<GameObject>();

    private void Start()
    {
        InitializeUI();
        CreateClassButtons();
        SetupButtonEvents();
    }

    private void InitializeUI()
    {
        if (selectedClassText != null)
            selectedClassText.text = "클래스를 선택해주세요";
        
        if (classDescriptionText != null)
            classDescriptionText.text = "";

        if (confirmButton != null)
            confirmButton.interactable = false;
    }

    private void CreateClassButtons()
    {
        if (classButtonContainer == null || classButtonPrefab == null)
        {
            Debug.LogError("ClassButtonContainer 또는 ClassButtonPrefab이 설정되지 않았습니다!");
            return;
        }

        // 기존 버튼들 제거
        foreach (GameObject button in classButtons)
        {
            if (button != null)
                DestroyImmediate(button);
        }
        classButtons.Clear();

        // 각 클래스에 대한 버튼 생성
        foreach (ClassData classData in availableClasses)
        {
            GameObject buttonObj = Instantiate(classButtonPrefab, classButtonContainer);
            classButtons.Add(buttonObj);

            // 버튼 텍스트 설정
            Text buttonText = buttonObj.GetComponentInChildren<Text>();
            if (buttonText != null)
                buttonText.text = classData.className;

            // 버튼 클릭 이벤트 설정
            Button button = buttonObj.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => SelectClass(classData));
            }
        }
    }

    private void SetupButtonEvents()
    {
        if (confirmButton != null)
            confirmButton.onClick.AddListener(ConfirmSelection);

        if (cancelButton != null)
            cancelButton.onClick.AddListener(CancelSelection);
    }

    public void SelectClass(ClassData classData)
    {
        selectedClass = classData;

        // UI 업데이트
        if (selectedClassText != null)
            selectedClassText.text = $"선택된 클래스: {classData.className}";

        if (classDescriptionText != null)
            classDescriptionText.text = classData.classDescription;

        if (confirmButton != null)
            confirmButton.interactable = true;

        // 선택된 버튼 하이라이트
        UpdateButtonHighlights(classData);
    }

    private void UpdateButtonHighlights(ClassData selectedClassData)
    {
        for (int i = 0; i < availableClasses.Count && i < classButtons.Count; i++)
        {
            if (classButtons[i] != null)
            {
                Image buttonImage = classButtons[i].GetComponent<Image>();
                if (buttonImage != null)
                {
                    // 선택된 버튼은 다른 색으로 표시
                    buttonImage.color = (availableClasses[i] == selectedClassData) ? 
                        Color.yellow : Color.white;
                }
            }
        }
    }

    public void ConfirmSelection()
    {
        if (selectedClass != null)
        {
            Debug.Log($"클래스 선택 완료: {selectedClass.className}");
            
            // ClassManager에 선택된 클래스 전달
            ClassManager.Instance?.SetSelectedClass(selectedClass);
            
            // 선택 완료 이벤트 발생
            OnClassSelected?.Invoke(selectedClass);
        }
    }

    public void CancelSelection()
    {
        selectedClass = null;
        InitializeUI();
        UpdateButtonHighlights(null);
        Debug.Log("클래스 선택이 취소되었습니다.");
    }

    // 외부에서 사용할 수 있는 이벤트
    public System.Action<ClassData> OnClassSelected;

    // 런타임에 클래스 데이터 추가
    public void AddClassData(ClassData classData)
    {
        if (!availableClasses.Contains(classData))
        {
            availableClasses.Add(classData);
            CreateClassButtons();
        }
    }

    // 런타임에 클래스 데이터 제거
    public void RemoveClassData(ClassData classData)
    {
        if (availableClasses.Contains(classData))
        {
            availableClasses.Remove(classData);
            CreateClassButtons();
        }
    }
}
