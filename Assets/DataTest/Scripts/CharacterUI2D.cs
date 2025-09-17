using UnityEngine;
using UnityEngine.UI;

public class CharacterUI2D : MonoBehaviour
{
    [Header("2D UI 설정")]
    public GameObject nameTagPrefab;
    public GameObject healthBarPrefab;
    public Transform uiParent;
    public float uiOffsetY = 1f; // 캐릭터 위에 UI 표시할 오프셋

    [Header("UI 스타일")]
    public Color nameTagColor = Color.white;
    public Color healthBarColor = Color.red;
    public Color healthBarBackgroundColor = Color.gray;
    public float uiScale = 1f;

    private GameObject nameTag;
    private GameObject healthBar;
    private Text nameText;
    private Slider healthSlider;
    private Image healthFill;
    private Image healthBackground;

    private CharacterInfo characterInfo;
    private Camera mainCamera;

    private void Start()
    {
        characterInfo = GetComponent<CharacterInfo>();
        mainCamera = Camera.main;
        
        if (mainCamera == null)
        {
            mainCamera = FindObjectOfType<Camera>();
        }

        CreateUI();
    }

    private void Update()
    {
        UpdateUIPosition();
        UpdateHealthBar();
    }

    private void CreateUI()
    {
        if (uiParent == null)
        {
            // UI를 위한 빈 오브젝트 생성
            GameObject uiContainer = new GameObject("CharacterUI");
            uiContainer.transform.SetParent(transform);
            uiContainer.transform.localPosition = new Vector3(0, uiOffsetY, 0);
            uiParent = uiContainer.transform;
        }

        // 이름 태그 생성
        if (nameTagPrefab != null)
        {
            nameTag = Instantiate(nameTagPrefab, uiParent);
            nameText = nameTag.GetComponentInChildren<Text>();
            if (nameText != null)
            {
                nameText.text = characterInfo.GetCharacterName();
                nameText.color = nameTagColor;
            }
        }
        else
        {
            CreateDefaultNameTag();
        }

        // 체력바 생성
        if (healthBarPrefab != null)
        {
            healthBar = Instantiate(healthBarPrefab, uiParent);
            healthSlider = healthBar.GetComponentInChildren<Slider>();
            if (healthSlider != null)
            {
                healthFill = healthSlider.fillRect.GetComponent<Image>();
                healthBackground = healthSlider.GetComponent<Image>();
                
                if (healthFill != null)
                    healthFill.color = healthBarColor;
                if (healthBackground != null)
                    healthBackground.color = healthBarBackgroundColor;
            }
        }
        else
        {
            CreateDefaultHealthBar();
        }

        // UI 스케일 적용
        if (uiParent != null)
        {
            uiParent.localScale = Vector3.one * uiScale;
        }
    }

    private void CreateDefaultNameTag()
    {
        // 기본 이름 태그 생성
        GameObject nameTagObj = new GameObject("NameTag");
        nameTagObj.transform.SetParent(uiParent);
        
        // Canvas 추가
        Canvas canvas = nameTagObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.sortingOrder = 10;
        
        // Text 추가
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(nameTagObj.transform);
        
        nameText = textObj.AddComponent<Text>();
        nameText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        nameText.text = characterInfo.GetCharacterName();
        nameText.fontSize = 20;
        nameText.color = nameTagColor;
        nameText.alignment = TextAnchor.MiddleCenter;
        
        // RectTransform 설정
        RectTransform textRect = nameText.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        
        nameTag = nameTagObj;
    }

    private void CreateDefaultHealthBar()
    {
        // 기본 체력바 생성
        GameObject healthBarObj = new GameObject("HealthBar");
        healthBarObj.transform.SetParent(uiParent);
        
        // Canvas 추가
        Canvas canvas = healthBarObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.sortingOrder = 10;
        
        // 배경 이미지
        GameObject backgroundObj = new GameObject("Background");
        backgroundObj.transform.SetParent(healthBarObj.transform);
        
        healthBackground = backgroundObj.AddComponent<Image>();
        healthBackground.color = healthBarBackgroundColor;
        
        RectTransform bgRect = healthBackground.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;
        
        // Slider 추가
        healthSlider = healthBarObj.AddComponent<Slider>();
        healthSlider.fillRect = backgroundObj.GetComponent<RectTransform>();
        healthSlider.value = 1f;
        
        // Fill 이미지
        GameObject fillObj = new GameObject("Fill");
        fillObj.transform.SetParent(backgroundObj.transform);
        
        healthFill = fillObj.AddComponent<Image>();
        healthFill.color = healthBarColor;
        
        RectTransform fillRect = healthFill.GetComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.one;
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;
        
        healthBar = healthBarObj;
    }

    private void UpdateUIPosition()
    {
        if (uiParent != null && mainCamera != null)
        {
            // UI가 항상 카메라를 향하도록 회전
            Vector3 lookDirection = mainCamera.transform.position - uiParent.position;
            lookDirection.y = 0; // Y축 회전만
            if (lookDirection != Vector3.zero)
            {
                uiParent.rotation = Quaternion.LookRotation(lookDirection);
            }
        }
    }

    private void UpdateHealthBar()
    {
        if (healthSlider != null && characterInfo != null)
        {
            healthSlider.value = characterInfo.GetHealthPercentage();
        }
    }

    public void UpdateName(string newName)
    {
        if (nameText != null)
        {
            nameText.text = newName;
        }
    }

    public void SetHealthBarVisible(bool visible)
    {
        if (healthBar != null)
        {
            healthBar.SetActive(visible);
        }
    }

    public void SetNameTagVisible(bool visible)
    {
        if (nameTag != null)
        {
            nameTag.SetActive(visible);
        }
    }

    public void SetUIScale(float scale)
    {
        uiScale = scale;
        if (uiParent != null)
        {
            uiParent.localScale = Vector3.one * uiScale;
        }
    }

    public void SetNameTagColor(Color color)
    {
        nameTagColor = color;
        if (nameText != null)
        {
            nameText.color = color;
        }
    }

    public void SetHealthBarColor(Color color)
    {
        healthBarColor = color;
        if (healthFill != null)
        {
            healthFill.color = color;
        }
    }
}
