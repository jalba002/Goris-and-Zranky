using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    [Header("General Settings")] public bool m_DisableOnDeath;
    public bool m_SpawnLootOnDeath;
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

    [Title("Debug")] public bool DebugMode = false;

    void Awake()
    {
        m_AttachedGameObject = this.gameObject;

        m_CurrentHealth = m_MaxHealth;
        m_CurrentShield = m_MaxShield;

        //Register into the Events
        onDamageTaken.AddListener(CheckDeath);
        onCharacterDeath.AddListener(OnCharacterDeath);
        onCharacterRespawn.AddListener(OnCharacterRespawn);
    }

    public void DealDamage(float amount)
    {
        int l_Damage = (int) amount;
        if (m_GodMode) return;
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
        onDamageTaken.Invoke();
    }

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
    
    private void CheckDeath()
    {
        if (m_CurrentHealth <= 0)
        {
            m_CurrentHealth = 0;
            onCharacterDeath.Invoke();
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