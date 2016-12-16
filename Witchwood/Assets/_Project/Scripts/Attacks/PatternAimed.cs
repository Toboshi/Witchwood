using UnityEngine;
using System.Collections;

public class PatternAimed : Pattern
{
    protected override IEnumerator pattern_cr(Character aTarget)
    {
        for (int i = 0; i < 20; i++)
        {
            Fire(aTarget.transform.position, aTarget);
            Fire(aTarget.transform.position + aTarget.transform.right * 2, aTarget);
            Fire(aTarget.transform.position + aTarget.transform.right * 4, aTarget);
            Fire(aTarget.transform.position - aTarget.transform.right * 2, aTarget);
            Fire(aTarget.transform.position - aTarget.transform.right * 4, aTarget);

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
