using UnityEngine;
using UnityEngine.UI;

public class CharacterInfo : MonoBehaviour
{
    [Header("캐릭터 정보")]
    public ClassData classData;
    public int characterIndex;
    public string characterName;

    [Header("캐릭터 상태")]
    public bool isSelected = false;
    public bool isMoving = false;
    public bool isAttacking = false;

    [Header("캐릭터 스탯")]
    public int currentHealth;
    public int maxHealth = 100;
    public int attack = 10;
    public int defense = 5;
    public int speed = 8;

    [Header("UI 표시")]
    public GameObject nameTagPrefab;
    public GameObject healthBarPrefab;
    public Transform uiParent;

    private GameObject nameTag;
    private GameObject healthBar;

    private void Start()
    {
        InitializeCharacter();
        CreateUI();
    }

    private void InitializeCharacter()
    {
        if (classData != null)
        {
            characterName = classData.GetDisplayName();
            gameObject.name = $"{characterName}_{characterIndex + 1}";
            
            // 클래스 데이터에서 스탯 로드
            maxHealth = classData.health;
            attack = classData.attack;
            defense = classData.defense;
            speed = classData.speed;
        }

        currentHealth = maxHealth;
    }

    private void CreateUI()
    {
        if (uiParent == null)
        {
            uiParent = transform;
        }

        // 이름 태그 생성
        if (nameTagPrefab != null)
        {
            nameTag = Instantiate(nameTagPrefab, uiParent);
            UpdateNameTag();
        }

        // 체력바 생성
        if (healthBarPrefab != null)
        {
            healthBar = Instantiate(healthBarPrefab, uiParent);
            UpdateHealthBar();
        }
    }

    public void SetClassData(ClassData data)
    {
        classData = data;
        if (classData != null)
        {
            characterName = classData.GetDisplayName();
            gameObject.name = $"{characterName}_{characterIndex + 1}";
            
            // 클래스 데이터에서 스탯 로드
            maxHealth = classData.health;
            attack = classData.attack;
            defense = classData.defense;
            speed = classData.speed;
            
            UpdateNameTag();
        }
    }

    public void SetCharacterIndex(int index)
    {
        characterIndex = index;
        if (classData != null)
        {
            characterName = classData.GetDisplayName();
            gameObject.name = $"{characterName}_{characterIndex + 1}";
            UpdateNameTag();
        }
    }

    private void UpdateNameTag()
    {
        if (nameTag != null)
        {
            Text nameText = nameTag.GetComponentInChildren<Text>();
            if (nameText != null)
            {
                nameText.text = characterName;
            }
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            Slider healthSlider = healthBar.GetComponentInChildren<Slider>();
            if (healthSlider != null)
            {
                healthSlider.value = (float)currentHealth / maxHealth;
            }
        }
    }

    // 캐릭터 선택/해제
    public void SelectCharacter()
    {
        isSelected = true;
        // 선택 효과 (예: 하이라이트)
        SetSelectionEffect(true);
    }

    public void DeselectCharacter()
    {
        isSelected = false;
        SetSelectionEffect(false);
    }

    private void SetSelectionEffect(bool selected)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            if (selected)
            {
                // 선택된 상태로 색상 변경 (예: 더 밝게)
                spriteRenderer.color = Color.yellow;
            }
            else
            {
                // 기본 색상으로 복원
                spriteRenderer.color = Color.white;
            }
        }
    }

    // 체력 관리
    public void TakeDamage(int damage)
    {
        int actualDamage = classData != null ? 
            classData.CalculateDefense(damage) : 
            Mathf.Max(1, damage - defense);
            
        currentHealth = Mathf.Max(0, currentHealth - actualDamage);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        UpdateHealthBar();
    }

    public void Die()
    {
        Debug.Log($"{characterName}이(가) 죽었습니다.");
        // 죽음 애니메이션 또는 효과
        gameObject.SetActive(false);
    }

    // 공격 관련
    public void Attack(CharacterInfo target)
    {
        if (target != null && !isAttacking)
        {
            isAttacking = true;
            Debug.Log($"{characterName}이(가) {target.characterName}을(를) 공격합니다!");
            
            // 공격 애니메이션 (여기서는 간단히 로그만)
            StartCoroutine(AttackCoroutine(target));
        }
    }

    private System.Collections.IEnumerator AttackCoroutine(CharacterInfo target)
    {
        // 공격 애니메이션 시간 (예: 1초)
        yield return new WaitForSeconds(1f);
        
        // 실제 데미지 적용 (ClassData의 데미지 계산 사용)
        int finalDamage = classData != null ? 
            classData.CalculateDamage(attack) : 
            attack;
        target.TakeDamage(finalDamage);
        
        isAttacking = false;
    }

    // 2D 이동 관련
    public void MoveTo(Vector2 targetPosition)
    {
        if (!isMoving)
        {
            StartCoroutine(MoveCoroutine2D(targetPosition));
        }
    }

    public void MoveTo(Vector3 targetPosition)
    {
        MoveTo(new Vector2(targetPosition.x, targetPosition.y));
    }

    private System.Collections.IEnumerator MoveCoroutine2D(Vector2 targetPosition)
    {
        isMoving = true;
        Vector2 startPosition = transform.position;
        
        // ClassData에서 이동 속도 가져오기
        float moveSpeed = classData != null ? classData.GetMoveSpeed() : speed;
        float moveTime = Vector2.Distance(startPosition, targetPosition) / moveSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveTime;
            Vector2 newPosition = Vector2.Lerp(startPosition, targetPosition, t);
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
            yield return null;
        }

        transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
        isMoving = false;
    }

    // 2D 물리 기반 이동
    public void MoveWithPhysics(Vector2 direction)
    {
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            // ClassData에서 이동 속도 가져오기
            float moveSpeed = classData != null ? classData.GetMoveSpeed() : speed;
            rb2d.linearVelocity = direction * moveSpeed;
        }
    }

    // 2D 애니메이션 기반 이동
    public void PlayWalkAnimation()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play("Walk"); // 또는 애니메이션 이름 관리 규칙에 따라 변경
        }
    }

    public void PlayIdleAnimation()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play("Idle"); // 또는 애니메이션 이름 관리 규칙에 따라 변경
        }
    }

    // 외부에서 호출할 수 있는 메서드들
    public ClassData GetClassData()
    {
        return classData;
    }

    public string GetCharacterName()
    {
        return characterName;
    }

    public int GetCharacterIndex()
    {
        return characterIndex;
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    public float GetHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }

    // 특수 능력 관련
    public bool CanUseSpecialAbility()
    {
        return classData != null && classData.CanUseSpecialAbility();
    }

    public string GetSpecialAbilityInfo()
    {
        return classData != null ? classData.GetSpecialAbilityInfo() : "No special ability";
    }

    public void UseSpecialAbility()
    {
        if (CanUseSpecialAbility())
        {
            Debug.Log($"{characterName}이(가) 특수 능력 '{classData.specialAbilityName}'을(를) 사용했습니다!");
            // 여기에 특수 능력 로직을 추가할 수 있습니다
        }
    }

    // 클래스 정보 가져오기
    public string GetClassSummary()
    {
        return classData != null ? classData.GetClassSummary() : "No class data";
    }

    public Color GetClassPrimaryColor()
    {
        return classData != null ? classData.GetPrimaryColor() : Color.white;
    }

    public Color GetClassSecondaryColor()
    {
        return classData != null ? classData.GetSecondaryColor() : Color.gray;
    }

    public Color GetClassAccentColor()
    {
        return classData != null ? classData.GetAccentColor() : Color.yellow;
    }

    // 마우스 클릭으로 선택
    private void OnMouseDown()
    {
        if (isSelected)
        {
            DeselectCharacter();
        }
        else
        {
            SelectCharacter();
        }
    }
}
