using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerScoreList : MonoBehaviour {

    public GameObject playerScoreEntry; //Player Info Prefab

    ScoreManager scoreManager;  //Score Manager Class

    int lastChangeCounter;

	void Start ()
    {
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();

	}
	
	// Update is called once per frame
	void Update ()
    {
        //if there has not been a change to the scoreboard do nothing
        if(scoreManager.GetChangeCounter() == lastChangeCounter)
        {
            return;
        }

        lastChangeCounter = scoreManager.GetChangeCounter();

        //Set the position of entries
        while (this.transform.childCount > 0)
        {
            Transform c = this.transform.GetChild(0);
            c.SetParent(null);
            Destroy(c.gameObject);
        }

        string[] names = scoreManager.GetPlayerNames("money");  //Sort by money quantity

        //Set value of all entries to the values in the dictionary
        foreach (string name in names)
        {
            GameObject go = (GameObject)Instantiate(playerScoreEntry);
            go.transform.SetParent(this.transform, false);
            go.transform.Find("Race").GetComponent<Text>().text = name;
            go.transform.Find("Level").GetComponent<Text>().text = scoreManager.GetScore(name, "level").ToString();
            go.transform.Find("EXP").GetComponent<Text>().text = scoreManager.GetScore(name, "exp").ToString();
            go.transform.Find("Money").GetComponent<Text>().text = scoreManager.GetScore(name, "money").ToString();
            go.transform.Find("Territory").GetComponent<Text>().text = scoreManager.GetScore(name, "territory").ToString();
            go.transform.Find("Battalions").GetComponent<Text>().text = scoreManager.GetScore(name, "battalions").ToString();
        }
    }
}
