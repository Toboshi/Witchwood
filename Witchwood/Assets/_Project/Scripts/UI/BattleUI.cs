using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleUI : MonoBehaviour
{
    [SerializeField]
    RectTransform m_BattlePanel = null;

    [SerializeField]
    RectTransform m_EnemyPanel = null;

    [SerializeField]
    Text m_EnemyName = null;

    [SerializeField]
    Slider m_EnemyHealth = null;

    IEnumerator m_FightCoroutine = null;
    Enemy m_Enemy = null;

    void Start ()
    {
        m_BattlePanel.gameObject.SetActive(false);
        m_EnemyPanel.gameObject.SetActive(false);
    }
	
	void Update ()
    {
	    
	}

    public void BattleStart(Enemy aEnemy)
    {
        Board.i.m_AllowMovement = false;
        Board.i.m_AllowRotation = false;
        m_BattlePanel.gameObject.SetActive(true);
        m_EnemyPanel.gameObject.SetActive(true);
        m_Enemy = aEnemy;
    }

    void BattleEnd()
    {
        Board.i.m_AllowMovement = true;
        Board.i.m_AllowRotation = true;
        m_BattlePanel.gameObject.SetActive(false);
        m_EnemyPanel.gameObject.SetActive(false);
        m_Enemy = null;

        StopCoroutine(m_FightCoroutine);
        m_FightCoroutine = null;
    }

    public void Flee()
    {
        Board.i.GetActiveCharacter().ChangeHealth(-1);
        BattleEnd();
    }

    public void Attack()
    {
        if (m_FightCoroutine != null) return;
        m_FightCoroutine = fight_cr();
        StartCoroutine(m_FightCoroutine);
    }

    IEnumerator fight_cr()
    {
        m_BattlePanel.gameObject.SetActive(false);

        int damage = Random.Range(0, 7);
        if (damage > 0)
        {
            m_Enemy.ChangeHealth(-damage);
            FindObjectOfType<TrapUI>().Activate(damage.ToString());
            m_EnemyHealth.value = m_Enemy.GetHealth();

            if (m_Enemy.GetHealth() <= 0)
            {
                BattleEnd();
            }
        }
        else
        {
            FindObjectOfType<TrapUI>().Activate("Miss");
        }

        yield return new WaitForSeconds(2.5f);

        damage = m_Enemy.GetDamage() + Random.Range(-3, 3) - 4;
        if (damage > 0)
        {
            Board.i.GetActiveCharacter().ChangeHealth(-damage);
            FindObjectOfType<TrapUI>().Activate(damage.ToString());
        }
        else
        {
            FindObjectOfType<TrapUI>().Activate("Miss");
        }

        yield return new WaitForSeconds(2.5f);

        m_BattlePanel.gameObject.SetActive(true);
        m_FightCoroutine = null;
    }
}
