using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CountryManager : MonoBehaviour {

    public static CountryManager instance;

    public GameObject attackPanel;          //Reference to the AttackPanel
    public GameObject battlePanel;          //Reference to the BattlePanel
    public GameObject battalionsPanel;      //Reference to the BattalionsPanel
    public GameObject GUI;                  //Reference to the main GUI
    public GameObject setUpPanel;           //Reference to the SetUpPanel
    public GameObject draftPanel;           //Reference to the DarftPanel
    public GameObject attackPhasePanel;     //Reference to the AttackPhasePanel
    public GameObject fortifyPhasePanel;    //Reference to the FortifyPhasePanel
    public GameObject gameOverPanel;        //Reference to the GameOver screen
    public GameObject Menu;                 //Reference to the Menu
    public GameObject _Map;                 //Reference to the Map
    public GameObject fortifySliderParent;
    public Slider fortifySlider;            //Reference to the Fortify Stage Slider
    public GameObject attackSliderParent;
    public Slider attackSlider;             //Reference to the Attack Stage Slider
    public GameObject camZoom;              //Reference to the Camera zoom script through a game object

    public List<GameObject> territoryList = new List<GameObject>(); //List of all territories containing thier info
    public List<GameObject> ownedList = new List<GameObject>();     //List of owned territories by active player

    public bool selectingFrom;
    public bool selectingTo;
    public bool turnOver;

    public int baseBattalionAmount;         //Battalions given at the start of every turn
    public int placedBattalions;            //Number of battalions placed in a single Draft Phase
    public int draftBattalions;             //Number of total battalions to draft in a single Draft Phase
    public int remainingBattalions;         //Remaining number of battalions to draft
    public int boughtBattalions;            //Number of total battalions purchased in the store

    public int fortifyClickCount;
    public int attackClickCount;
    public int fortify_amount;
    public int attack_amount;
    public int winCount;

    public int ownedTerritories;        //Number of owned territories by the currenet race
    public int homeOwnedTerritories;    //Number of home territories owned by the currenet race
    public int capitalOwnedTerritories; //Number of capitals owned by the currenet race
    public int areaOwnedBonus;

    ScoreManager scoreManager;  //Reference to the ScoreManager Script

    void Awake()
    {
        instance = this;

        scoreManager = GameObject.FindObjectOfType<ScoreManager>(); //Find the ScoreManager script
    }

    private void Update()
    {
        //info of fortify stage slider
        if (fortifySliderParent.activeInHierarchy)
        {
            FortifyPanel fort = fortifyPhasePanel.GetComponent<FortifyPanel>();

            fortify_amount = Mathf.RoundToInt(fortifySlider.value);

            fort.minValue.text = fortifySlider.minValue.ToString();
            fort.maxValue.text = fortifySlider.maxValue.ToString();
            fort.curramount.text = fortify_amount.ToString();
        }

        //info of attack stage slider
        if (attackSliderParent.activeInHierarchy)
        {
            AttackPhasePanel attack = attackPhasePanel.GetComponent<AttackPhasePanel>();

            attack_amount = Mathf.RoundToInt(attackSlider.value);

            attack.minValue.text = attackSlider.minValue.ToString();
            attack.maxValue.text = attackSlider.maxValue.ToString();
            attack.curramount.text = attack_amount.ToString();
        }

        //if a battle has just finished
        if (GameManager.instance.battleHasEnded)
        {
            CountryHandler count = GameObject.Find(GameManager.instance.attackedTerritory)
                .GetComponent<CountryHandler>();

            if (count.country.battalions <= 0)
            {
                //Change race of Territory
                print(GameManager.instance.CURRENTRACE + " has taken control of " + count.country.name);
                count.country.race = GameManager.instance.CURRENTRACE;

                //Add the territories exp reward to players total
                print(count.country.expReward + " exp has been rewarded");
                GameManager.instance.exp += count.country.expReward;
                scoreManager.ChangeScore(GameManager.instance.CURRENTRACE, "exp", count.country.expReward);

                //Add the territories money reward to the players total
                print(count.country.moneyReward + " money has been rewarded");
                GameManager.instance.money += count.country.moneyReward;
                scoreManager.ChangeScore(GameManager.instance.CURRENTRACE, "money", count.country.moneyReward);

                //Set the number of battalions in the territory
                count.country.battalions = 0;

                //Increment level
                if (GameManager.instance.exp >= 500)
                {
                    GameManager.instance.level = GameManager.instance.level + 1;
                    scoreManager.SetScore(GameManager.instance.CURRENTRACE, "level", GameManager.instance.level);

                    //Reset exp to 0
                    GameManager.instance.exp = 0;
                    scoreManager.SetScore(GameManager.instance.CURRENTRACE, "exp", 0);

                    print(GameManager.instance.CURRENTRACE + " has leveled up to " + GameManager.instance.level);
                }

                //update dictionary
                scoreManager.ChangeScore(count.country.race, "territory", -1);
                scoreManager.ChangeScore(GameManager.instance.CURRENTRACE, "territory", 1);

                //ResetRewards();

                TintTerritories();

                ShowGUI(GameManager.instance.CURRENTRACE, GameManager.instance.money, GameManager.instance.exp, GameManager.instance.level);

                CheckIfGameWon();

                AttackMoveBattalions();

            }

            GameManager.instance.battleHasEnded = false;
        }
    }

    void Start()
    {
        selectingFrom = true;
        selectingTo = false;

        //Make Sure the Map is active at the start of the game
        _Map.SetActive(true);
        //Make sure he number of battalions is showing
        battalionsPanel.SetActive(true);

        AddTerritoryData();

        TintTerritories();

        if (GameManager.instance.SetUpDone == true)
        {
            ShowGUI(GameManager.instance.CURRENTRACE, GameManager.instance.money, GameManager.instance.exp, GameManager.instance.level);
        }
    }

    //Store territory data
    void AddTerritoryData()
    {
        print("Adding Territory Data");
        GameObject[] theArray = GameObject.FindGameObjectsWithTag("Territory")
            as GameObject[];
        foreach (GameObject terr in theArray)
        {
            territoryList.Add(terr);
            print("Added: " + terr.name);
        }
        GameManager.instance.Loading();
    }

    //Colour the territories
    public void TintTerritories()
    {
        print("Tinting Territories");
        //For every territory in the territory list
        for (int i = 0; i < territoryList.Count; i++)
        {
            CountryHandler handler = territoryList[i].GetComponent
                <CountryHandler>();

            //Purple
            if (handler.country.race == "AVIAN")
            {
                handler.TintColor(new Color32(255, 0, 255, 100));
            }
            //Blue
            if (handler.country.race == "EVREI")
            {
                handler.TintColor(new Color32(0, 0, 255, 100));
            }
            //Green
            if (handler.country.race == "HUMAN")
            {
                handler.TintColor(new Color32(0, 255, 0, 100));
            }
            //Red
            if (handler.country.race == "KABAL")
            {
                handler.TintColor(new Color32(255, 0, 0, 100));
            }
            //Yellow
            if (handler.country.race == "TENNO")
            {
                handler.TintColor(new Color32(255, 255, 0, 100));
            }
            //Cyan
            if (handler.country.race == "VORTA")
            {
                handler.TintColor(new Color32(0, 255, 255, 100));
            }
        }
        print("Territories Tinted!");
    }

    //Calculate the number of owned territories
    public void CalculateOwnedTerritories()
    {
        int AURORA_count = 0;
        int GENESIS_count = 0;
        int TIBERIUS_count = 0;
        int XINDI_count = 0;
        int AETHER_count = 0;
        int PROMETHEUS_count = 0;

        print("Calculating Owned Territories");
        foreach (GameObject owned in territoryList)
        {
            CountryHandler handler = owned.GetComponent
                <CountryHandler>();

            if (handler.country.race == GameManager.instance.CURRENTRACE)
            {
                ownedList.Add(owned);
                ownedTerritories++;
            }
        }
        print("Owned Territories Calculated");

        //Calculate the number of home territories, capitals and complete areas owned
        foreach (GameObject home in ownedList)
        {
            CountryHandler owner = home.GetComponent
                <CountryHandler>();

            if (owner.country.homeRace == GameManager.instance.CURRENTRACE)
            {
                print(owner.country.name + " is a home territory");
                homeOwnedTerritories++;
            }
            if (owner.country.capital)
            {
                print(owner.country.name + " is a owned Capital");
                capitalOwnedTerritories++;
            }
            if (owner.country.area == "AURORA")
            {
                AURORA_count++;
                if (AURORA_count == 4)
                {
                    areaOwnedBonus = areaOwnedBonus + 2;
                }
            }
            if (owner.country.area == "GENESIS")
            {
                GENESIS_count++;
                if (GENESIS_count == 8)
                {
                    areaOwnedBonus = areaOwnedBonus + 5;
                }
            }
            if (owner.country.area == "TIBERIUS")
            {
                TIBERIUS_count++;
                if (TIBERIUS_count == 6)
                {
                    areaOwnedBonus = areaOwnedBonus + 3;
                }
            }
            if (owner.country.area == "XINDI")
            {
                XINDI_count++;
                if (XINDI_count == 4)
                {
                    areaOwnedBonus = areaOwnedBonus + 2;
                }
            }
            if (owner.country.area == "AETHER")
            {
                AETHER_count++;
                if (AETHER_count == 8)
                {
                    areaOwnedBonus = areaOwnedBonus + 5;
                }
            }
            if (owner.country.area == "PROMETHEUS")
            {
                PROMETHEUS_count++;
                if (PROMETHEUS_count == 12)
                {
                    areaOwnedBonus = areaOwnedBonus + 7;
                }
            }
        }

        scoreManager.SetScore(GameManager.instance.CURRENTRACE, "territory", ownedTerritories);

        CalculateBattalions();
        ownedList.Clear();

        ownedTerritories = 0;
        homeOwnedTerritories = 0;
        capitalOwnedTerritories = 0;
        areaOwnedBonus = 0;
    }

    //Calculate the number of battalions to draft
    public void CalculateBattalions()
    {
        int total_amount;
        int owned_amount;
        int home_amount;
        int capital_amount;
        int area_amount;

        print("Calculating Battalions to Draft");

        area_amount = areaOwnedBonus;
        print("Area Bonus: " + area_amount);
        owned_amount = ownedTerritories / 3;
        print("Owned Bonus: " + owned_amount);
        home_amount = homeOwnedTerritories / 3;
        print("Home owned Bonus: " + home_amount);
        capital_amount = capitalOwnedTerritories * 3;
        print("Capital owned Bonus: " + capital_amount);

        total_amount = owned_amount + home_amount + capital_amount + area_amount;
        print("Total Bonus: " + total_amount);

        draftBattalions = baseBattalionAmount + total_amount;
        remainingBattalions = draftBattalions;  //Set the amount
    }

    //Set the race of the selected territory in the SetUp phase
    public void SelectTerritory()
    {
        print("Changing!");
        CountryHandler select = GameObject.Find(GameManager.instance.setUpTerritory)
                .GetComponent<CountryHandler>();

        if (GameManager.instance.currentRace == "AVIAN")
        {
            if (select.country.race == "NULL")
            {
                select.country.race = "AVIAN";
                select.TintColor(new Color32(255, 0, 255, 70));
                select.country.battalions++;
                select.SetBattalions();
                print("Owner Set to AVIAN.");
                GameManager.instance.SetUpClickValid = true;

                scoreManager.ChangeScore(GameManager.instance.currentRace, "territory", 1);
                scoreManager.ChangeScore(GameManager.instance.currentRace, "battalions", 1);
            }
            else if (select.country.race == "AVIAN")
            {
                select.country.battalions++;
                select.SetBattalions();
                print("Extra battalion placed.");
                GameManager.instance.SetUpClickValid = true;
                scoreManager.ChangeScore(GameManager.instance.currentRace, "battalions", 1);
            }
        }
        if (GameManager.instance.currentRace == "VORTA")
        {
            if (select.country.race == "NULL")
            {
                select.country.race = "VORTA";
                select.TintColor(new Color32(0, 255, 255, 70));
                select.country.battalions++;
                select.SetBattalions();
                print("Owner Set to VORTA.");
                GameManager.instance.SetUpClickValid = true;

                scoreManager.ChangeScore(GameManager.instance.currentRace, "territory", 1);
                scoreManager.ChangeScore(GameManager.instance.currentRace, "battalions", 1);
            }
            else if (select.country.race == "VORTA")
            {
                select.country.battalions++;
                select.SetBattalions();
                print("Extra battalion placed.");
                GameManager.instance.SetUpClickValid = true;
                scoreManager.ChangeScore(GameManager.instance.currentRace, "battalions", 1);
            }
        }
        if (GameManager.instance.currentRace == "KABAL")
        {
            if (select.country.race == "NULL")
            {
                select.country.race = "KABAL";
                select.TintColor(new Color32(255, 0, 0, 70));
                select.country.battalions++;
                select.SetBattalions();
                print("Owner Set to KABAL.");
                GameManager.instance.SetUpClickValid = true;

                scoreManager.ChangeScore(GameManager.instance.currentRace, "territory", 1);
                scoreManager.ChangeScore(GameManager.instance.currentRace, "battalions", 1);
            }
            else if (select.country.race == "KABAL")
            {
                select.country.battalions++;
                select.SetBattalions();
                print("Extra battalion placed.");
                GameManager.instance.SetUpClickValid = true;
                scoreManager.ChangeScore(GameManager.instance.currentRace, "battalions", 1);
            }
        }
        if (GameManager.instance.currentRace == "EVREI")
        {
            if (select.country.race == "NULL")
            {
                select.country.race = "EVREI";
                select.TintColor(new Color32(0, 0, 255, 70));
                select.country.battalions++;
                select.SetBattalions();
                print("Owner Set to EVREI.");
                GameManager.instance.SetUpClickValid = true;

                scoreManager.ChangeScore(GameManager.instance.currentRace, "territory", 1);
                scoreManager.ChangeScore(GameManager.instance.currentRace, "battalions", 1);
            }
            else if (select.country.race == "EVREI")
            {
                select.country.battalions++;
                select.SetBattalions();
                print("Extra battalion placed.");
                GameManager.instance.SetUpClickValid = true;
                scoreManager.ChangeScore(GameManager.instance.currentRace, "battalions", 1);
            }
        }
        if (GameManager.instance.currentRace == "TENNO")
        {
            if (select.country.race == "NULL")
            {
                select.country.race = "TENNO";
                select.TintColor(new Color32(255, 255, 0, 70));
                select.country.battalions++;
                select.SetBattalions();
                print("Owner Set to TENNO.");
                GameManager.instance.SetUpClickValid = true;

                scoreManager.ChangeScore(GameManager.instance.currentRace, "territory", 1);
                scoreManager.ChangeScore(GameManager.instance.currentRace, "battalions", 1);
            }
            else if (select.country.race == "TENNO")
            {
                select.country.battalions++;
                select.SetBattalions();
                print("Extra battalion placed.");
                GameManager.instance.SetUpClickValid = true;
                scoreManager.ChangeScore(GameManager.instance.currentRace, "battalions", 1);
            }
        }
        if (GameManager.instance.currentRace == "HUMAN")
        {
            if (select.country.race == "NULL")
            {
                select.country.race = "HUMAN";
                select.TintColor(new Color32(0, 255, 0, 70));
                select.country.battalions++;
                select.SetBattalions();
                print("Owner Set to HUMAN.");
                GameManager.instance.SetUpClickValid = true;

                scoreManager.ChangeScore(GameManager.instance.currentRace, "territory", 1);
                scoreManager.ChangeScore(GameManager.instance.currentRace, "battalions", 1);
            }
            else if (select.country.race == "HUMAN")
            {
                select.country.battalions++;
                select.SetBattalions();
                print("Extra battalion placed.");
                GameManager.instance.SetUpClickValid = true;
                scoreManager.ChangeScore(GameManager.instance.currentRace, "battalions", 1);
            }
        }
    }

    //Places a battalion
    public void DraftCounter()
    {
        placedBattalions++;
        remainingBattalions--;
        print(placedBattalions + " placed");

        scoreManager.ChangeScore(GameManager.instance.CURRENTRACE, "battalions", 1);
    }

    //Set the Slider info
    public void FortifyMoveBattalions()
    {
        CountryHandler from = GameObject.Find(GameManager.instance.fortifyFrom)
                .GetComponent<CountryHandler>();

        fortifySliderParent.SetActive(true);
        fortifySlider.maxValue = from.country.battalions - 1;
        print("Slider max:" + fortifySlider.maxValue);
        fortifySlider.minValue = 0;
        print("Slider min:" + fortifySlider.minValue);
    }

    //Move specified amount of battalions in fortify phase
    public void MoveBattalions_f()
    {
        CountryHandler to = GameObject.Find(GameManager.instance.fortifyTo)
               .GetComponent<CountryHandler>();

        CountryHandler from = GameObject.Find(GameManager.instance.fortifyFrom)
                .GetComponent<CountryHandler>();

        from.country.battalions -= fortify_amount;
        to.country.battalions += fortify_amount;

        fortifySliderParent.SetActive(false);
        DisableFortifyPhasePanel();
    }

    //Set the Slider info
    public void AttackMoveBattalions()
    {
        //CountryHandler def = GameObject.Find(GameManager.instance.attackedTerritory)
        //.GetComponent<CountryHandler>();

        CountryHandler win = GameObject.Find(GameManager.instance.attackFrom)
                .GetComponent<CountryHandler>();

        attackSliderParent.SetActive(true);
        attackSlider.maxValue = win.country.battalions - 1;
        print("Slider max:" + attackSlider.maxValue);
        attackSlider.minValue = 1;
        print("Slider min:" + attackSlider.minValue);
    }

    //Move specified amount of battalions in attack phase
    public void MoveBattalions_a()
    {
        CountryHandler def = GameObject.Find(GameManager.instance.attackedTerritory)
               .GetComponent<CountryHandler>();

        CountryHandler win = GameObject.Find(GameManager.instance.attackFrom)
                .GetComponent<CountryHandler>();

        win.country.battalions -= attack_amount;
        def.country.battalions += attack_amount;

        attackSliderParent.SetActive(false);
    }

    public void ShowMap()
    {
        _Map.SetActive(true);
    }

    public void DisableMap()
    {
        _Map.SetActive(false);
    }

    public void ShowMenu()
    {
        Menu.SetActive(true);
    }

    public void DisableMenu()
    {
        Menu.SetActive(false);
    }

    public void ShowBattalions()
    {
        battalionsPanel.SetActive(true);
    }

    public void DisableBattalions()
    {
        battalionsPanel.SetActive(false);
    }

    //Enable the GUI
    public void ShowGUI(string race, int money, int exp, int level)
    {
        print("Showing GUI");

        GUI gui = GUI.GetComponent<GUI>();

        GUI.SetActive(true);

        //Show values to player
        gui.current_race.text = race.ToString();
        gui.race_exp.text = "EXP: " + exp.ToString();
        gui.race_money.text = "Money: " + money.ToString();
        gui.race_level.text = "Level: " + level.ToString();

        //Set the appearence of the turn button
        if (race == "AVIAN")
        {
            gui.button_image.color = new Color32(255, 0, 255, 255);
            gui.race_icon.sprite = gui.avian_icon;
        }
        else if (race == "VORTA")
        {
            gui.button_image.color = new Color32(0, 255, 255, 255);
            gui.race_icon.sprite = gui.vorta_icon;
        }
        else if (race == "KABAL")
        {
            gui.button_image.color = new Color32(255, 0, 0, 255);
            gui.race_icon.sprite = gui.kabal_icon;
        }
        else if (race == "EVREI")
        {
            gui.button_image.color = new Color32(0, 0, 255, 255);
            gui.race_icon.sprite = gui.evrei_icon;
        }
        else if (race == "TENNO")
        {
            gui.button_image.color = new Color32(255, 255, 0, 255);
            gui.race_icon.sprite = gui.tenno_icon;
        }
        else if (race == "HUMAN")
        {
            gui.button_image.color = new Color32(0, 255, 0, 255);
            gui.race_icon.sprite = gui.human_icon;
        }

        GameManager.instance.territory = scoreManager.GetScore(race, "territory");
        GameManager.instance.battalions = scoreManager.GetScore(race, "battalions");
    }

    //Disable the GUI
    public void DisableGUI()
    {
        GUI.SetActive(false);
    }

    //Show SetUp phase interface
    public void ShowSetUpPanel(string _race, int cycle)
    {
        setUpPanel.SetActive(true);
        SetUpPanel set = setUpPanel.GetComponent<SetUpPanel>();
        set.selectingRace.text = "Selecting Race: " + _race;
        set.cycle.text = "Cycle no: " + cycle;

        //Purple
        if (_race == "AVIAN")
        {
            set.selectingRace.color = new Color32(255, 0, 255, 255);
        }

        //Blue
        if (_race == "EVREI")
        {
            set.selectingRace.color = new Color32(0, 0, 255, 255);
        }

        //Green
        if (_race == "HUMAN")
        {
            set.selectingRace.color = new Color32(0, 255, 0, 255);
        }

        //Red
        if (_race == "KABAL")
        {
            set.selectingRace.color = new Color32(255, 0, 0, 255);
        }

        //Yellow
        if (_race == "TENNO")
        {
            set.selectingRace.color = new Color32(255, 255, 0, 255);
        }

        //Cyan
        if (_race == "VORTA")
        {
            set.selectingRace.color = new Color32(0, 255, 255, 255);
        }
    }

    //Disable SetUp phase interface
    public void DisableSetUpPanel()
    {
        setUpPanel.SetActive(false);

        print("Set up panel disabled");
    }

    //Show draft phase interface
    public void ShowDraftPanel()
    {
        draftPanel.SetActive(true);
        DraftPanel draft = draftPanel.GetComponent<DraftPanel>();

        int bonus = draftBattalions - baseBattalionAmount;

        draft.description.text = " You have placed: " + placedBattalions.ToString();
        draft.description1.text = remainingBattalions + " battalions to place!";
        draft.description2.text = "Bonus: " + bonus.ToString();
        draft.description3.text = "Bought: " + boughtBattalions.ToString();
        draft.description4.text = draftBattalions.ToString() + " battalions total!";

        GameManager.instance.FortifyDone = false;
    }

    //Disable draft phase interface
    public void DisableDraftPanel()
    {
        GameManager.instance.DraftDone = true;
        draftPanel.SetActive(false);
        placedBattalions = 0;
        print("Draft Panel Disabled");
        ShowAttackPhasePanel();
    }

    //Show attack phase interface
    public void ShowAttackPhasePanel()
    {
        print("AttackPhasePanel active");

        AttackPhasePanel attack = attackPhasePanel.GetComponent<AttackPhasePanel>();

        attackPhasePanel.SetActive(true);
    }

    //Disable the attack phase interface
    public void DisableAttackPhasePanel()
    {
        print("AttackPhasePanel disabled");
        attackPhasePanel.SetActive(false);
        GameManager.instance.AttackDone = true;

        ShowFortifyPhasePanel();
    }

    //Show the attack interface
    public void ShowAttackPanel(string description, int moneyReward, int expReward)
    {
        //Enable the Attack panel
        attackPanel.SetActive(true);
        //Disable the Menu
        DisableMenu();
        //Disable the battalion numbers
        DisableBattalions();
        //Disable Main GUI
        DisableGUI();
        //Disable Attack Phase Gui
        attackPhasePanel.SetActive(false);
        AttackPanel gui = attackPanel.GetComponent<AttackPanel>();
        gui.descriptionText.text = description;
        gui.moneyRewardText.text = "Money Reward: +" + moneyReward.ToString();
        gui.expRewardText.text = "EXP Reward: +" + expReward.ToString();
        print("Attack Panel Active!");
    }

    //Reset to the beginning of the attack
    public void ResetAttack()
    {
        attackClickCount = 0;

        ResetCamera();

        //ResetRewards();
    }

    public void DisableAttackPanel()
    {
        //Disable the Attack panel
        attackPanel.SetActive(false);
        print("Attack Panel Disabled!");
        //Enable the Menu
        ShowMenu();
        //Enable the Map
        ShowMap();
        //Enable Attack phase Gui
        attackPhasePanel.SetActive(true);
        //TintTerritories();
    }

    //Show results of battle
    public void ShowBattlePanel(string description, int defenderRoll_1, int attackerRoll_1,
        int defenderRoll_2, int attackerRoll_2, int defenderRoll_3, int attackerRoll_3, string winLoss)
    {

        attackPanel.SetActive(false);
        _Map.SetActive(false);
        battalionsPanel.SetActive(false);
        Menu.SetActive(false);
        //Disable Attack Phase Gui
        attackPhasePanel.SetActive(false);
        battlePanel.SetActive(true);

        BattlePanel battle = battlePanel.GetComponent<BattlePanel>();

        battle.descriptionText.text = description;
        battle.defenderRoll1Text.text = defenderRoll_1.ToString();
        battle.attackerRoll1Text.text = attackerRoll_1.ToString();
        battle.defenderRoll2Text.text = defenderRoll_2.ToString();
        battle.attackerRoll2Text.text = attackerRoll_2.ToString();
        battle.defenderRoll3Text.text = defenderRoll_3.ToString();
        battle.attackerRoll3Text.text = attackerRoll_3.ToString();
        battle.winLossText.text = winLoss;
        print("Battle Panel Active!");
    }

    //Disable the battle results
    public void DisableBattlePanel()
    {
        GameManager.instance.battleHasEnded = true;

        battlePanel.SetActive(false);
        _Map.SetActive(true);
        battalionsPanel.SetActive(true);
        Menu.SetActive(true);
        //Disable Attack Phase Gui
        attackPhasePanel.SetActive(true);
        print("Battle Panel Disabled!");

        //SceneManager.LoadScene("Main");
    }

    //Show fortify interface
    public void ShowFortifyPhasePanel()
    {
        FortifyPanel fortify = fortifyPhasePanel.GetComponent<FortifyPanel>();

        fortifyPhasePanel.SetActive(true);

        fortify.description.text = "Choose two territories to fortify";

        print("Fortify Phase Panel Active");
    }

    //Print error
    public void FortifyError()
    {
        FortifyPanel fortify = fortifyPhasePanel.GetComponent<FortifyPanel>();

        fortify.description.text = "ERROR: Choose two different territories!!!";

        print("ERROR: Choose two different territories!!!");
    }

    //Disable the fortify interface
    public void DisableFortifyPhasePanel()
    {
        fortifyPhasePanel.SetActive(false);
        GameManager.instance.FortifyDone = true;
        GameManager.instance.DraftDone = false;
        GameManager.instance.AttackDone = false;

        print("Fortify Phase Panel Disabled");
    }

    //End current stage
    public void EndStage()
    {
        print("BIG BUTTON PRESSED!!!");

        ResetCamera();

        //if in draft phase
        if (GameManager.instance.SetUpDone == true && GameManager.instance.DraftDone == false && remainingBattalions == 0)
        {
            DisableDraftPanel();
        }
        //if in attack phase
        else if (GameManager.instance.SetUpDone == true && GameManager.instance.DraftDone == true && GameManager.instance.AttackDone == false)
        {
            DisableAttackPhasePanel();
        }
        //if in fortify pahse
        else if (GameManager.instance.SetUpDone == true && GameManager.instance.DraftDone == true && GameManager.instance.AttackDone == true && GameManager.instance.FortifyDone == false)
        {
            DisableFortifyPhasePanel();

            GameManager.instance.territory = scoreManager.GetScore(GameManager.instance.CURRENTRACE, "territory");
            GameManager.instance.battalions = scoreManager.GetScore(GameManager.instance.CURRENTRACE, "battalions");

            EndTurn();
        }

    }

    //Zoom camera in on the clicked game object
    public void ZoomCamera()
    {
        CameraZoom cam = camZoom.GetComponent<CameraZoom>();

        cam.fromPosition = GameManager.instance.attackFromPosition;
        cam.toPosition = GameManager.instance.attackedTerritoryPosition;

        battalionsPanel.SetActive(false);

        cam.Move();
    }

    //Reset Camera position to show all of the map
    public void ResetCamera()
    {
        CameraZoom cam = camZoom.GetComponent<CameraZoom>();

        battalionsPanel.SetActive(true);

        cam.ResetPosition();
    }

    public void Buy1battalion()
    {
        //if the player has enough money
        if (GameManager.instance.money >= 150)
        {
            //Add battalions to draft
            remainingBattalions += 1;
            draftBattalions += 1;
            boughtBattalions += 1;
            GameManager.instance.money -= 150;
            scoreManager.ChangeScore(GameManager.instance.CURRENTRACE, "money", -150);  //Update Dictionary
            ShowGUI(GameManager.instance.CURRENTRACE, GameManager.instance.money, GameManager.instance.exp, GameManager.instance.level);    //Show GUI with new values

            print(GameManager.instance.CURRENTRACE + " has bought 1 battalion for: 150");
        }

        print(GameManager.instance.CURRENTRACE + " does not have enough funds to buy 1 battalion.");
    }

    public void Buy5battalion()
    {
        //if the player has enough money and levels
        if (GameManager.instance.money >= 500 && GameManager.instance.level >= 4)
        {
            //Add battalions to draft
            remainingBattalions += 5;
            draftBattalions += 5;
            boughtBattalions += 5;
            GameManager.instance.money -= 500;
            scoreManager.ChangeScore(GameManager.instance.CURRENTRACE, "money", -500);  //Update Dictionary
            ShowGUI(GameManager.instance.CURRENTRACE, GameManager.instance.money, GameManager.instance.exp, GameManager.instance.level);    //Show GUI with new values

            print(GameManager.instance.CURRENTRACE + " has bought 5 battalions for: 500");
        }

        print(GameManager.instance.CURRENTRACE + " does not have enough funds to buy 5 battalions and/or aren't a high enough level.");
    }

    //Buy 10 battalions
    public void Buy10battalion()
    {
        //if the player has enough money and levels
        if (GameManager.instance.money >= 800 && GameManager.instance.level >= 8)
        {
            //Add battalions to draft
            remainingBattalions += 10;
            draftBattalions += 10;
            boughtBattalions += 10;
            GameManager.instance.money -= 800;
            scoreManager.ChangeScore(GameManager.instance.CURRENTRACE, "money", -800);  //Update Dictionary
            ShowGUI(GameManager.instance.CURRENTRACE, GameManager.instance.money, GameManager.instance.exp, GameManager.instance.level);    //Show GUI with new values

            print(GameManager.instance.CURRENTRACE + " has bought 10 battalions for: 800");
        }

        print(GameManager.instance.CURRENTRACE + " does not have enough funds to buy 10 battalions and/or aren't a high enough level.");
    }

    //Check if the game has been won
    public void CheckIfGameWon()
    {
        print("Checking if Game has been Won");
        //Loop through the list of territories
        for (int i = 0; i < territoryList.Count; i++)
        {
            CountryHandler handler = territoryList[i].GetComponent
                <CountryHandler>();

            //Check if it belongs to the playing race
            if (handler.country.race == GameManager.instance.CURRENTRACE)
            {
                winCount++;
            }
        }

        //Game not won
        if (winCount < 42)
        {
            print("Game not Won");
            GameManager.instance.gameActive = true;

            winCount = 0;
        }

        //Game won
        if (winCount >= 42)
        {
            GameOverPanel gameover = gameOverPanel.GetComponent<GameOverPanel>();

            print("Game Won!!!");

            //Show game over panel
            GameManager.instance.gameActive = true;

            draftPanel.SetActive(false);
            attackPhasePanel.SetActive(false);
            attackPanel.SetActive(false);
            fortifyPhasePanel.SetActive(false);
            Menu.SetActive(false);
            GUI.SetActive(false);
            battalionsPanel.SetActive(false);
            gameOverPanel.SetActive(true);
            gameover.Winner.text = "Congratulations " + GameManager.instance.CURRENTRACE + "! You've dominated the Galaxy";

            GameManager.instance.gameActive = false;

        }
    }

    //Call delete function
    public void DeleteFile()
    {
        GameManager.instance.DeleteSaveFile();
    }

    //Call save function
    public void SaveFile()
    {
        GameManager.instance.Saving();
    }

    public int SaveGetScore(string race, string type)
    {
        return scoreManager.GetScore(race, type);
    }

    //Exit out of the application
    public void QuitApp()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    //Reset the rewards of a territory
    public void ResetRewards()
    {
        CountryHandler handler = GameObject.Find(GameManager.instance.attackedTerritory)
                .GetComponent<CountryHandler>();

        handler.country.expReward = 30; //Set base exp value
        handler.country.moneyReward = 100;  //Set base money value
    }

    //Restore the leaderboard to saved values
    public void RestoreLeaderboard()
    {
        if(GameManager.instance.gameBegins == false)
        {
            print("Restoring Leaderboard");
            foreach (string race in GameManager.instance.races)
            {
                if(race == "AVIAN")
                {
                    scoreManager.SetScore(race, "money", GameManager.instance.money_AVIAN);
                    scoreManager.SetScore(race, "exp", GameManager.instance.exp_AVIAN);
                    scoreManager.SetScore(race, "level", GameManager.instance.level_AVIAN);
                    scoreManager.SetScore(race, "battalions", GameManager.instance.battalions_AVIAN);
                    scoreManager.SetScore(race, "territory", GameManager.instance.territories_AVIAN);

                    print("AVIAN stats complete!");
                }
                if (race == "VORTA")
                {
                    scoreManager.SetScore(race, "money", GameManager.instance.money_VORTA);
                    scoreManager.SetScore(race, "exp", GameManager.instance.exp_VORTA);
                    scoreManager.SetScore(race, "level", GameManager.instance.level_VORTA);
                    scoreManager.SetScore(race, "battalions", GameManager.instance.battalions_VORTA);
                    scoreManager.SetScore(race, "territory", GameManager.instance.territories_VORTA);

                    print("VORTA stats complete!");
                }
                if (race == "EVREI")
                {
                    scoreManager.SetScore(race, "money", GameManager.instance.money_EVREI);
                    scoreManager.SetScore(race, "exp", GameManager.instance.exp_EVREI);
                    scoreManager.SetScore(race, "level", GameManager.instance.level_EVREI);
                    scoreManager.SetScore(race, "battalions", GameManager.instance.battalions_EVREI);
                    scoreManager.SetScore(race, "territory", GameManager.instance.territories_EVREI);

                    print("EVREI stats complete!");
                }
                if (race == "KABAL")
                {
                    scoreManager.SetScore(race, "money", GameManager.instance.money_KABAL);
                    scoreManager.SetScore(race, "exp", GameManager.instance.exp_KABAL);
                    scoreManager.SetScore(race, "level", GameManager.instance.level_KABAL);
                    scoreManager.SetScore(race, "battalions", GameManager.instance.battalions_KABAL);
                    scoreManager.SetScore(race, "territory", GameManager.instance.territories_KABAL);

                    print("KABAL stats complete!");
                }
                if (race == "TENNO")
                {
                    scoreManager.SetScore(race, "money", GameManager.instance.money_TENNO);
                    scoreManager.SetScore(race, "exp", GameManager.instance.exp_TENNO);
                    scoreManager.SetScore(race, "level", GameManager.instance.level_TENNO);
                    scoreManager.SetScore(race, "battalions", GameManager.instance.battalions_TENNO);
                    scoreManager.SetScore(race, "territory", GameManager.instance.territories_TENNO);

                    print("TENNO stats complete!");
                }
                if (race == "HUMAN")
                {
                    scoreManager.SetScore(race, "money", GameManager.instance.money_HUMAN);
                    scoreManager.SetScore(race, "exp", GameManager.instance.exp_HUMAN);
                    scoreManager.SetScore(race, "level", GameManager.instance.level_HUMAN);
                    scoreManager.SetScore(race, "battalions", GameManager.instance.battalions_HUMAN);
                    scoreManager.SetScore(race, "territory", GameManager.instance.territories_HUMAN);

                    print("HUMAN stats complete!");
                }

            }
        }
    }

    //Ends a player turn
    public void EndTurn()
    {
        //reset values
        ownedTerritories = 0;
        fortifyClickCount = 0;
        attackClickCount = 0;

        GameManager.instance.RemoveRace();  //Checks if there is need to remove a race

        print("END OF TURN");
        turnOver = true;
    }
}
