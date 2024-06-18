using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class RouletteWheel : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public GameObject[] slotWindow;
    public TextMeshProUGUI[] slotText;
    public Button spinButton;
    public Color unselectedColor;
    public Color selectdColor;
    private float spinRate = 0.03f;         //The rate at which slots are selected. Gets slower over time till it stops completely
    float spinRateTimer = 0f;
    int slotIndex;
    bool spinning;

    float spinDuration = 3f;
    float spinTimer = 0f;

    string roomType;

    MapStatController statWindowController;
    DungeonController dungeonController;
    PlayerController playerController;

    private void Start()
    {
        spinning = false;
        spinTimer = spinDuration;
        spinDuration = Random.Range(2.5f, 3.5f);
        slotIndex = 0;

        statWindowController = FindObjectOfType<MapStatController>();
        dungeonController = FindAnyObjectByType<DungeonController>();
        playerController = FindAnyObjectByType<PlayerController>();
    }

    private void Update()
    {
        if (spinning)
        {
            spinDuration -= Time.deltaTime;
            spinRateTimer -= Time.deltaTime;

            if(spinRateTimer <= 0)
            {
                slotWindow[slotIndex].GetComponent<Image>().color = unselectedColor;
                slotIndex = (slotIndex + 1) % 6;
                slotWindow[slotIndex].GetComponent<Image>().color = selectdColor;

                spinRate += 0.005f;

                spinRateTimer = spinRate;
            }

            if(spinDuration <= 0)
            {
/*                //------RIGGED SLOTS (for debug ofc)
                slotWindow[slotIndex].GetComponent<Image>().color = unselectedColor;
                slotIndex = 3;
                slotWindow[slotIndex].GetComponent<Image>().color = selectdColor;
                //-----------------------*/

                spinning = false;

                Debug.Log("Landed on " + slotIndex);
                switch (roomType)
                {
                    case "blessing":
                        switch (slotIndex)
                        {
                            //Heal 5 HP
                            case 0:
                                //Heal Player
                                playerController.Heal(5);
                                //Simple VFX animation or text

                                dungeonController.SetNextRoomTimer(1f);
                                Debug.Log("Healed 5 HP!");
                                break;
                            //Max HP +3
                            case 1:
                                //Increase max HP
                                playerController.IncreaseMaxHP(3);
                                //Simple VFX animation or text

                                dungeonController.SetNextRoomTimer(1f);
                                Debug.Log("Max HP +3!");
                                break;
                            //Max rerolls +1
                            case 2:
                                //Increase max rerolls
                                playerController.IncreaseMaxHP(3);

                                dungeonController.SetNextRoomTimer(1f);
                                Debug.Log("Max Rerolls +1!");
                                break;
                            case 3: //+1 to 2 dice
                                //Open stat window
                                statWindowController.OpenStats(true);
                                //Make specific abilities clickable
                                if (playerController.curClass == PlayerController.Class.druid)
                                {
                                    statWindowController.SetUpgradeValue(1, 2);
                                    statWindowController.abilityButtons[1].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[2].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[3].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[4].GetComponent<Button>().interactable = true;
                                }
                                else //if brawler
                                {
                                    statWindowController.SetUpgradeValue(1, 2);
                                    statWindowController.abilityButtons[1].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[2].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[3].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[4].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[5].GetComponent<Button>().interactable = true;
                                }
                                break;
                            case 4: //+2 to 1 dice
                                //Open stat window
                                statWindowController.OpenStats(true);
                                //Make specific abilities clickable
                                if (playerController.curClass == PlayerController.Class.druid)
                                {
                                    statWindowController.SetUpgradeValue(2, 1);
                                    statWindowController.abilityButtons[0].GetComponent<Button>().interactable = false;
                                    statWindowController.abilityButtons[1].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[2].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[3].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[4].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[5].GetComponent<Button>().interactable = false;

                                }
                                else //if brawler
                                {
                                    statWindowController.SetUpgradeValue(2, 1);
                                    statWindowController.abilityButtons[0].GetComponent<Button>().interactable = false;
                                    statWindowController.abilityButtons[1].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[2].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[3].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[4].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[5].GetComponent<Button>().interactable = true;
                                }
                                break;
                            case 5: //copy dice
                                //Open stat window
                                statWindowController.OpenStats(true);
                                //Make all abilities clickable
                                /*
                                statWindowController.abilityButtons[0].GetComponent<Button>().interactable = true;
                                statWindowController.abilityButtons[1].GetComponent<Button>().interactable = true;
                                statWindowController.abilityButtons[2].GetComponent<Button>().interactable = true;
                                statWindowController.abilityButtons[3].GetComponent<Button>().interactable = true;
                                statWindowController.abilityButtons[4].GetComponent<Button>().interactable = true;
                                statWindowController.abilityButtons[5].GetComponent<Button>().interactable = true;
                                */

                                break;
                        }
                        break;
                    case "curse":
                        switch (slotIndex)
                        {
                            //No effect
                            case 0: 
                                //Simple VFX animation or text

                                dungeonController.SetNextRoomTimer(1f);
                                Debug.Log("Never Punished");
                                break;
                            //Take 3 DMG
                            case 1:
                                //Increase max HP
                                playerController.Damage(3);
                                //Simple VFX animation or text

                                dungeonController.SetNextRoomTimer(1f);
                                Debug.Log("Max HP +3!");
                                break;
                            //Lower max HP by 3
                            case 2:
                                playerController.IncreaseMaxHP(-3);
                                break;
                            //Lower max reroll by 1
                            case 3:
                                playerController.IncreaseMaxRerolls(-1);
                                break;
                            //-1 to 1 die
                            case 4:
                                //Open stat window
                                statWindowController.OpenStats(true);
                                //Make specific abilities clickable
                                if (playerController.curClass == PlayerController.Class.druid)
                                {
                                    statWindowController.SetUpgradeValue(-1, 1);
                                    statWindowController.abilityButtons[1].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[2].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[3].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[4].GetComponent<Button>().interactable = true;
                                }
                                else //if brawler
                                {
                                    statWindowController.SetUpgradeValue(-1, 1);
                                    statWindowController.abilityButtons[1].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[2].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[3].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[4].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[5].GetComponent<Button>().interactable = true;
                                }
                                break;
                            //-2 to 1 die
                            case 5: 
                                //Open stat window
                                statWindowController.OpenStats(true);
                                //Make specific abilities clickable
                                if (playerController.curClass == PlayerController.Class.druid)
                                {
                                    statWindowController.SetUpgradeValue(-2, 1);
                                    statWindowController.abilityButtons[1].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[2].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[3].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[4].GetComponent<Button>().interactable = true;
                                }
                                else //if brawler
                                {
                                    statWindowController.SetUpgradeValue(-2, 1);
                                    statWindowController.abilityButtons[1].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[2].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[3].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[4].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[5].GetComponent<Button>().interactable = true;
                                }
                                break;
                        }
                        break;
                    case "player":

                        break;
                    case "enemy":

                        break;
                }
            }
        }
    }

    public void ResetSlots()
    {
        foreach(GameObject slot in slotWindow)
        {
            slot.GetComponent<Image>().color = unselectedColor;
        }
    }

    public void SetupBlessings()
    {
        roomType = "blessing";

        slotText[0].text = "Heal for 5 HP";
        slotText[1].text = "Increase Max HP by 3";
        slotText[2].text = "Raise max rerolls by 1";
        slotText[3].text = "Add +1 to 2 dice faces"; 
        slotText[4].text = "Add +2 to 1 dice face";
        slotText[5].text = "Copy a dice face onto another dice face";
    }

    public void SetupCurses()
    {
        roomType = "curse";

        slotText[0].text = "Never punished (no effect)";
        slotText[1].text = "Take 3 DMG";
        slotText[2].text = "Lowers max HP by 3";
        slotText[3].text = "Lowers max rerolls by 1";
        slotText[4].text = "Subtract [1] to 1 die face";
        slotText[5].text = "Subtract [2] to 1 die face";
    }

    public void SetupPlayerAttacks()
    {
        roomType = "player";


    }

    public void SetupEnemyAttacks()
    {
        roomType = "enemy";


    }

    public void Spin()
    {
        spinning = true;
        spinButton.interactable = false;
    }
}
