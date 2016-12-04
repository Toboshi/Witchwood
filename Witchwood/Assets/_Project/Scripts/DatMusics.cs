using UnityEngine;
using System.Collections;

public class DatMusics : MonoBehaviour
{
    [SerializeField]
    AudioClip m_OverworldTrack = null;

    [SerializeField]
    AudioClip m_CombatTrack = null;

    AudioSource m_Audio = null;

    void Start ()
    {
        m_Audio = GetComponent<AudioSource>();
        PlayOverworldTrack();
	}

    public void PlayOverworldTrack()
    {
        m_Audio.clip = m_OverworldTrack;
        m_Audio.Play();
    }

    public void PlayCombatTrack()
    {
        m_Audio.clip = m_CombatTrack;
        m_Audio.Play();
    }
}
