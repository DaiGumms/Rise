using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public GameObject raceInfo;

    public Vector3 attackFromPosition;
    public Vector3 attackedTerritoryPosition;

    public string attackedTerritory;    //Name of territory selected for attack
    public string attackFrom;           //Name of territory selected to attack from
    public string attackFromArea;       //Name of area attacking from
    public string selectedTerritory;    //Name of territory selected at any stage of the game
    public string setUpTerritory;
    public string currentRace;          //Race being played during SetUp
    public string CURRENTRACE;          //Race being played

    public string fortifyFrom;          //Name of territory that battalions are being moved from
    public string fortifyTo;            //Name of territory that battalions are being moved to

    public bool gameActive;             //Is the game active?

    public bool battleHasEnded;         //Has a battle finished?
    public bool battleWon;              //Has the battle been won by the aggressor?
    public bool SetUpDone;              //Has the SetUp Stage finished?
    public bool SetUpClickValid;
    public bool Click;
    public bool DraftDone;              //Has the Draft Stage finished?
    public bool AttackDone;             //Has the Attack Stage finished?
    public bool FortifyDone;            //Has the Fortify Stage finished?

    public bool gameBegins;             //Has the game only just begun

    public int current_cycle_setup;

    public int exp;
    public int money;
    public int level;
    public int territory;
    public int battalions;

    public int exp_AVIAN;                     
    public int money_AVIAN;                   
    public int level_AVIAN;
    public int battalions_AVIAN;
    public int territories_AVIAN;
    public int exp_VORTA;
    public int money_VORTA;
    public int level_VORTA;
    public int battalions_VORTA;
    public int territories_VORTA;
    public int exp_KABAL;
    public int money_KABAL;
    public int level_KABAL;
    public int battalions_KABAL;
    public int territories_KABAL;
    public int exp_EVREI;
    public int money_EVREI;
    public int level_EVREI;
    public int battalions_EVREI;
    public int territories_EVREI;
    public int exp_TENNO;
    public int money_TENNO;
    public int level_TENNO;
    public int battalions_TENNO;
    public int territories_TENNO;
    public int exp_HUMAN;
    public int money_HUMAN;
    public int level_HUMAN;
    public int battalions_HUMAN;
    public int territories_HUMAN;

    //Array of races as a string
    public List<string> races = new List<string> { //List of Races in the game
            //"AVIAN",
            //"VORTA",
            //"KABAL",
            //"EVREI",
            //"TENNO",
            //"HUMAN"
    };

    int skip_amount;

    //Saved data
    [System.Serializable]
    public class SaveData
    {
        public List<Country> savedCountries = new List<Country>();  //List of territories and their properties
        public List<string> playingRaces = new List<string>();  //List of races that are playing

        //money, exp and level count for each race
        public int cur_exp_AVIAN;
        public int cur_money_AVIAN;
        public int cur_level_AVIAN;
        public int cur_battalions_AVIAN;
        public int cur_territories_AVIAN;
        public int cur_exp_VORTA;
        public int cur_money_VORTA;
        public int cur_level_VORTA;
        public int cur_battalions_VORTA;
        public int cur_territories_VORTA;
        public int cur_exp_KABAL;
        public int cur_money_KABAL;
        public int cur_level_KABAL;
        public int cur_battalions_KABAL;
        public int cur_territories_KABAL;
        public int cur_exp_EVREI;
        public int cur_money_EVREI;
        public int cur_level_EVREI;
        public int cur_battalions_EVREI;
        public int cur_territories_EVREI;
        public int cur_exp_TENNO;
        public int cur_money_TENNO;
        public int cur_level_TENNO;
        public int cur_battalions_TENNO;
        public int cur_territories_TENNO;
        public int cur_exp_HUMAN;
        public int cur_money_HUMAN;
        public int cur_level_HUMAN;
        public int cur_battalions_HUMAN;
        public int cur_territories_HUMAN;

        public string cur_race; //Current race playing
    }

    //On Awake
    private void Awake()
    {

        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        //Dont destroy objects
        DontDestroyOnLoad(gameObject);
        gameBegins = true;
        gameActive = true;
    }
    
    //SetUp Stage
    public IEnumerator SetUp(bool wait)
    {

        print("Starting SetUp");

        if (SetUpDone == false) //If the SetUp stage has not finished
        {
            CountryManager.instance.DisableGUI();
            print("Start Loop!");
            for (current_cycle_setup = 1; current_cycle_setup <= 22; current_cycle_setup++)    //Main loop for SetUp
            {
                foreach (string owner in races) //For each race playing
                {
                    CountryManager.instance.ShowSetUpPanel(owner, current_cycle_setup);  //Display the race that is playing
                    wait = true;
                    SetUpClickValid = false;
                    print("Waiting for Input...");
                    while (wait)    //Wait for input
                    {
                        if (Input.GetMouseButtonDown(0))    //If a click is registered
                        {
                            Click = false;
                            while (Click == false)  //Wait for selecting territory to be set
                            {
                                yield return null;
                            }

                            print("Click Registered!");

                            //Set currentRace
                            if (owner == "AVIAN")
                            {
                                currentRace = "AVIAN";
                                CountryManager.instance.SelectTerritory();

                                if (SetUpClickValid == true)
                                {
                                    wait = false;
                                }
                            }

                            if (owner == "VORTA")
                            {
                                currentRace = "VORTA";
                                CountryManager.instance.SelectTerritory();

                                if (SetUpClickValid == true)
                                {
                                    wait = false;
                                }
                            }
                            if (owner == "KABAL")
                            {
                                currentRace = "KABAL";
                                CountryManager.instance.SelectTerritory();

                                if (SetUpClickValid == true)
                                {
                                    wait = false;
                                }
                            }
                            if (owner == "EVREI")
                            {
                                currentRace = "EVREI";
                                CountryManager.instance.SelectTerritory();

                                if (SetUpClickValid == true)
                                {
                                    wait = false;
                                }
                            }
                            if (owner == "TENNO")
                            {
                                currentRace = "TENNO";
                                CountryManager.instance.SelectTerritory();

                                if (SetUpClickValid == true)
                                {
                                    wait = false;
                                }
                            }
                            if (owner == "HUMAN")
                            {
                                currentRace = "HUMAN";
                                CountryManager.instance.SelectTerritory();

                                if (SetUpClickValid == true)
                                {
                                    wait = false;
                                }
                            }
                            print("Cycle: " + current_cycle_setup);
                        }
                    yield return null;
                    }
                }

                if (current_cycle_setup == 22) //If loop is completed
                {
                    print("Loop Completed!!!");
                    SetUpDone = true;
                    Saving();   //Save Game
                }
            }               
        }
        print("SetUp Finished!");
        CountryManager.instance.DisableSetUpPanel();
        CountryManager.instance.ownedTerritories = 0;
        gameActive = true;  //The main game phase is active
        StartCoroutine(Turn()); //Start the main game
    }

    //Thread for the main game loop
    public IEnumerator Turn()
    {
        print("Starting Turn Thread.");
        int i;

        RaceSelection info = raceInfo.GetComponent<RaceSelection>();

        CountryManager.instance.RestoreLeaderboard();

        for (i = 1; i <= 1000; i++)
        {
            for (int j = 0; j < races.Count; j++)  //For each race playing
            {
                //Skip over turns until the last played player is found
                if (races[j] != CURRENTRACE && i == 1 && CountryManager.instance.turnOver == false)
                    continue;

                print("Starting turn for " + races[j]);
                print("turnover state: " + CountryManager.instance.turnOver);

                CURRENTRACE = races[j]; //Set the playing race to be used in other classes

                if(CURRENTRACE == "AVIAN" && info.Player1Joined)
                {
                    exp = exp_AVIAN;
                    money = money_AVIAN;
                    level = level_AVIAN;
                    territory = territories_AVIAN;
                    battalions = battalions_AVIAN;
                    print("Info loaded for AVIAN");
                    CountryManager.instance.turnOver = false;
                }
                if (CURRENTRACE == "VORTA" && info.Player2Joined)
                {
                    exp = exp_VORTA;
                    money = money_VORTA;
                    level = level_VORTA;
                    territory = territories_VORTA;
                    battalions = battalions_VORTA;
                    print("Info loaded for VORTA");
                    CountryManager.instance.turnOver = false;
                }
                if (CURRENTRACE == "KABAL" && info.Player3Joined)
                {
                    exp = exp_KABAL;
                    money = money_KABAL;
                    level = level_KABAL;
                    territory = territories_KABAL;
                    battalions = battalions_KABAL;
                    print("Info loaded for KABAL");
                    CountryManager.instance.turnOver = false;
                }
                if (CURRENTRACE == "EVREI" && info.Player4Joined)
                {
                    exp = exp_EVREI;
                    money = money_EVREI;
                    level = level_EVREI;
                    territory = territories_EVREI;
                    battalions = battalions_EVREI;
                    print("Info loaded for EVREI");
                    CountryManager.instance.turnOver = false;
                }
                if (CURRENTRACE == "TENNO" && info.Player5Joined)
                {
                    exp = exp_TENNO;
                    money = money_TENNO;
                    level = level_TENNO;
                    territory = territories_TENNO;
                    battalions = battalions_TENNO;
                    print("Info loaded for TENNO");
                    CountryManager.instance.turnOver = false;
                }
                if (CURRENTRACE == "HUMAN" && info.Player6Joined)
                {
                    exp = exp_HUMAN;
                    money = money_HUMAN;
                    level = level_HUMAN;
                    territory = territories_HUMAN;
                    battalions = battalions_HUMAN;
                    print("Info loaded for HUMAN");
                    CountryManager.instance.turnOver = false;
                }

                CountryManager.instance.ShowGUI(CURRENTRACE, money, exp, level);    //Display progress on screen

                if(DraftDone == false)  //if in the draft phase
                {
                    CountryManager.instance.CalculateOwnedTerritories();
                    CountryManager.instance.ShowDraftPanel();
                }
              
                print("Waiting for end of turn...");
                //Wait for turn to end
                while (CountryManager.instance.turnOver == false)
                {
                    yield return null;
                }

                if (CURRENTRACE == "AVIAN")
                {
                    exp_AVIAN = exp;
                    money_AVIAN = money;
                    level_AVIAN = level;
                    territories_AVIAN = territory;
                    battalions_AVIAN = battalions;
                    print("Saved AVIAN info");
                }
                if (CURRENTRACE == "VORTA")
                {
                    exp_VORTA = exp;
                    money_VORTA = money;
                    level_VORTA = level;
                    territories_VORTA = territory;
                    battalions_VORTA = battalions;
                    print("Saved VORTA info");
                }
                if (CURRENTRACE == "KABAL")
                {
                    exp_KABAL = exp;
                    money_KABAL = money;
                    level_KABAL = level;
                    territories_KABAL = territory;
                    battalions_KABAL = battalions;
                    print("Saved KABAL info");
                }
                if (CURRENTRACE == "EVREI")
                {
                    exp_EVREI = exp;
                    money_EVREI = money;
                    level_EVREI = level;
                    territories_EVREI = territory;
                    battalions_EVREI = battalions;
                    print("Saved EVREI info");
                }
                if (CURRENTRACE == "TENNO")
                {
                    exp_TENNO = exp;
                    money_TENNO = money;
                    level_TENNO = level;
                    territories_TENNO = territory;
                    battalions_TENNO = battalions;
                    print("Saved TENNO info");
                }
                if (CURRENTRACE == "HUMAN")
                {
                    exp_HUMAN = exp;
                    money_HUMAN = money;
                    level_HUMAN = level;
                    territories_HUMAN = territory;
                    battalions_HUMAN = battalions;
                    print("Saved HUMAN info");
                }

                print("Turn number: " + i + " complete for " + CURRENTRACE);
            }
            if(gameActive == true)
            {
                print("Game is still active: " + gameActive);

            }
            //If game has ended
            if(gameActive == false)
            {
                i = 1000;

                print("GAME OVER!!!");
            }
        }
    }

    //Set the races playing
    public void AddRace()
    {
        string Player1_race;
        string Player2_race;
        string Player3_race;
        string Player4_race;
        string Player5_race;
        string Player6_race;

        print("Adding Races");

        //Reference to RaceSelection script
        RaceSelection race = raceInfo.GetComponent<RaceSelection>();

        Player1_race = race.Player1Race;    //Set to name of player's race in RaceSelection
        if(Player1_race != null && race.Player1Joined)  //Check both name and boolean
        {
            races.Add(Player1_race);    //Add to list of races playing
            print("Player 1 in the game");
            print("Player 1 playing as: " + Player1_race);
        }
        Player2_race = race.Player2Race;
        if (Player2_race != null && race.Player2Joined)
        {
            races.Add(Player2_race);    //Add to list of races playing
            print("Player 2 in the game");
            print("Player 2 playing as: " + Player2_race);
        }
        Player3_race = race.Player3Race;
        if (Player3_race != null && race.Player3Joined)
        {
            races.Add(Player3_race);
            print("Player 3 in the game");
            print("Player 3 playing as: " + Player3_race);
        }
        Player4_race = race.Player4Race;
        if (Player4_race != null && race.Player4Joined)
        {
            races.Add(Player4_race);
            print("Player 4 in the game");
            print("Player 4 playing as: " + Player4_race);
        }
        Player5_race = race.Player5Race;
        if (Player5_race != null && race.Player5Joined)
        {
            races.Add(Player5_race);
            print("Player 5 in the game");
            print("Player 5 playing as: " + Player5_race);
        }
        Player6_race = race.Player6Race;
        if (Player1_race != null && race.Player6Joined)
        {
            races.Add(Player6_race);
            print("Player 6 in the game");
            print("Player 6 playing as: " + Player6_race);
        }

        print("Races Added!");
    }

    public void RemoveRace()
    {
        print("Checking if a player has been eliminated");

        int count = 0;

        foreach (string race in races)
        {
            print("Checking " + race);
            count = 0;
            foreach (GameObject owned in CountryManager.instance.territoryList)
            {
                CountryHandler handler = owned.GetComponent
                    <CountryHandler>();

                if (handler.country.race == race)
                {
                    print("Owned territory found");
                    count++;
                }
            }

            if (count == 0)
            {
                RaceSelection info = raceInfo.GetComponent<RaceSelection>();

                print("Removing: " + race + " they have been eliminated.");

                if (race == "AVIAN")
                {
                    info.Player1Joined = false;

                    print(race + "has been removed");
                }
                if (race == "VORTA")
                {
                    info.Player2Joined = false;

                    print(race + "has been removed");
                }
                if (race == "KABAL")
                {
                    info.Player3Joined = false;

                    print(race + "has been removed");
                }
                if (race == "EVREI")
                {
                    info.Player4Joined = false;

                    print(race + "has been removed");
                }
                if (race == "TENNO")
                {
                    info.Player5Joined = false;

                    print(race + "has been removed");
                }
                if (race == "HUMAN")
                {
                    info.Player6Joined = false;

                    print(race + "has been removed");
                }
            }
            print("Territories owned by " + race + " = " + count);
        }
    }


    

    //Save the game state
    public void Saving()
    {
        print("Saving Game");
        if (File.Exists(Application.persistentDataPath + "/SaveFile.dat"))
        {
            File.Delete(Application.persistentDataPath + "/SaveFile.dat");
            print("Save File Deleted");
        }

        SaveData data = new SaveData();
        for(int i = 0; i < CountryManager.instance.territoryList.Count; i++)
        {
            data.savedCountries.Add(CountryManager.instance.territoryList[i]
                .GetComponent<CountryHandler>().country);
        }

        //Store progress values
        for (int j = 0; j < races.Count; j++)
        {
            data.playingRaces.Add(races[j]);
            if(races[j] == "AVIAN")
            {
                data.cur_exp_AVIAN = CountryManager.instance.SaveGetScore("AVIAN", "exp");
                data.cur_money_AVIAN = CountryManager.instance.SaveGetScore("AVIAN", "money");
                data.cur_level_AVIAN = CountryManager.instance.SaveGetScore("AVIAN", "level");
                data.cur_battalions_AVIAN = CountryManager.instance.SaveGetScore("AVIAN", "battalions");
                data.cur_territories_AVIAN = CountryManager.instance.SaveGetScore("AVIAN", "territory");
            }
            else if (races[j] == "VORTA")
            {               
                data.cur_exp_VORTA = CountryManager.instance.SaveGetScore("VORTA", "exp");
                data.cur_money_VORTA = CountryManager.instance.SaveGetScore("VORTA", "money");
                data.cur_level_VORTA = CountryManager.instance.SaveGetScore("VORTA", "level");
                data.cur_battalions_VORTA = CountryManager.instance.SaveGetScore("VORTA", "battalions");
                data.cur_territories_VORTA = CountryManager.instance.SaveGetScore("VORTA", "territory");
            }
            else if (races[j] == "KABAL")
            {               
                data.cur_exp_KABAL = CountryManager.instance.SaveGetScore("KABAL", "exp");
                data.cur_money_KABAL = CountryManager.instance.SaveGetScore("KABAL", "money");
                data.cur_level_KABAL = CountryManager.instance.SaveGetScore("KABAL", "level");
                data.cur_battalions_KABAL = CountryManager.instance.SaveGetScore("KABAL", "battalions");
                data.cur_territories_KABAL = CountryManager.instance.SaveGetScore("KABAL", "territory");
            }
            else if (races[j] == "EVREI")
            {               
                data.cur_exp_EVREI = CountryManager.instance.SaveGetScore("EVREI", "exp");
                data.cur_money_EVREI = CountryManager.instance.SaveGetScore("EVREI", "money");
                data.cur_level_EVREI = CountryManager.instance.SaveGetScore("EVREI", "level");
                data.cur_battalions_EVREI = CountryManager.instance.SaveGetScore("EVREI", "battalions");
                data.cur_territories_EVREI = CountryManager.instance.SaveGetScore("EVREI", "territory");
            }
            else if (races[j] == "TENNO")
            {
                data.cur_exp_TENNO = CountryManager.instance.SaveGetScore("TENNO", "exp");
                data.cur_money_TENNO = CountryManager.instance.SaveGetScore("TENNO", "money");
                data.cur_level_TENNO = CountryManager.instance.SaveGetScore("TENNO", "level");
                data.cur_battalions_TENNO = CountryManager.instance.SaveGetScore("TENNO", "battalions");
                data.cur_territories_TENNO = CountryManager.instance.SaveGetScore("TENNO", "territory");
            }
            else if (races[j] == "HUMAN")
            {                
                data.cur_exp_HUMAN = CountryManager.instance.SaveGetScore("HUMAN", "exp");
                data.cur_money_HUMAN = CountryManager.instance.SaveGetScore("HUMAN", "money");
                data.cur_level_HUMAN = CountryManager.instance.SaveGetScore("HUMAN", "level");
                data.cur_battalions_HUMAN = CountryManager.instance.SaveGetScore("HUMAN", "battalions");
                data.cur_territories_HUMAN = CountryManager.instance.SaveGetScore("HUMAN", "territory");
            }
        }

        data.cur_race = CURRENTRACE;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + 
            "/SaveFile.dat", FileMode.Create);

        bf.Serialize(stream, data);
        stream.Close();
        print("Saved Game!");
        //Loading();
    }

    //Load the saved game state
    public void Loading()
    {
        RaceSelection info = raceInfo.GetComponent<RaceSelection>();
        print("Loading Game");
        print("Game Beginning: " + gameBegins);
        //If a file exists
        if(File.Exists(Application.persistentDataPath + "/SaveFile.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + 
                "/SaveFile.dat", FileMode.Open);

            SaveData data = (SaveData)bf.Deserialize(stream);
            stream.Close();

            //For all saved Territories
            for(int i = 0; i < data.savedCountries.Count; i++)
            {
                //For all Territories to fill
                for(int j = 0; j < CountryManager.instance.territoryList.Count; j++)
                {
                    if (data.savedCountries[i].name == CountryManager.instance.
                        territoryList[j].GetComponent<CountryHandler>().country.name)
                    {
                        CountryManager.instance.territoryList[j].
                            GetComponent<CountryHandler>().country = data.savedCountries[i];
                    }
                }
            }

            //Restore progress values
            for (int j = 0; j < data.playingRaces.Count; j++)
            {
                if (data.playingRaces[j] == "AVIAN")
                {
                    info.Player1Joined = true;
                    exp_AVIAN = data.cur_exp_AVIAN;
                    money_AVIAN = data.cur_money_AVIAN;
                    level_AVIAN = data.cur_level_AVIAN;
                    battalions_AVIAN = data.cur_battalions_AVIAN;
                    territories_AVIAN = data.cur_territories_AVIAN;
                }
                if (data.playingRaces[j] == "VORTA")
                {
                    info.Player2Joined = true;
                    exp_VORTA = data.cur_exp_VORTA;
                    money_VORTA = data.cur_money_VORTA;
                    level_VORTA = data.cur_level_VORTA;
                    battalions_VORTA = data.cur_battalions_VORTA;
                    territories_VORTA = data.cur_territories_VORTA;
                }
                if (data.playingRaces[j] == "KABAL")
                {
                    info.Player3Joined = true;
                    exp_KABAL = data.cur_exp_KABAL;
                    money_KABAL = data.cur_money_KABAL;
                    level_KABAL = data.cur_level_KABAL;
                    battalions_KABAL = data.cur_battalions_KABAL;
                    territories_KABAL = data.cur_territories_KABAL;
                }
                if (data.playingRaces[j] == "EVREI")
                {
                    info.Player4Joined = true;
                    exp_EVREI = data.cur_exp_EVREI;
                    money_EVREI = data.cur_money_EVREI;
                    level_EVREI = data.cur_level_EVREI;
                    battalions_EVREI = data.cur_battalions_EVREI;
                    territories_EVREI = data.cur_territories_EVREI;
                }
                if (data.playingRaces[j] == "TENNO")
                {
                    info.Player5Joined = true;
                    exp_TENNO = data.cur_exp_TENNO;
                    money_TENNO = data.cur_money_TENNO;
                    level_TENNO = data.cur_level_TENNO;
                    battalions_TENNO = data.cur_battalions_TENNO;
                    territories_TENNO = data.cur_territories_TENNO;
                }
                if (data.playingRaces[j] == "HUMAN")
                {
                    info.Player6Joined = true;
                    exp_HUMAN = data.cur_exp_HUMAN;
                    money_HUMAN = data.cur_money_HUMAN;
                    level_HUMAN = data.cur_level_HUMAN;
                    battalions_HUMAN = data.cur_battalions_HUMAN;
                    territories_HUMAN = data.cur_territories_HUMAN;
                }
            }

            CURRENTRACE = data.cur_race;

            SetUpDone = true;
            CountryManager.instance.TintTerritories();
            print("Game Loaded!");

            for (int j = 0; j < data.playingRaces.Count; j++)
            {
                races.Add(data.playingRaces[j]);
            }

            gameBegins = false;

            StartCoroutine(Turn());

            //If a file does not exist
        } else
        {
            print("No Save File found. Starting SetUp.");
            AddRace();
            SetUpDone = false;
            StartCoroutine (SetUp(true));
            if (SetUpDone == true)
            {
                CountryManager.instance.TintTerritories();
            }
        }
    }

    //Delete the saved file
    public void DeleteSaveFile()
    {
        print("Deleting save file");
        if (File.Exists(Application.persistentDataPath + "/SaveFile.dat"))
        {
            File.Delete(Application.persistentDataPath + "/SaveFile.dat");
            print("Save File Deleted");

            //Reset progress
            for (int j = 0; j < races.Count; j++)
            {
                if (races[j] == "AVIAN")
                {
                    exp_AVIAN = 0;
                    money_AVIAN = 0;
                    level_AVIAN = 0;
                    battalions_AVIAN = 0;
                    territories_AVIAN = 0;
                }
                if (races[j] == "VORTA")
                {
                    exp_VORTA = 0;
                    money_VORTA = 0;
                    level_VORTA = 0;
                    battalions_VORTA = 0;
                    territories_VORTA = 0;
                }
                if (races[j] == "KABAL")
                {
                    exp_KABAL = 0;
                    money_KABAL = 0;
                    level_KABAL = 0;
                    battalions_KABAL = 0;
                    territories_KABAL = 0;
                }
                if (races[j] == "EVREI")
                {
                    exp_EVREI = 0;
                    money_EVREI = 0;
                    level_EVREI = 0;
                    battalions_EVREI = 0;
                    territories_EVREI = 0;
                }
                if (races[j] == "TENNO")
                {
                    exp_TENNO = 0;
                    money_TENNO = 0;
                    level_TENNO = 0;
                    battalions_TENNO = 0;
                    territories_TENNO = 0;
                }
                if (races[j] == "HUMAN")
                {
                    exp_HUMAN = 0;
                    money_HUMAN = 0;
                    level_HUMAN = 0;
                    battalions_HUMAN = 0;
                    territories_HUMAN = 0;
                }
            }

            CountryManager.instance.ownedTerritories = 0;

            battleHasEnded = false;
            battleWon = false;
        }
        else
        {
            print("No File Exists!");
        }
    }

}
