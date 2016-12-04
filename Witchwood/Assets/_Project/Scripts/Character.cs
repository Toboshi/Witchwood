using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
    [SerializeField]
    protected int m_MaxHealth = 10;
    protected int m_Health = 1;

    [SerializeField]
    protected Pattern[] m_Attacks = null;

    protected virtual void Start ()
    {
        m_Health = m_MaxHealth;

        foreach (Pattern attack in m_Attacks)
        {
            attack.Initialize(this);
        }
    }

    public void Attack(Character aTarget)
    {
        foreach (Pattern attack in m_Attacks)
        {
            attack.Attack(aTarget);
        }
    }

    public bool ChangeHealth(int aHealthChange)
    {
        m_Health += aHealthChange;
        if (m_Health > m_MaxHealth) m_Health = m_MaxHealth;
        if (m_Health <= 0)
        {
            gameObject.SetActive(false);
            return true;
        }

        return false;
    }

    public int GetHealth()
    {
        return m_Health;
    }

    public int GetMaxHealth()
    {
        return m_MaxHealth;
    }
}
