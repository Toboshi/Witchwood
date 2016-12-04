using UnityEngine;
using System.Collections;

public class Enemy : Character
{
    [SerializeField]
    int m_Damage = 1;

    IEnumerator m_DodgeCoroutine = null;

    protected override void Start()
    {
        base.Start();
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

    public float GetHealthPercent()
    {
        float result = (float)m_Health / m_MaxHealth;
        return result;
    }

    public void Dodge()
    {
        if (m_DodgeCoroutine != null) return;
        m_DodgeCoroutine = dodge_cr();
        StartCoroutine(m_DodgeCoroutine);
    }

    IEnumerator dodge_cr()
    {
        Vector3 startPos = transform.position;
        Vector3 posRight = startPos + transform.right * 5;
        Vector3 posLeft = startPos - transform.right * 5;

        float moveSpeed = 2.0f;
        float dodgeTimer = 0;
        float duration = 9.9f;
        float t = 0;

        while (t < duration)
        {
            if (t >= dodgeTimer)
            {
                moveSpeed = -moveSpeed;
                dodgeTimer += Random.Range(0, Vector3.Distance(transform.position, (moveSpeed > 0 ? posRight : posLeft)) / Mathf.Abs(moveSpeed));
            }

            transform.position += transform.right * moveSpeed * Time.deltaTime;

            t += Time.deltaTime;
            yield return null;
        }

        transform.position = startPos;
        m_DodgeCoroutine = null;
    }
}
