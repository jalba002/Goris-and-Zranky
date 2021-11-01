using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    [Header("General Settings")] public bool m_DisableOnDeath;

    public bool m_SpawnLootOnDeath;
    [ShowIf("m_SpawnLootOnDeath")]public GameObject loot;
    [HideInInspector] public bool m_GodMode;

    [Header("Stats")] public int m_MaxHealth;
    public int m_MaxShield;
    [Range(0.0f, 1.0f)] public float m_ShieldAbsorbption;

    [Space(10)] [Header("Events")] public UnityEvent onDamageTaken;
    public UnityEvent onCharacterDeath;
    public UnityEvent onCharacterRespawn;

    private int m_CurrentHealth;
    private int m_CurrentShield;

    private GameObject m_AttachedGameObject;
    private bool IsDead = false;

    [Title("Debug")] public bool DebugMode = false;

    void Awake()
    {
        m_AttachedGameObject = this.gameObject;

        m_CurrentHealth = m_MaxHealth;
        m_CurrentShield = m_MaxShield;

        //Register into the Events
        onCharacterDeath.AddListener(OnCharacterDeath);
        onCharacterDeath.AddListener(SpawnLoot);
        onCharacterRespawn.AddListener(OnCharacterRespawn);
    }

    public void DealDamage(float amount)
    {
        int l_Damage = (int) amount;
        if (m_GodMode || IsDead) return;
        if (l_Damage <= 0 && DebugMode)
        {
            Debug.LogError($"Someone sent incorrect damage values to {this.gameObject.name}");
            return;
        }

        if (m_CurrentShield > 0)
        {
            var shieldDamage = (int) Mathf.Ceil(l_Damage * m_ShieldAbsorbption);
            m_CurrentShield -= shieldDamage;
            m_CurrentHealth -= l_Damage - shieldDamage;
            if (m_CurrentShield < 0) m_CurrentShield = 0;
        }
        else
        {
            m_CurrentHealth -= l_Damage;
        }

        if(DebugMode)
            Debug.Log(this.gameObject.name + ": " + m_CurrentHealth + "/" + m_MaxHealth + " HP. Damage: " + l_Damage);

        if (m_CurrentHealth <= 0)
        {
            m_CurrentHealth = 0;
            IsDead = true;
            onCharacterDeath.Invoke();
        }
        else
        {
            onDamageTaken.Invoke();
        }
    }

    [Button("Kill")]
    public void Kill()
    {
        DealDamage(m_MaxHealth);
    }

    #region Getters

    public int GetCurrentHealth()
    {
        return m_CurrentHealth;
    }

    public int GetCurrentShield()
    {
        return m_CurrentShield;
    }

    #endregion

    #region Restoration

    public bool RestoreHealth(int l_Amount)
    {
        if (m_CurrentHealth >= m_MaxHealth) return false;

        int l_Difference = m_MaxHealth - m_CurrentHealth;
        m_CurrentHealth += Mathf.Min(l_Difference, l_Amount);
        if (m_CurrentHealth > m_MaxHealth) m_CurrentHealth = m_MaxHealth;

        return true;
    }

    public bool RestoreShield(int l_Amount)
    {
        if (m_CurrentShield >= m_MaxShield) return false;

        int l_Difference = m_MaxShield - m_CurrentShield;
        m_CurrentShield += Mathf.Min(l_Difference, l_Amount);
        if (m_CurrentShield > m_MaxShield) m_CurrentShield = m_MaxShield;

        return true;
    }

    #endregion
    
    protected virtual void SpawnLoot()
    {
        if (m_SpawnLootOnDeath)
        {
            Instantiate(loot, this.gameObject.transform.position, Quaternion.identity);
        }
    }

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        
    }
    #endif

    #region Events

    private void OnCharacterDeath()
    {
        if (m_DisableOnDeath)
        {
            m_AttachedGameObject.SetActive(false);
        }
    }

    private void OnCharacterRespawn()
    {
        m_CurrentHealth = m_MaxHealth;
        m_CurrentShield = m_MaxShield;
    }

    #endregion
}