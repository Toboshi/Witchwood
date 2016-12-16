using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    Pattern m_Pattern = null;
    Character m_Target = null;
    int m_Damage = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        Character hit = other.GetComponent<Character>();
        if (hit == m_Target)
        {
            if (m_Damage > 0)
            {
                if (hit.ChangeHealth(-m_Damage)) FindObjectOfType<BattleUI>().BattleEnd();
            }
            else
            {
                FindObjectOfType<BattleUI>().m_Power++;
            }

            m_Pattern.RemoveBullet(this);
        }
    }

    public void Initialize(Character aTarget, Pattern aPattern, int aDamage = 1)
    {
        m_Pattern = aPattern;
        m_Target = aTarget;
        m_Damage = aDamage;

        if(m_Damage == 0)
        {
            transform.Find("Mesh").GetComponent<Renderer>().material.color = Color.green;
        }
    }
}
