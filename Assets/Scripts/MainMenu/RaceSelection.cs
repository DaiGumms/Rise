using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceSelection : MonoBehaviour {

    public static RaceSelection instance;

    public string Player1Race;
    public string Player2Race;
    public string Player3Race;
    public string Player4Race;
    public string Player5Race;
    public string Player6Race;

    public bool Player1Joined;
    public bool Player2Joined;
    public bool Player3Joined;
    public bool Player4Joined;
    public bool Player5Joined;
    public bool Player6Joined;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        //Dont destroy objects
        DontDestroyOnLoad(gameObject);
    }

    public void SetPlayer1Race()
    {
        Player1Race = "AVIAN";
        Player1Joined = true;
    }
    public void SetPlayer2Race()
    {
        Player2Race = "VORTA";
        Player2Joined = true;
    }
    public void SetPlayer3Race()
    {
        Player3Race = "KABAL";
        Player3Joined = true;
    }
    public void SetPlayer4Race()
    {
        Player4Race = "EVREI";
        Player4Joined = true;
    }
    public void SetPlayer5Race()
    {
        Player5Race = "TENNO";
        Player5Joined = true;
    }
    public void SetPlayer6Race()
    {
        Player6Race = "HUMAN";
        Player6Joined = true;
    }

    public void ResetRaces()
    {
        Player1Joined = false;
        Player2Joined = false;
        Player3Joined = false;
        Player4Joined = false;
        Player5Joined = false;
        Player6Joined = false;
    }
}
