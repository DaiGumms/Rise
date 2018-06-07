using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    Dictionary<string, Dictionary<string, int>> playerScores;   //Dictionary of scores

    int changeCounter = 0;  //Record the number of changes

	void Start ()
    {
        // Set the values for every race to 0
        foreach(string race in GameManager.instance.races)
        {
            print("Setting Leaderboard values to 0");

            SetScore(race, "level", 0);
            SetScore(race, "exp", 0);
            SetScore(race, "money", 0);
            SetScore(race, "territory", 0);
            SetScore(race, "battalions", 0);
        }

        //If the game has been loaded restore values from previous games
        if (GameManager.instance.gameBegins == false)
        {
            CountryManager.instance.RestoreLeaderboard();
        }
    }

    //Initialise Dictionary
    void Init()
    {
        if (playerScores != null)
            return;

        playerScores = new Dictionary<string, Dictionary<string, int>>();
    }
	
    //Get a value from the Dictionary
	public int GetScore(string race, string scoreType)
    {
        Init();

        if(playerScores.ContainsKey(race) == false)
        {
            return 0;
        }

        if(playerScores[race].ContainsKey(scoreType) == false)
        {
            return 0;
        }

        return playerScores[race][scoreType];
    }

    //Set a value in the Dictionary
    public void SetScore(string race, string scoreType, int value)
    {
        Init();

        changeCounter++;

        if(playerScores.ContainsKey(race) == false)
        {
            playerScores[race] = new Dictionary<string, int>();
        }

        playerScores[race][scoreType] = value;
    }

    //Change a value in the Dictionary by a given amount
    public void ChangeScore(string race, string scoreType, int amount)
    {
        Init();

        int currScore = GetScore(race, scoreType);
        SetScore(race, scoreType, currScore + amount);
    }

    //Retrieve player names
    public string[] GetPlayerNames()
    {
        Init();

        return playerScores.Keys.ToArray();
    }

    //Retrieve player names ordered by a specific score type
    public string[] GetPlayerNames(string sortingScoreType)
    {
        Init();

        return playerScores.Keys.OrderByDescending(n => GetScore(n, sortingScoreType)).ToArray();
    }

    //Get the value of the changecounter
    public int GetChangeCounter()
    {
        return changeCounter;
    }
}
