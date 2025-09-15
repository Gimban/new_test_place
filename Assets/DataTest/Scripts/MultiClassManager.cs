using UnityEngine;
using System.Collections.Generic;

public class MultiClassManager : MonoBehaviour
{
    public static MultiClassManager Instance { get; private set; }

    [Header("Class Data Settings")]
    public List<ClassData> allClassData = new List<ClassData>();
    
    private List<ClassData> selectedClasses = new List<ClassData>();

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
        // 모든 ClassData 자동 로드 (Resources 폴더에서)
        LoadAllClassData();
    }

    private void LoadAllClassData()
    {
        // Resources 폴더에서 모든 ClassData 로드
        ClassData[] loadedClasses = Resources.LoadAll<ClassData>("ClassData");
        allClassData.AddRange(loadedClasses);
        
        Debug.Log($"로드된 클래스 수: {allClassData.Count}");
    }

    public void SetSelectedClasses(List<ClassData> classes)
    {
        selectedClasses = new List<ClassData>(classes);
        Debug.Log($"선택된 클래스들이 설정되었습니다. 총 {selectedClasses.Count}개");
        
        for (int i = 0; i < selectedClasses.Count; i++)
        {
            Debug.Log($"{i + 1}. {selectedClasses[i].className}");
        }
    }

    public List<ClassData> GetSelectedClasses()
    {
        return new List<ClassData>(selectedClasses);
    }

    public List<ClassData> GetAllClasses()
    {
        return new List<ClassData>(allClassData);
    }

    public ClassData GetClassByName(string className)
    {
        return allClassData.Find(c => c.className == className);
    }

    public bool HasSelectedClasses()
    {
        return selectedClasses.Count > 0;
    }

    public bool IsSelectionComplete()
    {
        return selectedClasses.Count == 3;
    }

    public void ClearSelection()
    {
        selectedClasses.Clear();
        Debug.Log("클래스 선택이 초기화되었습니다.");
    }

    // 선택된 클래스들의 정보를 가져오는 메서드들
    public string[] GetSelectedClassNames()
    {
        string[] names = new string[selectedClasses.Count];
        for (int i = 0; i < selectedClasses.Count; i++)
        {
            names[i] = selectedClasses[i].className;
        }
        return names;
    }

    public string[] GetSelectedClassDescriptions()
    {
        string[] descriptions = new string[selectedClasses.Count];
        for (int i = 0; i < selectedClasses.Count; i++)
        {
            descriptions[i] = selectedClasses[i].classDescription;
        }
        return descriptions;
    }

    // 특정 순서의 클래스 가져오기
    public ClassData GetClassAt(int index)
    {
        if (index >= 0 && index < selectedClasses.Count)
        {
            return selectedClasses[index];
        }
        return null;
    }

    // 첫 번째, 두 번째, 세 번째 클래스 가져오기
    public ClassData GetFirstClass()
    {
        return GetClassAt(0);
    }

    public ClassData GetSecondClass()
    {
        return GetClassAt(1);
    }

    public ClassData GetThirdClass()
    {
        return GetClassAt(2);
    }

    // 선택된 클래스 개수
    public int GetSelectedCount()
    {
        return selectedClasses.Count;
    }

    // 특정 클래스가 선택되었는지 확인
    public bool IsClassSelected(ClassData classData)
    {
        return selectedClasses.Contains(classData);
    }

    // 특정 클래스가 선택되었는지 확인 (이름으로)
    public bool IsClassSelected(string className)
    {
        return selectedClasses.Exists(c => c.className == className);
    }

    // 클래스 데이터 추가/제거 (런타임에서)
    public void AddClassData(ClassData classData)
    {
        if (!allClassData.Contains(classData))
        {
            allClassData.Add(classData);
            Debug.Log($"클래스 데이터가 추가되었습니다: {classData.className}");
        }
    }

    public void RemoveClassData(ClassData classData)
    {
        if (allClassData.Contains(classData))
        {
            allClassData.Remove(classData);
            // 선택된 클래스에서도 제거
            if (selectedClasses.Contains(classData))
            {
                selectedClasses.Remove(classData);
            }
            Debug.Log($"클래스 데이터가 제거되었습니다: {classData.className}");
        }
    }

    // 선택된 클래스들의 정보를 문자열로 반환
    public string GetSelectionSummary()
    {
        if (selectedClasses.Count == 0)
        {
            return "선택된 클래스가 없습니다.";
        }

        string summary = "선택된 클래스들:\n";
        for (int i = 0; i < selectedClasses.Count; i++)
        {
            summary += $"{i + 1}. {selectedClasses[i].className}\n";
        }
        return summary;
    }
}
