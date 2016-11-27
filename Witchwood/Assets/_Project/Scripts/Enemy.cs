using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    int m_MaxHealth = 10;
    int m_Health = 1;

    [SerializeField]
    int m_Damage = 3;

    void Start()
    {
        m_Health = m_MaxHealth;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<BattleUI>().BattleStart(this);
        }
    }

    public int GetDamage()
    {
        return m_Damage;
    }

    public float GetHealth()
    {
        float result = m_Health * 100 / m_MaxHealth;
        return result * 0.01f;
    }

    public void ChangeHealth(int aHealthChange)
    {
        m_Health += aHealthChange;
        if (m_Health > m_MaxHealth) m_Health = m_MaxHealth;
        if (m_Health <= 0) gameObject.SetActive(false);
    }
}
