using UnityEngine;

[CreateAssetMenu(fileName = "ClassData", menuName = "Scriptable Objects/ClassData")]
public class ClassData : ScriptableObject
{
    [Header("클래스 기본 정보")]
    public string className;
    public string classDescription;
    public Sprite classIcon;

    [Header("2D 캐릭터 설정")]
    public Sprite characterSprite;
    public GameObject characterPrefab2D; // 2D 캐릭터 프리팹

    [Header("클래스 스탯")]
    public int health = 100;
    public int attack = 10;
    public int defense = 5;
    public int speed = 8;
    public int mana = 50;

    [Header("2D 스폰 설정")]
    public Vector2 spawnOffset = Vector2.zero;
    public float spawnRadius = 2f;
    public bool canMove = true;
    public bool canAttack = true;

    [Header("2D 물리 설정")]
    public float colliderRadius = 0.5f;
    public bool useRigidbody2D = true;
    public float mass = 1f;
    public float drag = 0f;
    public float angularDrag = 0.5f;

    [Header("클래스 특수 능력")]
    public bool hasSpecialAbility = false;
    public string specialAbilityName = "";
    public string specialAbilityDescription = "";
    public float specialAbilityCooldown = 5f;

    [Header("클래스 색상 테마")]
    public Color primaryColor = Color.white;
    public Color secondaryColor = Color.gray;
    public Color accentColor = Color.yellow;

    // 유틸리티 메서드들
    public bool IsValid()
    {
        return !string.IsNullOrEmpty(className);
    }

    public string GetDisplayName()
    {
        return string.IsNullOrEmpty(className) ? "Unknown Class" : className;
    }

    public string GetDescription()
    {
        return string.IsNullOrEmpty(classDescription) ? "No description available." : classDescription;
    }

    public Sprite GetClassIcon()
    {
        return classIcon != null ? classIcon : null;
    }

    public Sprite GetCharacterSprite()
    {
        return characterSprite != null ? characterSprite : null;
    }

    public GameObject GetCharacterPrefab()
    {
        return characterPrefab2D != null ? characterPrefab2D : null;
    }

    public float GetHealthPercentage(int currentHealth)
    {
        return (float)currentHealth / health;
    }

    public int CalculateDamage(int baseDamage)
    {
        return baseDamage + attack;
    }

    public int CalculateDefense(int incomingDamage)
    {
        return Mathf.Max(1, incomingDamage - defense);
    }

    public float GetMoveSpeed()
    {
        return speed;
    }

    public bool CanUseSpecialAbility()
    {
        return hasSpecialAbility && !string.IsNullOrEmpty(specialAbilityName);
    }

    public string GetSpecialAbilityInfo()
    {
        if (!CanUseSpecialAbility())
            return "No special ability";
        
        return $"{specialAbilityName}: {specialAbilityDescription} (Cooldown: {specialAbilityCooldown}s)";
    }

    public Color GetPrimaryColor()
    {
        return primaryColor;
    }

    public Color GetSecondaryColor()
    {
        return secondaryColor;
    }

    public Color GetAccentColor()
    {
        return accentColor;
    }

    public void SetColors(Color primary, Color secondary, Color accent)
    {
        primaryColor = primary;
        secondaryColor = secondary;
        accentColor = accent;
    }

    public string GetClassSummary()
    {
        return $"Class: {GetDisplayName()}\n" +
               $"Description: {GetDescription()}\n" +
               $"Health: {health} | Attack: {attack} | Defense: {defense} | Speed: {speed}\n" +
               $"Special Ability: {(CanUseSpecialAbility() ? GetSpecialAbilityInfo() : "None")}";
    }

    public bool Equals(ClassData other)
    {
        if (other == null) return false;
        return className == other.className;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as ClassData);
    }

    public override int GetHashCode()
    {
        return className != null ? className.GetHashCode() : 0;
    }

    public static bool operator ==(ClassData a, ClassData b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;
        return a.Equals(b);
    }

    public static bool operator !=(ClassData a, ClassData b)
    {
        return !(a == b);
    }
}
