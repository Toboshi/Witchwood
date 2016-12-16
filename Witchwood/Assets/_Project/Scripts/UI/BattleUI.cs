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

    [SerializeField]
    Text m_PowerText = null;

    IEnumerator m_FightCoroutine = null;
    Enemy m_Enemy = null;
    float BATTLE_DIST = 16;

    float m_SpeedMod = 1;
    public int m_Armor = 0;
    public int m_Power = 0;

    void Start ()
    {
        m_BattlePanel.gameObject.SetActive(false);
        m_EnemyPanel.gameObject.SetActive(false);
    }
	
	void Update ()
    {
        if (m_Enemy == null) return;

        m_EnemyHealth.value = m_Enemy.GetHealthPercent();
        m_PowerText.text = m_Power.ToString();

        Transform player = Board.i.GetActiveCharacter().transform;
        float dist = Vector3.Distance(player.position, m_Enemy.transform.position);
        if (dist > BATTLE_DIST)
        {
            player.position += (m_Enemy.transform.position - player.position).normalized * (dist - BATTLE_DIST);
        }
        player.LookAt(m_Enemy.transform);
	}

    public void BattleStart(Enemy aEnemy)
    {
        FindObjectOfType<DatMusics>().PlayCombatTrack();

        Vector3 pos = aEnemy.transform.position + Vector3.forward * BATTLE_DIST;
        Vector3 posS = aEnemy.transform.position + Vector3.back * BATTLE_DIST;
        Vector3 posE = aEnemy.transform.position + Vector3.left * BATTLE_DIST;
        Vector3 posW = aEnemy.transform.position + Vector3.right * BATTLE_DIST;
        Vector3 playerPos = Board.i.GetActiveCharacter().transform.position;
        if (Vector3.Distance(playerPos, posS) < Vector3.Distance(playerPos, pos)) pos = posS;
        if (Vector3.Distance(playerPos, posE) < Vector3.Distance(playerPos, pos)) pos = posE;
        if (Vector3.Distance(playerPos, posW) < Vector3.Distance(playerPos, pos)) pos = posW;
        aEnemy.transform.LookAt(new Vector3(pos.x, aEnemy.transform.position.y, pos.z));
        Board.i.GetActiveCharacter().transform.position = pos;
        Board.i.GetActiveCharacter().transform.LookAt(aEnemy.transform);
        Camera.main.transform.position = Camera.main.transform.position + Vector3.up + Board.i.GetActiveCharacter().transform.forward * 2;
        Camera.main.transform.Rotate(Vector3.right * 20);

        Board.i.SetGameState(Board.GameState.Battle);
        m_BattlePanel.gameObject.SetActive(true);
        m_EnemyPanel.gameObject.SetActive(true);
        m_Enemy = aEnemy;

        m_SpeedMod = 1;
        m_Armor = 0;
        m_Power = 2;
    }

    public void BattleEnd()
    {
        FindObjectOfType<DatMusics>().PlayOverworldTrack();

        Board.i.SetActiveCharacter(Board.i.GetActiveCharacter());
        Board.i.SetGameState(Board.GameState.Movement);
        m_BattlePanel.gameObject.SetActive(false);
        m_EnemyPanel.gameObject.SetActive(false);
        m_Enemy = null;

        StopCoroutine(m_FightCoroutine);
        m_FightCoroutine = null;
    }

    public void Attack()
    {
        if (m_FightCoroutine != null) return;
        m_FightCoroutine = fight_cr();
        StartCoroutine(m_FightCoroutine);
    }

    public void IncSpeedMod()
    {
        if (m_Power < 2) return;

        m_SpeedMod += 0.25f;
        m_Power -= 2;
    }

    public float GetSpeedMod()
    {
        return m_SpeedMod;
    }

    public void ArmorUp()
    {
        if (m_Power < 3) return;

        m_Armor++;
        m_Power -= 3;
    }

    IEnumerator fight_cr()
    {
        m_BattlePanel.gameObject.SetActive(false);

        if (m_Enemy.ChangeHealth(-m_Power)) BattleEnd();
        m_Power = 0;

        yield return new WaitForSeconds(1);

        m_Enemy.Attack(Board.i.GetActiveCharacter());

        yield return new WaitForSeconds(20);

        m_Armor = 0;
        m_BattlePanel.gameObject.SetActive(true);
        m_FightCoroutine = null;
    }
}
