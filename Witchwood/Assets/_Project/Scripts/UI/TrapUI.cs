using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TrapUI : MonoBehaviour
{
    [SerializeField]
    RectTransform m_Panel = null;

    [SerializeField]
    Text m_Text = null;
    Color m_TextColor = Color.white;

    IEnumerator m_TrapCoroutine = null;

    void Start ()
    {
        m_TextColor = m_Text.color;
        m_Panel.gameObject.SetActive(false);
	}

    public void Activate(string aText = "TRAPPED")
    {
        if (m_TrapCoroutine != null) return;
        m_Text.text = aText;
        m_TrapCoroutine = trap_cr();
        StartCoroutine(m_TrapCoroutine);
    }

    IEnumerator trap_cr()
    {
        m_Panel.gameObject.SetActive(true);

        float duration = 2;
        float t = 0;
        Color end = m_TextColor;
        end.a = 0;

        while (t < duration)
        {
            float val = t / duration;
            m_Text.color = Color.Lerp(m_TextColor, end, val);

            t += Time.deltaTime;
            yield return null;
        }

        m_Panel.gameObject.SetActive(false);
        m_TrapCoroutine = null;
    }
}
