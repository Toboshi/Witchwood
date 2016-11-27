using UnityEngine;
using System.Collections;
using System;

public class UseEffectHeal : MonoBehaviour, UseEffect
{
    [SerializeField]
    int m_HealAmount = 1;

    public void Use()
    {
        Board.i.GetActiveCharacter().ChangeHealth(m_HealAmount);
    }
}
