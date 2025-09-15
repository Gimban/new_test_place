using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class MultiClassSelector : MonoBehaviour
{
    [Header("UI References")]
    public Transform classButtonContainer;
    public GameObject classButtonPrefab;
    public Button confirmButton;
    public Button cancelButton;
    public Button clearAllButton;

    [Header("Selected Classes Display")]
    public Transform selectedClassesContainer;
    public GameObject selectedClassSlotPrefab;
    public Text[] selectedClassTexts = new Text[3]; // 3개의 선택된 클래스 표시용
    public Image[] selectedClassImages = new Image[3]; // 선택된 클래스 이미지 (선택사항)

    [Header("Class Data")]
    public List<ClassData> availableClasses = new List<ClassData>();

    [Header("Selection Settings")]
    public int maxSelections = 3;
    public Color selectedButtonColor = Color.green;
    public Color defaultButtonColor = Color.white;
    public Color disabledButtonColor = Color.gray;

    private List<ClassData> selectedClasses = new List<ClassData>();
    private List<GameObject> classButtons = new List<GameObject>();
    private List<GameObject> selectedClassSlots = new List<GameObject>();

    private void Start()
    {
        InitializeUI();
        CreateClassButtons();
        CreateSelectedClassSlots();
        SetupButtonEvents();
        UpdateUI();
    }

    private void InitializeUI()
    {
        if (confirmButton != null)
            confirmButton.interactable = false;

        if (clearAllButton != null)
            clearAllButton.interactable = false;
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
                button.onClick.AddListener(() => ToggleClassSelection(classData));
            }
        }
    }

    private void CreateSelectedClassSlots()
    {
        if (selectedClassesContainer == null || selectedClassSlotPrefab == null)
        {
            Debug.LogError("SelectedClassesContainer 또는 SelectedClassSlotPrefab이 설정되지 않았습니다!");
            return;
        }

        // 기존 슬롯들 제거
        foreach (GameObject slot in selectedClassSlots)
        {
            if (slot != null)
                DestroyImmediate(slot);
        }
        selectedClassSlots.Clear();

        // 3개의 선택 슬롯 생성
        for (int i = 0; i < maxSelections; i++)
        {
            GameObject slotObj = Instantiate(selectedClassSlotPrefab, selectedClassesContainer);
            selectedClassSlots.Add(slotObj);

            // 슬롯에 순서 번호 표시
            Text slotText = slotObj.GetComponentInChildren<Text>();
            if (slotText != null)
            {
                slotText.text = $"슬롯 {i + 1}";
            }

            // 선택된 클래스 텍스트 배열에 저장
            if (i < selectedClassTexts.Length)
            {
                selectedClassTexts[i] = slotText;
            }
        }
    }

    private void SetupButtonEvents()
    {
        if (confirmButton != null)
            confirmButton.onClick.AddListener(ConfirmSelection);

        if (cancelButton != null)
            cancelButton.onClick.AddListener(CancelSelection);

        if (clearAllButton != null)
            clearAllButton.onClick.AddListener(ClearAllSelections);
    }

    public void ToggleClassSelection(ClassData classData)
    {
        if (selectedClasses.Contains(classData))
        {
            // 이미 선택된 클래스면 선택 취소
            RemoveClassSelection(classData);
        }
        else
        {
            // 새로운 클래스 선택
            AddClassSelection(classData);
        }
    }

    private void AddClassSelection(ClassData classData)
    {
        if (selectedClasses.Count >= maxSelections)
        {
            Debug.Log($"최대 {maxSelections}개의 클래스만 선택할 수 있습니다!");
            return;
        }

        selectedClasses.Add(classData);
        UpdateUI();
        Debug.Log($"클래스 선택됨: {classData.className} (순서: {selectedClasses.Count})");
    }

    private void RemoveClassSelection(ClassData classData)
    {
        if (selectedClasses.Contains(classData))
        {
            int removedIndex = selectedClasses.IndexOf(classData);
            selectedClasses.Remove(classData);
            
            // 선택된 클래스들을 다시 정렬하여 빈 슬롯을 채움
            ReorderSelectedClasses();
            
            UpdateUI();
            Debug.Log($"클래스 선택 취소됨: {classData.className}");
        }
    }

    private void ReorderSelectedClasses()
    {
        // 선택된 클래스들을 앞쪽으로 정렬하여 빈 슬롯을 채움
        List<ClassData> reorderedList = new List<ClassData>();
        
        foreach (ClassData classData in selectedClasses)
        {
            if (classData != null)
            {
                reorderedList.Add(classData);
            }
        }
        
        selectedClasses = reorderedList;
    }

    private void UpdateUI()
    {
        UpdateClassButtons();
        UpdateSelectedClassSlots();
        UpdateConfirmButton();
        UpdateClearAllButton();
    }

    private void UpdateClassButtons()
    {
        for (int i = 0; i < availableClasses.Count && i < classButtons.Count; i++)
        {
            if (classButtons[i] != null)
            {
                Image buttonImage = classButtons[i].GetComponent<Image>();
                Button button = classButtons[i].GetComponent<Button>();
                
                if (buttonImage != null)
                {
                    if (selectedClasses.Contains(availableClasses[i]))
                    {
                        buttonImage.color = selectedButtonColor;
                    }
                    else if (selectedClasses.Count >= maxSelections)
                    {
                        buttonImage.color = disabledButtonColor;
                        if (button != null) button.interactable = false;
                    }
                    else
                    {
                        buttonImage.color = defaultButtonColor;
                        if (button != null) button.interactable = true;
                    }
                }
            }
        }
    }

    private void UpdateSelectedClassSlots()
    {
        for (int i = 0; i < maxSelections; i++)
        {
            if (i < selectedClassTexts.Length && selectedClassTexts[i] != null)
            {
                if (i < selectedClasses.Count && selectedClasses[i] != null)
                {
                    selectedClassTexts[i].text = selectedClasses[i].className;
                    selectedClassTexts[i].color = Color.black;
                }
                else
                {
                    selectedClassTexts[i].text = $"슬롯 {i + 1} (비어있음)";
                    selectedClassTexts[i].color = Color.gray;
                }
            }
        }
    }

    private void UpdateConfirmButton()
    {
        if (confirmButton != null)
        {
            confirmButton.interactable = selectedClasses.Count == maxSelections;
        }
    }

    private void UpdateClearAllButton()
    {
        if (clearAllButton != null)
        {
            clearAllButton.interactable = selectedClasses.Count > 0;
        }
    }

    public void ConfirmSelection()
    {
        if (selectedClasses.Count == maxSelections)
        {
            Debug.Log($"클래스 선택 완료! 선택된 클래스들:");
            for (int i = 0; i < selectedClasses.Count; i++)
            {
                Debug.Log($"{i + 1}. {selectedClasses[i].className}");
            }
            
            // MultiClassManager에 선택된 클래스들 전달
            MultiClassManager.Instance?.SetSelectedClasses(selectedClasses);
            
            // 선택 완료 이벤트 발생
            OnClassesSelected?.Invoke(selectedClasses);
        }
        else
        {
            Debug.Log($"3개의 클래스를 모두 선택해야 합니다! (현재: {selectedClasses.Count}개)");
        }
    }

    public void CancelSelection()
    {
        ClearAllSelections();
        Debug.Log("클래스 선택이 취소되었습니다.");
    }

    public void ClearAllSelections()
    {
        selectedClasses.Clear();
        UpdateUI();
        Debug.Log("모든 클래스 선택이 초기화되었습니다.");
    }

    // 외부에서 사용할 수 있는 이벤트
    public System.Action<List<ClassData>> OnClassesSelected;

    // 현재 선택된 클래스들 가져오기
    public List<ClassData> GetSelectedClasses()
    {
        return new List<ClassData>(selectedClasses);
    }

    // 선택된 클래스 개수 가져오기
    public int GetSelectedCount()
    {
        return selectedClasses.Count;
    }

    // 특정 클래스가 선택되었는지 확인
    public bool IsClassSelected(ClassData classData)
    {
        return selectedClasses.Contains(classData);
    }

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
            // 선택된 클래스에서도 제거
            if (selectedClasses.Contains(classData))
            {
                selectedClasses.Remove(classData);
                ReorderSelectedClasses();
            }
            CreateClassButtons();
            UpdateUI();
        }
    }
}
