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

        statWindowController = FindAnyObjectByType<MapStatController>();
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
                            case 0: //Heal 5 HP
                                //Heal Player
                                playerController.Heal(5);
                                //Simple VFX animation or text

                                dungeonController.SetNextRoomTimer(1f);
                                Debug.Log("Healed 5 HP!");
                                break;
                            case 1: //Max HP +3
                                //Increase max HP
                                playerController.IncreaseMaxHP(3);
                                //Simple VFX animation or text

                                dungeonController.SetNextRoomTimer(1f);
                                Debug.Log("Max HP +3!");
                                break;
                            case 2: //+1 to 2 dice
                                //Open stat window
                                statWindowController.OpenStats(true);
                                //Make specific abilities clickable
                                if(playerController.curClass == PlayerController.Class.druid)
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
                            case 3: //+2 to 1 dice
                                //Open stat window
                                statWindowController.OpenStats(true);
                                //Make specific abilities clickable
                                if (playerController.curClass == PlayerController.Class.druid)
                                {
                                    statWindowController.SetUpgradeValue(2, 1);
                                    statWindowController.abilityButtons[1].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[2].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[3].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[4].GetComponent<Button>().interactable = true;
                                }
                                else //if brawler
                                {
                                    statWindowController.SetUpgradeValue(2, 1);
                                    statWindowController.abilityButtons[1].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[2].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[3].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[4].GetComponent<Button>().interactable = true;
                                    statWindowController.abilityButtons[5].GetComponent<Button>().interactable = true;
                                }
                                break;
                            case 4: //copy dice
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
                            case 5: //x2 dice chance buff
                                GetComponent<RectTransform>().DOAnchorPosY(540f, 1f).SetEase(Ease.InOutCubic).SetDelay(1f);
                                //Simple VFX animation or text

                                dungeonController.SetNextRoomTimer(1f);
                                Debug.Log("Gained x2 dice chance buff");
                                break;
                        }
                        break;
                    case "curse":

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
        slotText[2].text = "Add +1 to 2 dice faces";
        slotText[3].text = "Add +2 to 1 dice face";
        slotText[4].text = "Copy a dice face onto another dice face";
        slotText[5].text = "Grants a chance for dice face to activate twice";
    }

    public void SetupCurses()
    {
        roomType = "curse";


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
