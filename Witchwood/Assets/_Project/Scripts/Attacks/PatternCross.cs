using UnityEngine;
using System.Collections;

public class PatternCross : Pattern
{
    protected override IEnumerator pattern_cr(Character aTarget)
    {
        for (int i = 0; i < 30; i++)
        {
            float offset = Mathf.Lerp(-10, 10, (float)(i % 15) / 15);
            Fire(transform.position + transform.forward * 40 + transform.right * offset, aTarget, 0);
            Fire(transform.position + transform.forward * 40 - transform.right * offset, aTarget);
            Fire(transform.position - transform.forward * 40 + transform.right * offset, aTarget, 0);
            Fire(transform.position - transform.forward * 40 - transform.right * offset, aTarget);
            Fire(transform.position + transform.right * 40 + transform.forward * offset, aTarget);
            Fire(transform.position + transform.right * 40 - transform.forward * offset, aTarget);
            Fire(transform.position - transform.right * 40 + transform.forward * offset, aTarget);
            Fire(transform.position - transform.right * 40 - transform.forward * offset, aTarget);

            yield return new WaitForSeconds(0.5f);
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
