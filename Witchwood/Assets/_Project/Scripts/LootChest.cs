using UnityEngine;
using System.Collections;

public class LootChest : MonoBehaviour
{
    [SerializeField]
    Transform m_Lid = null;

    [SerializeField]
    GameObject[] m_Items = null;

    bool m_Active = false;

	void Start ()
    {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (m_Active) return;

        if (other.tag == "Player")
        {
            StartCoroutine(open_cr());
            m_Active = true;
        }
    }

    IEnumerator open_cr()
    {
        Quaternion start = m_Lid.localRotation;
        Quaternion end = Quaternion.Euler(0, 0, -100);

        float duration = 1;
        float t = 0;
        while (t < duration)
        {
            float val = t / duration;
            m_Lid.localRotation = Quaternion.Lerp(start, end, val);

            t += Time.deltaTime;
            yield return null;
        }

        m_Lid.localRotation = end;

        Instantiate(m_Items[Random.Range(0, m_Items.Length)], transform.position, Quaternion.identity);
    }
}
