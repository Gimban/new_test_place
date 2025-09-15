using UnityEngine;
using System.Collections.Generic;

public class ClassManager : MonoBehaviour
{
    public static ClassManager Instance { get; private set; }

    [Header("Class Data Settings")]
    public List<ClassData> allClassData = new List<ClassData>();
    
    private ClassData selectedClass;

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

    public void SetSelectedClass(ClassData classData)
    {
        selectedClass = classData;
        Debug.Log($"선택된 클래스가 설정되었습니다: {classData.className}");
    }

    public ClassData GetSelectedClass()
    {
        return selectedClass;
    }

    public List<ClassData> GetAllClasses()
    {
        return new List<ClassData>(allClassData);
    }

    public ClassData GetClassByName(string className)
    {
        return allClassData.Find(c => c.className == className);
    }

    public bool HasSelectedClass()
    {
        return selectedClass != null;
    }

    public void ClearSelection()
    {
        selectedClass = null;
        Debug.Log("클래스 선택이 초기화되었습니다.");
    }

    // 특정 클래스의 정보를 가져오는 메서드들
    public string GetSelectedClassName()
    {
        return selectedClass != null ? selectedClass.className : "선택된 클래스 없음";
    }

    public string GetSelectedClassDescription()
    {
        return selectedClass != null ? selectedClass.classDescription : "클래스를 선택해주세요";
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
            Debug.Log($"클래스 데이터가 제거되었습니다: {classData.className}");
        }
    }
}
