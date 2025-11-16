using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    public static Stamina Instance { get; private set; }

    public GameObject staminaBarBG;
    public GameObject staminaBar;
    
    // 능력의 스태미나 소모는 100으로 고정한다 가정함
    public int maxStamina = 200;
    public float stamina;

    public enum StaminaEventType
    {
        AttackHit,
        TakeDamage,
        Parry,
        PerSecond
    }

    private Dictionary<StaminaEventType, float> recoveryValues;

    [Header("Stamina Recovery Events")]
    public float attackHit = 0;
    public float takeDamage = 0;
    public float parry = 0;
    public float perSecond = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        recoveryValues = new Dictionary<StaminaEventType, float>()
        {
            { StaminaEventType.AttackHit, attackHit },
            { StaminaEventType.TakeDamage, takeDamage },
            { StaminaEventType.Parry, parry },
            { StaminaEventType.PerSecond, perSecond },
        };

        stamina = 0;
        staminaBarBG.GetComponent<RectTransform>().sizeDelta = new Vector2(maxStamina + 5, staminaBarBG.GetComponent<RectTransform>().sizeDelta.y);
        UpdateStaminaBar();
    }

    public void Recover(float value)
    {
        stamina += value;
        if (stamina > maxStamina) stamina = maxStamina;
        if (stamina < 0) stamina = 0;

        UpdateStaminaBar();
    }

    public void Recover(StaminaEventType staminaEventType)
    {
        stamina += recoveryValues[staminaEventType];
        if (stamina > maxStamina) stamina = maxStamina;
        if (stamina < 0) stamina = 0;

        UpdateStaminaBar();
    }

    public bool TrySkill()
    {
        if (stamina < 100) return false;
        stamina -= 100;
        UpdateStaminaBar();
        return true;
    }

    private void UpdateStaminaBar()
    {
        staminaBar.GetComponent<RectTransform>().sizeDelta = new Vector2(stamina, staminaBar.GetComponent<RectTransform>().sizeDelta.y);
    }

    // Update is called once per frame
    void Update()
    {
        Recover(recoveryValues[StaminaEventType.PerSecond]* Time.deltaTime);
        UpdateStaminaBar();
    }
}
