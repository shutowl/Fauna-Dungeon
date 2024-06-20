using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;

public class BattleController : MonoBehaviour
{
    public enum State
    {
        idle,
        attacking,
        inventory,
        running,
        enemy,
    }
    public State currentState;
    public TextMeshProUGUI curHPText;
    public TextMeshProUGUI maxHPText;
    public TextMeshProUGUI curRerollText;
    public TextMeshProUGUI maxRerollText;
    public GameObject[] battleButtons;
    int battleStep;

    GameObject enemy;

    RouletteWheel rouletteWheel;
    DungeonController dungeonController;
    PlayerController playerController;
    Enemy enemyController;

    float timer;

    void Start()
    {
        battleStep = 0;

        //Get necessary scripts
        rouletteWheel = GameObject.FindGameObjectWithTag("RouletteWheel").GetComponent<RouletteWheel>();
        dungeonController = GameObject.FindGameObjectWithTag("DungeonController").GetComponent<DungeonController>();

    }

    void Update()
    {
        //Player currently choosing an option
        if(currentState == State.idle)
        {
            //Activate and debuff or player turn-based attack
            if (battleStep == 0)
            {

                battleStep = 1;
            }
            else if(battleStep == 1)
            {
                //Ability
                if (EventSystem.current.currentSelectedGameObject == battleButtons[0])
                {
                    AbilityButton();
                    battleStep = 0;
                    currentState = State.attacking;
                }
                //Item
                if (EventSystem.current.currentSelectedGameObject == battleButtons[1])
                {
                    ItemButton();
                    currentState = State.inventory;
                }
                //Run
                if (EventSystem.current.currentSelectedGameObject == battleButtons[2])
                {
                    RunButton();
                    currentState = State.running;
                }
            }
        }
        //Attacking (roulette wheel shows)
        if(currentState == State.attacking)
        {
            if (battleStep == 0)
            {

            }
            else if(battleStep == 1)
            {
                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    Debug.Log(enemyController.enemyName + "'s Turn!");
                    battleStep = 0;
                    currentState = State.enemy;
                }
            }
        }
        //Use Item
        //Player can cancel and go back if needed
        if(currentState == State.inventory)
        {

        }
        //Running
        if(currentState == State.running)
        {

        }
        //Enemy action (also roulette wheel)
        if(currentState == State.enemy)
        {
            //Activate and debuff or player turn-based attack
            if (battleStep == 0)
            {
                battleStep = 1;
            }
            else if (battleStep == 1)
            {
                dungeonController.MoveRouletteWindow(1f, "enemy");
                rouletteWheel.SetupEnemyAttacks();
                battleStep = 2;
                timer = 1.5f;
            }
            //Automatically spin wheel after short delay
            else if (battleStep == 2)
            {
                timer -= Time.deltaTime;

                if(timer <= 0)
                {
                    rouletteWheel.Spin();
                    battleStep = 3;
                }
            }
            else if(battleStep == 3)
            {
                //Enemy Attack
            }
            else if(battleStep == 4)
            {
                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    Debug.Log("Player's Turn!");
                    MoveButtons(true, 0);
                    battleStep = 0;
                    currentState = State.idle;
                }
            }
        }
    }

    //Moves once when fight begins and ends
    public void MoveUI(bool onscreen, float delay)
    {
        //Opening UI should double as Fight initialization
        //Reupdate HP Text
        curHPText.text = "" + playerController.GetCurrentHP();
        maxHPText.text = "/" + playerController.GetMaxHP();
        //Refresh rerolls
        playerController.IncreaseMaxRerolls(0);
        curRerollText.text = "" + playerController.GetCurrentRerolls();
        maxRerollText.text = "/" + playerController.GetMaxRerolls();

        if (onscreen)
        {
            GetComponent<RectTransform>().DOAnchorPosY(0, 1f).SetEase(Ease.OutCubic).SetDelay(delay);
            MoveButtons(true, delay + 0.5f);

            currentState = State.idle;
        }
        else
        {
            GetComponent<RectTransform>().DOAnchorPosY(1080, 1f).SetEase(Ease.OutCubic).SetDelay(delay);

        }
    }

    public void MoveButtons(bool onscreen, float delay)
    {
        if (onscreen)
        {
            battleButtons[0].GetComponent<RectTransform>().DOAnchorPosX(750, 1f).SetEase(Ease.InOutBack).SetDelay(delay);
            battleButtons[1].GetComponent<RectTransform>().DOAnchorPosX(750, 1f).SetEase(Ease.InOutBack).SetDelay(delay + 0.25f);
            battleButtons[2].GetComponent<RectTransform>().DOAnchorPosX(750, 1f).SetEase(Ease.InOutBack).SetDelay(delay + 0.5f);
        }
        else
        {
            battleButtons[0].GetComponent<RectTransform>().DOAnchorPosX(-400, 1f).SetEase(Ease.InOutBack).SetDelay(delay);
            battleButtons[1].GetComponent<RectTransform>().DOAnchorPosX(-400, 1f).SetEase(Ease.InOutBack).SetDelay(delay + 0.25f);
            battleButtons[2].GetComponent<RectTransform>().DOAnchorPosX(-400, 1f).SetEase(Ease.InOutBack).SetDelay(delay + 0.5f);
        }
    }

    public void GetEnemy(GameObject enemy)
    {
        this.enemy = enemy;
        enemyController = enemy.GetComponent<Enemy>();

        //Debug.Log("Enemy: " + enemyController.enemyName);
    }

    public void AbilityButton()
    {
        MoveButtons(false, 0);
        dungeonController.MoveRouletteWindow(1f, "player");
        rouletteWheel.SetupPlayerAttacks();
    }
    public void ItemButton()
    {

    }
    public void RunButton()
    {

    }

    //Final boss will destroy the run button >:)
    public void DestroyRunButton()
    {

    }

    public void ChangeBattleState(string state)
    {
        if (state.Equals("idle"))
        {
            currentState = State.idle;
        }
        if (state.Equals("inventory"))
        {
            currentState = State.inventory;
        }
        if (state.Equals("attacking"))
        {
            currentState = State.attacking;
        }
        if (state.Equals("running"))
        {
            currentState = State.running;
        }
        if (state.Equals("enemy"))
        {
            currentState = State.enemy;
        }
    }

    public void SetPlayer(GameObject player)
    {
        playerController = player.GetComponent<PlayerController>();
    }

    public void SetTimer(float time)
    {
        battleStep++;
        timer = time;
    }

    public void UpdateUIText()
    {
        //Reupdate HP Text
        curHPText.text = "" + playerController.GetCurrentHP();
        maxHPText.text = "/" + playerController.GetMaxHP();
        //Refresh rerolls
        playerController.IncreaseMaxRerolls(0);
        curRerollText.text = "" + playerController.GetCurrentRerolls();
        maxRerollText.text = "/" + playerController.GetMaxRerolls();
    }

}
