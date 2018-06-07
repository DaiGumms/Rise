using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountryHandler : MonoBehaviour {

    public Country country;         //Reference to Country script

    public Text battalion;          //Reference to battalion number

    public AudioSource hover_audio; //Reference to audio clip
    public AudioSource click_audio; //Reference to audio clip

    public int strat_value;
    public int reward_value;

    private SpriteRenderer sprite;  //Reference to Sprite renderer of territory

    private Color32 oldColor;
    private Color32 hoverColor;

    public List<GameObject> adjacentTerritories = new List<GameObject>();   //List of territories adjacent to the territory

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        SetBattalions();
    }

    //Highlight relavent territories
    void OnMouseEnter()
    {
        oldColor = sprite.color;
        if (GameManager.instance.SetUpDone == true)
        {
            //If in the Draft Phase:
            if (CountryManager.instance.draftPanel.activeInHierarchy)   //If draft phase panel is showing
            {
                if (country.race == GameManager.instance.CURRENTRACE)   //If player has entered an owned territory
                {
                    hover_audio.Play();
                    hoverColor = new Color32(oldColor.r, oldColor.g, oldColor.b, 175);  //Increase the alpha of that territory
                    sprite.color = hoverColor;  //Chsnge the colour
                }
            }

            //If in the Attack Phase:
            if (CountryManager.instance.attackPhasePanel.activeInHierarchy) //If attack phase panel is showing
            {
                if (CountryManager.instance.attackClickCount == 0 && !CountryManager.instance.attackSliderParent.activeInHierarchy && country.battalions > 1)
                {
                    if (country.race == GameManager.instance.CURRENTRACE)
                    {
                        hover_audio.Play();
                        hoverColor = new Color32(oldColor.r, oldColor.g, oldColor.b, 175);  //Highlight player owned territories
                        sprite.color = hoverColor;
                    }
                }

                else if (country.race != GameManager.instance.CURRENTRACE && CountryManager.instance.attackClickCount == 1 && !CountryManager.instance.attackSliderParent.activeInHierarchy)
                {
                    foreach (GameObject attackable in adjacentTerritories)
                    {
                        if (GameManager.instance.attackFrom == attackable.name)
                        {
                            hover_audio.Play();
                            hoverColor = new Color32(oldColor.r, oldColor.g, oldColor.b, 175);  //Highlight attackable territories
                            sprite.color = hoverColor;
                        }
                    }
                }
            }

            //If in the Fortify Phase:
            if (CountryManager.instance.fortifyPhasePanel.activeInHierarchy)
            {
                if (country.race == GameManager.instance.CURRENTRACE)
                {
                    hover_audio.Play();
                    hoverColor = new Color32(oldColor.r, oldColor.g, oldColor.b, 125);  //Highlight player owned territories
                    sprite.color = hoverColor;
                }
            }
        }
    }

    //Set colour when mouse exits GAmeObject
    void OnMouseExit()
    {
        if (GameManager.instance.SetUpDone == true)
        {
            sprite.color = oldColor;    //Reset Colour
        }
    }

    //On click do:
    private void OnMouseUpAsButton()
    {
        if(GameManager.instance.SetUpDone == false)
        {
            GameManager.instance.setUpTerritory = country.name; //Set name of the territory in the SetUp Phase
            GameManager.instance.Click = true;
            click_audio.Play();
        }
        if (GameManager.instance.SetUpDone == true)
        {
            GameManager.instance.selectedTerritory = country.name;

            //print status of game
            print(GameManager.instance.selectedTerritory);
        }
       

        //During Draft Phase
        if (country.race == GameManager.instance.CURRENTRACE && GameManager.instance.SetUpDone == true && GameManager.instance.DraftDone == false
            && CountryManager.instance.placedBattalions < CountryManager.instance.draftBattalions)
        {
            country.battalions++;   //Add one to the number of battalions
            click_audio.Play();
            CountryManager.instance.DraftCounter();
            CountryManager.instance.ShowDraftPanel();
        }

        //During Attack Phase
        if (GameManager.instance.SetUpDone == true && GameManager.instance.DraftDone == true 
                    && GameManager.instance.AttackDone == false)
        {
            if (country.race == GameManager.instance.CURRENTRACE && CountryManager.instance.attackClickCount == 0 && !CountryManager.instance.attackSliderParent.activeInHierarchy
                && CountryManager.instance.attackPhasePanel.activeInHierarchy && country.battalions > 1)
            {
                click_audio.Play();
                GameManager.instance.attackFrom = country.name; //Set the territory the player is attacking from
                GameManager.instance.attackFromArea = country.area; //Set the asre the player is attacking from
                print("Attacking from: " + GameManager.instance.attackFrom);

                GameManager.instance.attackFromPosition = transform.position;

                CountryManager.instance.ZoomCamera();

                CountryManager.instance.attackClickCount++; //Increment the click count for the attack phase
            }
            else if (country.race != GameManager.instance.CURRENTRACE && CountryManager.instance.attackClickCount == 1)
            {
                //CountryManager.instance.attackClickCount--;
                foreach (GameObject attackable in adjacentTerritories)
                {
                    print(attackable.name);
                    if(GameManager.instance.attackFrom == attackable.name)  //If the territory the player is attacking from is in 
                    {                                                           //the attable territories of the territory being attacked
                        GameManager.instance.attackedTerritory = country.name;  //Set name of attacked territory
                        print("Attacking: " + GameManager.instance.attackedTerritory);

                        GameManager.instance.attackedTerritoryPosition = transform.position;

                        click_audio.Play();
                        CalculateRewards();

                        CountryManager.instance.ZoomCamera();

                        ShowGUI();

                    }
                }
            }
            
        }

        //During Fortify Phase
        if (country.race == GameManager.instance.CURRENTRACE && GameManager.instance.SetUpDone == true && GameManager.instance.DraftDone == true 
                && GameManager.instance.AttackDone == true && GameManager.instance.FortifyDone == false)
        {
            //On first click
            if (CountryManager.instance.fortifyClickCount == 0)
            {
                click_audio.Play();
                GameManager.instance.fortifyFrom = country.name;
                print("Battalions moving from: " + GameManager.instance.fortifyFrom);
                CountryManager.instance.fortifyClickCount++;
            }
            //On second click
            else if (CountryManager.instance.fortifyClickCount == 1)
            {
                GameManager.instance.fortifyTo = country.name;

                //If the selcted territories are the same show error
                if (GameManager.instance.fortifyTo == GameManager.instance.fortifyFrom)
                {
                    CountryManager.instance.fortifyClickCount--;    //Reduce click count back to 1

                    CountryManager.instance.FortifyError();
                }
                //If click valid
                if (GameManager.instance.fortifyTo != GameManager.instance.fortifyFrom)
                {
                    print("Battalions moving to: " + GameManager.instance.fortifyTo);
                    click_audio.Play();
                    CountryManager.instance.fortifyClickCount--;
                    CountryManager.instance.FortifyMoveBattalions();
                }
            }
        }
    }

    //Set the game objects tag
    private void OnDrawGizmos()
    {
        country.name = name;
        this.tag = "Territory";
    }

    //Tint the territories colour
    public void TintColor(Color32 color)
    {
        sprite.color = color;
        SetBattalions();
    }

    //Set the battalion number for the territory
    public void SetBattalions()
    {
        battalion.text = country.battalions.ToString();
    }

    // Pass info to Attack window
    void ShowGUI()
    {
        CountryManager.instance.ShowAttackPanel(country.name + " is owned by the " + 
            country.race.ToString() + " race. Are you sure you want to attack from " + GameManager.instance.attackFrom.ToString() + "?", 
                country.moneyReward, country.expReward);
        GameManager.instance.battleHasEnded = false;
        GameManager.instance.battleWon = false;
    }

    public void CalculateRewards()
    {
        print("Calculating Rewards!");

        //If capital extra rewards
        if (country.capital == true)
        {
            country.expReward += 100;
            country.moneyReward += 250;
        }
        //Extra rewards for more surrounding territories
        foreach (GameObject terr in adjacentTerritories)
        {
            CountryHandler handler = terr.GetComponent<CountryHandler>();

            //Less reward for surrounding territories being your own
            if (handler.country.race != GameManager.instance.CURRENTRACE)
            {
                country.expReward += 8;
                country.moneyReward += 23;
            } 
        }
        //Extra reward if in another area
        if (country.area != GameManager.instance.attackFromArea)
        {
            country.expReward += 20;
            country.moneyReward += 40;
        }
        //Extra reward if home territory
        if (country.homeRace == GameManager.instance.CURRENTRACE)
        {
            country.expReward += 40;
            country.moneyReward += 75;
        }
        //Extra reward the more battalions there are defending
        country.expReward += country.expReward + (country.battalions * (5/2));
        country.moneyReward = country.moneyReward + (country.battalions * 5);

    }

}
