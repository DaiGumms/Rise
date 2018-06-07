using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FightSim : MonoBehaviour {

    ScoreManager scoreManager;

    int roll2_1;
    int roll2_2;
    int roll2_3;
    int roll1_1;
    int roll1_2;
    int roll1_3;

    List<int> attacker = new List<int>();
    List<int> defender = new List<int>();

    private void Awake()
    {
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();
    }

    //Simulate battle
    public void StartFight()
    {

        print("Simulating Battle");

        CountryHandler def = GameObject.Find(GameManager.instance.attackedTerritory)
                .GetComponent<CountryHandler>();

        CountryHandler att = GameObject.Find(GameManager.instance.attackFrom)
              .GetComponent<CountryHandler>();

        CountryHandler count = GameObject.Find(GameManager.instance.attackedTerritory)
                .GetComponent<CountryHandler>();

        int roll2_1 = 0;
        int roll2_2 = 0;
        int roll2_3 = 0;
        int roll1_1 = 0;
        int roll1_2 = 0;
        int roll1_3 = 0;

        int win_count = 0;

        if (att.country.battalions >= 3)
        {   //Attacker Roll

            print("Attacker rolls 3 die");

            roll2_1 = Random.Range(1, 7);
            attacker.Add(roll2_1);
            roll2_2 = Random.Range(1, 7);
            attacker.Add(roll2_2);
            roll2_3 = Random.Range(1, 7);
            attacker.Add(roll2_3);

            print("Attacker rolls: " + roll2_1 + roll2_2 + roll2_3);

            //Sort Attacker
            if (attacker[0] <= attacker[1] && attacker[1] <= attacker[2])
            {
                Swap(attacker, 0, 2);
            }
            else if (attacker[0] < attacker[1] && attacker[1] > attacker[2] && attacker[0] < attacker[2])
            {
                Swap(attacker, 0, 1);
                Swap(attacker, 1, 2);
            }
            else if (attacker[0] > attacker[1] && attacker[1] < attacker[2] && attacker[0] < attacker[2])
            {
                Swap(attacker, 1, 2);
                Swap(attacker, 0, 1);
            }
            else if (attacker[0] < attacker[1] && attacker[1] > attacker[2] && attacker[0] >= attacker[2])
            {
                Swap(attacker, 0, 1);
            }
            else if (attacker[0] > attacker[1] && attacker[1] < attacker[2] && attacker[0] >= attacker[2])
            {
                Swap(attacker, 1, 2);
            }

            roll2_1 = attacker[0];
            roll2_2 = attacker[1];
            roll2_3 = attacker[2];

            print("Attacker's sorted roll: " + roll2_1 + roll2_2 + roll2_3);
        }

        else if (att.country.battalions == 2)
        {

            print("Attacker rolls 2 die");

            //Attacker Roll
            roll2_1 = Random.Range(1, 7);
            attacker.Add(roll2_1);
            roll2_2 = Random.Range(1, 7);
            attacker.Add(roll2_2);

            print("Attacker rolls: " + roll2_1 + roll2_2);

            //Sort Attacker
            if (attacker[0] < attacker[1])
            {
                Swap(attacker, 0, 1);
            }

            roll2_1 = attacker[0];
            roll2_2 = attacker[1];

            print("Attacker's sorted roll: " + roll2_1 + roll2_2);
        }

        if (def.country.battalions >= 3)
        {

            print("Defender rolls 3 die");

            //Defender Roll
            roll1_1 = Random.Range(1, 7);
            defender.Add(roll1_1);
            roll1_2 = Random.Range(1, 7);
            defender.Add(roll1_2);
            roll1_3 = Random.Range(1, 7);
            defender.Add(roll1_3);

            print("Defender rolls: " + roll1_1 + roll1_2 + roll1_3);

            //Sort Defender
            if (defender[0] <= defender[1] && defender[1] <= defender[2])
            {
                Swap(defender, 0, 2);
            }
            else if (defender[0] < defender[1] && defender[1] > defender[2] && defender[0] < defender[2])
            {
                Swap(defender, 0, 1);
                Swap(defender, 1, 2);
            }
            else if (defender[0] > defender[1] && defender[1] < defender[2] && defender[0] < defender[2])
            {
                Swap(defender, 1, 2);
                Swap(defender, 0, 1);
            }
            else if (defender[0] < defender[1] && defender[1] > defender[2] && defender[0] >= defender[2])
            {
                Swap(defender, 0, 1);
            }
            else if (defender[0] > defender[1] && defender[1] < defender[2] && defender[0] >= defender[2])
            {
                Swap(defender, 1, 2);
            }

            roll1_1 = defender[0];
            roll1_2 = defender[1];
            roll1_3 = defender[2];

            print("Defender's sorted roll: " + roll1_1 + roll1_2 + roll1_3);
        }

        else if (def.country.battalions == 2)
        {

            print("Defender rolls 2 die");

            //Defender Roll
            roll1_1 = Random.Range(1, 7);
            defender.Add(roll1_1);
            roll1_2 = Random.Range(1, 7);
            defender.Add(roll1_2);

            print("Defender rolls: " + roll1_1 + roll1_2);


            //Sort Defender
            if (defender[0] < defender[1])
            {
                Swap(defender, 0, 1);
            }

            roll1_1 = defender[0];
            roll1_2 = defender[1];

            print("Defender's sorted roll: " + roll1_1 + roll1_2);
        }

        else if (def.country.battalions == 1)
        {

            print("Defender rolls 1 dice");

            //Defender Roll
            roll1_1 = Random.Range(1, 7);
            defender.Add(roll1_1);

            print("Defender rolls: " + roll1_1);
        }

        string result;

        print("Battle Simulated");

        if (roll2_1 > roll1_1)
        {
            //Attacker wins
            //battleWon = false;
            win_count++;

            def.country.battalions--;

            scoreManager.ChangeScore(count.country.race, "battalions", -1);
        }
        else if (roll1_1 >= roll2_1)
        {
            //Attacker looses
            //battleWon = false;

            att.country.battalions--;

            scoreManager.ChangeScore(GameManager.instance.CURRENTRACE, "battalions", -1);
        }
        if (roll2_2 > roll1_2 && roll1_2 != 0)
        {
            //Attacker wins
            //battleWon = false;
            win_count++;

            def.country.battalions--;

            scoreManager.ChangeScore(count.country.race, "battalions", -1);
        }
        else if (roll1_2 >= roll2_2 && roll1_2 != 0)
        {
            //Attacker looses
            //battleWon = false;

            att.country.battalions--;

            scoreManager.ChangeScore(GameManager.instance.CURRENTRACE, "battalions", -1);
        }
        if (roll2_3 > roll1_3 && roll1_3 != 0)
        {
            //Attacker wins
            //battleWon = false;
            win_count++;

            def.country.battalions--;

            scoreManager.ChangeScore(count.country.race, "battalions", -1);
        }
        else if (roll1_3 >= roll2_3 && roll1_3 != 0)
        {
            //Attacker looses
            //battleWon = false;

            att.country.battalions--;

            scoreManager.ChangeScore(GameManager.instance.CURRENTRACE, "battalions", -1);
        }

        result = "You have won " + win_count + " battles!";

            attacker = new List<int>();

            defender = new List<int>();

        //Show the results of the battle
        CountryManager.instance.ShowBattlePanel(GameManager.instance.attackedTerritory + " is defending!!!", roll1_1, roll2_1, roll1_2, roll2_2, roll1_3, roll2_3, result);

        //battleHasEnded = true;
    }

    //Swap the order of values
    static void Swap(IList<int> list, int indexA, int indexB)
    {
        print("Swapping");
        int tmp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = tmp;
    }
}
