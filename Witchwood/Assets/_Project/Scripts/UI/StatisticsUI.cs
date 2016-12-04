using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatisticsUI : MonoBehaviour
{
    [SerializeField]
    Text m_NameText = null;

    [SerializeField]
    Text m_HealthText = null;

    [SerializeField]
    Text m_AspectText = null;

    void Start ()
    {
	
	}
	
	void Update ()
    {
        m_NameText.text = Board.i.GetActiveCharacter().GetName();
        m_HealthText.text = Board.i.GetActiveCharacter().GetHealth().ToString() + " / " + Board.i.GetActiveCharacter().GetMaxHealth().ToString();
        m_AspectText.text = Board.i.GetActiveCharacter().GetAspect().ToString();
    }
}
