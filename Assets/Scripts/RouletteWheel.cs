using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class RouletteWheel : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public GameObject[] slotWindow;
    public TextMeshProUGUI[] slotText;
    public Button spinButton;
    public Button rerollButton;
    public Button continueButton;
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
    BattleController battleController;
    PlayerController playerController;

    private void Start()
    {
        spinning = false;
        spinTimer = spinDuration;
        spinDuration = Random.Range(2.5f, 3.5f);
        slotIndex = Random.Range(0, 6);

        statWindowController = GameObject.FindGameObjectWithTag("MapStat").GetComponent<MapStatController>();
        dungeonController = GameObject.FindGameObjectWithTag("DungeonController").GetComponent<DungeonController>();
        battleController = GameObject.FindGameObjectWithTag("BattleController").GetComponent<BattleController>();

        rerollButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (spinning)
        {
            spinTimer -= Time.deltaTime;
            spinRateTimer -= Time.deltaTime;

            if(spinRateTimer <= 0)
            {
                slotWindow[slotIndex].GetComponent<Image>().color = unselectedColor;
                slotIndex = (slotIndex + 1) % 6;
                slotWindow[slotIndex].GetComponent<Image>().color = selectdColor;

                spinRate += 0.005f;

                spinRateTimer = spinRate;
            }

            if(spinTimer <= 0)
            {
/*                //------RIGGED SLOTS (for debug ofc)
                slotWindow[slotIndex].GetComponent<Image>().color = unselectedColor;
                slotIndex = 3;
                slotWindow[slotIndex].GetComponent<Image>().color = selectdColor;
                //-----------------------*/

                spinning = false;

                //Debug.Log("Landed on " + slotIndex);
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
                                playerController.IncreaseMaxRerolls(1);

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
                            case 5: //copy dice (Heal until it gets updated)
                                slotWindow[slotIndex].GetComponent<Image>().color = unselectedColor;
                                slotIndex = 0;
                                slotWindow[slotIndex].GetComponent<Image>().color = selectdColor;
                                playerController.Heal(5);

                                dungeonController.SetNextRoomTimer(1f);
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
                                //Take 3 dmg
                                playerController.Damage(3, 1f);
                                //Simple VFX animation or text

                                dungeonController.SetNextRoomTimer(1f);
                                Debug.Log("Max HP +3!");
                                break;
                            //Lower max HP by 3
                            case 2:
                                playerController.IncreaseMaxHP(-3);

                                dungeonController.SetNextRoomTimer(1f);
                                break;
                            //Lower max reroll by 1
                            case 3:
                                playerController.IncreaseMaxRerolls(-1);

                                dungeonController.SetNextRoomTimer(1f);
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
                        if (playerController.GetCurrentRerolls() > 0 && spinTimer > -10)
                        {
                            rerollButton.gameObject.SetActive(true);
                            continueButton.gameObject.SetActive(true);
                        }
                        else
                        {
                            switch (playerController.curClass)
                            {
                                case PlayerController.Class.druid:
                                    switch (slotIndex)
                                    {
                                        case 0: //Nothing
                                            playerController.Nothing(1f);
                                            break;
                                        case 1: //Heal
                                            playerController.DruidHeal(1f);
                                            break;
                                        case 2: //Vine Attack
                                            playerController.DruidVine(2f);
                                            break;
                                        case 3: //Summon ATK Sapling
                                            playerController.SummonATKSapling(1f);
                                            break;
                                        case 4: //Fly
                                            playerController.SummonDEFSapling(1f);
                                            break;
                                        case 5: //Enhance
                                            playerController.EnhanceSaplings(1f);
                                            break;
                                    }
                                    break;
                                case PlayerController.Class.brawler:
                                    switch (slotIndex)
                                    {
                                        case 0: //Nothing
                                            playerController.Nothing(1f);
                                            break;
                                        case 1: //Heal
                                            playerController.BrawlerHeal(1f);
                                            break;
                                        case 2: //Block
                                            playerController.BrawlerBlock(1f);
                                            break;
                                        case 3: //Counter
                                            playerController.BrawlerCounter(1f);
                                            break;
                                        case 4: //Slap
                                            playerController.BrawlerSlap(2f);
                                            break;
                                        case 5: //Double Slap
                                            playerController.BrawlerDoubleSlap(2f);
                                            break;
                                    }
                                    break;
                            }

                            //Close roulette window
                            GetComponent<RectTransform>().DOAnchorPosY(540f, 1f).SetEase(Ease.InOutCubic).SetDelay(1f);
                        }

                        break;
                    case "enemy":
                        switch (dungeonController.enemy.GetComponent<Enemy>().enemyName)
                        {
                            case "Owl with a Knife":
                                switch (slotIndex)
                                {
                                    case 0: //Nothing
                                        dungeonController.enemy.GetComponent<KnifeOwl>().Nothing(1f);
                                        break;
                                    case 1: //Peck
                                    case 2:
                                        dungeonController.enemy.GetComponent<KnifeOwl>().Peck(1, 2f);
                                        break;
                                    case 3: //Slash
                                        dungeonController.enemy.GetComponent<KnifeOwl>().Slash(2, 2f);
                                        break;
                                    case 4: //Fly
                                        dungeonController.enemy.GetComponent<KnifeOwl>().Fly(1f);
                                        break;
                                    case 5: //Jumpscare
                                        dungeonController.enemy.GetComponent<KnifeOwl>().Jumpscare(3, 2f);
                                        break;
                                }
                                break;
                            case "Moai Statue":
                                switch (slotIndex)
                                {
                                    case 0: //Nothing
                                        dungeonController.enemy.GetComponent<MoaiStatue>().Nothing(1f);
                                        break;
                                    case 1: //Harden
                                        dungeonController.enemy.GetComponent<MoaiStatue>().Harden(1f);
                                        break;
                                    case 2: //Harden
                                        dungeonController.enemy.GetComponent<MoaiStatue>().Harden(1f);
                                        break;
                                    case 3: //Throw Pebble
                                        dungeonController.enemy.GetComponent<MoaiStatue>().ThrowPebble(1, 2f);
                                        break;
                                    case 4: //Throw Pebble
                                        dungeonController.enemy.GetComponent<MoaiStatue>().ThrowPebble(1, 2f);
                                        break;
                                    case 5: //Rock Slide
                                        dungeonController.enemy.GetComponent<MoaiStatue>().RockSlide(2, 2f);
                                        break;
                                }
                                break;
                        }

                        //Close roulette window
                        GetComponent<RectTransform>().DOAnchorPosY(540f, 1f).SetEase(Ease.InOutCubic).SetDelay(1f);

                        break;
                }
            }
        }
    }

    public void ResetSlots()
    {
        spinButton.interactable = true;
        foreach (GameObject slot in slotWindow)
        {
            slot.GetComponent<Image>().color = unselectedColor;
        }
    }

    public void SetupBlessings()
    {
        spinButton.gameObject.SetActive(true);
        spinning = false;

        roomType = "blessing";

        titleText.text = "Obtain a blessing!";

        slotText[0].text = "Heal for 5 HP";
        slotText[1].text = "Increase Max HP by 3";
        slotText[2].text = "Raise max rerolls by 1";
        slotText[3].text = "Add +1 to 2 dice faces"; 
        slotText[4].text = "Add +2 to 1 dice face";
        slotText[5].text = "Copy a dice face onto another dice face (Sorry this doesn't work yet D:)";

        foreach(GameObject button in statWindowController.abilityButtons)
        {
            button.GetComponent<Button>().interactable = false;
        }
    }

    public void SetupCurses()
    {
        spinButton.gameObject.SetActive(true);
        spinning = false;

        roomType = "curse";

        titleText.text = "Cursed!";

        slotText[0].text = "Never punished (no effect)";
        slotText[1].text = "Take 3 DMG";
        slotText[2].text = "Lowers max HP by 3";
        slotText[3].text = "Lowers max rerolls by 1";
        slotText[4].text = "Subtract [1] to 1 die face";
        slotText[5].text = "Subtract [2] to 1 die face";

        foreach (GameObject button in statWindowController.abilityButtons)
        {
            button.GetComponent<Button>().interactable = false;
        }
    }

    public void SetupPlayerAttacks()
    {
        spinButton.gameObject.SetActive(true);
        spinning = false;

        roomType = "player";

        titleText.text = "Player Abilities";

        slotText[0].text = playerController.abilityTexts[0].text;
        slotText[1].text = playerController.abilityTexts[1].text;
        slotText[2].text = playerController.abilityTexts[2].text;
        slotText[3].text = playerController.abilityTexts[3].text;
        slotText[4].text = playerController.abilityTexts[4].text;
        slotText[5].text = playerController.abilityTexts[5].text;
    }

    public void SetupEnemyAttacks()
    {
        spinButton.gameObject.SetActive(false);
        spinning = false;

        roomType = "enemy";

        titleText.text = "Enemy Attack!";

        slotText[0].text = dungeonController.enemy.GetComponent<Enemy>().abilities[0];
        slotText[1].text = dungeonController.enemy.GetComponent<Enemy>().abilities[1];
        slotText[2].text = dungeonController.enemy.GetComponent<Enemy>().abilities[2];
        slotText[3].text = dungeonController.enemy.GetComponent<Enemy>().abilities[3];
        slotText[4].text = dungeonController.enemy.GetComponent<Enemy>().abilities[4];
        slotText[5].text = dungeonController.enemy.GetComponent<Enemy>().abilities[5];
    }

    public void Spin()
    {
        spinning = true;
        spinButton.interactable = false;

        spinRate = Random.Range(0.005f, 0.03f);
        spinDuration = Random.Range(2.5f, 3.5f);
        slotIndex = Random.Range(0, 6);
        spinTimer = spinDuration;

        rerollButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);

        //Debug.Log("Rate: " + spinRate + ". Duration: " + spinDuration);
    }

    public void Reroll()
    {
        rerollButton.gameObject.SetActive(false);
        Debug.Log(playerController.GetCurrentRerolls());
        playerController.IncreaseCurrentRerolls(-1);
        battleController.UpdateUIText();
        Spin();

        Debug.Log(playerController.GetCurrentRerolls());
    }

    public void Continue()
    {
        rerollButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);
        spinning = true;
        spinTimer = -50;
        spinRateTimer = 0.02f;
    }

    public void SetPlayer(GameObject player)
    {
        playerController = player.GetComponent<PlayerController>();
    }
}
