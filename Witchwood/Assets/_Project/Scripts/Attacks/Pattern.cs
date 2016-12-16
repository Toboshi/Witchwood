using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pattern : MonoBehaviour
{
    [SerializeField]
    GameObject m_BulletPrefab = null;
    protected List<Bullet> m_Bullets = new List<Bullet>();
    Character m_Character = null;
    protected IEnumerator m_PatternCoroutine = null;

	public void Initialize (Character aCharacter)
    {
        m_Character = aCharacter;
	}

    public void Attack(Character aCharacter)
    {
        if (m_PatternCoroutine != null) return;
        m_PatternCoroutine = pattern_cr(aCharacter);
        StartCoroutine(m_PatternCoroutine);
    }

    protected void Fire(Vector3 aAim, Character aTarget, int aDamage = 1)
    {
        GameObject obj = Instantiate(m_BulletPrefab, m_Character.transform.position + Vector3.up, Quaternion.LookRotation(aAim - m_Character.transform.position)) as GameObject;
        Bullet shot = obj.GetComponent<Bullet>();
        shot.Initialize(aTarget, this, aDamage);
        m_Bullets.Add(shot);
    }
	
	void Update ()
    {
        foreach (Bullet bullet in m_Bullets)
        {
            bullet.transform.position += bullet.transform.forward * 5 * Time.deltaTime;
        }
	}

    public Character GetCharacter()
    {
        return m_Character;
    }

    public void RemoveBullet(Bullet aBullet)
    {
        if (!m_Bullets.Contains(aBullet)) return;

        m_Bullets.Remove(aBullet);
        Destroy(aBullet.gameObject);
    }

    protected virtual IEnumerator pattern_cr(Character aTarget)
    {
        for (int i = 0; i < 20; i++)
        {
            Fire(transform.position + transform.forward * 20, aTarget);
            Fire(transform.position + transform.forward * 20 + aTarget.transform.right * 3, aTarget);
            Fire(transform.position + transform.forward * 20 - aTarget.transform.right * 3, aTarget);

            yield return new WaitForSeconds(0.75f);
        }

        yield return new WaitForSeconds(4);

        foreach (Bullet shot in m_Bullets)
        {
            Destroy(shot.gameObject);
        }
        m_Bullets.Clear();

        m_PatternCoroutine = null;
    }
}
