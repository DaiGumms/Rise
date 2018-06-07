using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

    public GameObject Map_Pause;    //Reference to Game Map
    public GameObject UI_Pause;     //Reference to GUI

    private Toggle m_MenuToggle;
	private float m_TimeScaleRef = 1f;
    private float m_VolumeRef = 1f;
    private bool m_Paused;


    void Awake()
    {
        m_MenuToggle = GetComponent <Toggle> ();
	}

    //Turn Menu On
    private void MenuOn ()
    {
        Map_Pause.SetActive(false);
        UI_Pause.SetActive(false);

        m_TimeScaleRef = Time.timeScale;
        Time.timeScale = 0f;

        m_VolumeRef = AudioListener.volume;
        AudioListener.volume = 0.5f;

        m_Paused = true;

        print("Menu Active!");
    }

    //Turn Menu Off
    public void MenuOff ()
    {
        Map_Pause.SetActive(true);
        UI_Pause.SetActive(true);

        Time.timeScale = m_TimeScaleRef;
        AudioListener.volume = m_VolumeRef;

        m_Paused = false;

        print("Menu Disabled");
    }

    //Toggle Menu
    public void OnMenuStatusChange ()
    {
        if (m_MenuToggle.isOn && !m_Paused)
        {
            MenuOn();
        }
        else if (!m_MenuToggle.isOn && m_Paused)
        {
            MenuOff();
        }
    }

}
