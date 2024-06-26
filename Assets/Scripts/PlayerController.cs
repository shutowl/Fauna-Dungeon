using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public enum Class
    {
        druid,
        brawler
    }
    public Class curClass;
    [SerializeField] int maxHP;
    [SerializeField] int currentHP;
    [SerializeField] int DEF;
    [SerializeField] int maxRerolls;
    [SerializeField] int currentRerolls;

    public TextMeshProUGUI[] abilityTexts;
    private int[] abilityValues;
    private bool[] upgradable;
    bool[] offensive;
    float[] abilityLength;

    DungeonController dungeonController;
    BattleController battleController;
    GameOver gameOverController;

    [Header("Passive Item Effects")]
    public bool diceHeld;
    public bool knifeHeld;
    public bool pebbleHeld;

    [Header("Turn-based moves")]
    public int ATKSaplingTurnsLeft;
    public int DEFSaplingTurnsLeft;
    public int enhanceTurnsLeft;
    public int blockTurnsLeft;
    public int counterTurnsLeft;
    bool enhanceSaplingsOn;
    bool ATKSaplingOn;
    bool counterOn;

    private void Start()
    {
        abilityTexts = new TextMeshProUGUI[6];
        abilityValues = new int[6];
        upgradable = new bool[6];
        offensive = new bool[6];
        abilityLength = new float[6];

        dungeonController = GameObject.FindGameObjectWithTag("DungeonController").GetComponent<DungeonController>();
        battleController = GameObject.FindGameObjectWithTag("BattleController").GetComponent<BattleController>();
        gameOverController = FindObjectOfType<GameOver>(true);

        ATKSaplingTurnsLeft = 0;
        DEFSaplingTurnsLeft = 0;
        enhanceTurnsLeft = 0;
        blockTurnsLeft = 0;
        counterTurnsLeft = 0;
        enhanceSaplingsOn = false;
        ATKSaplingOn = false;
        counterOn = false;

        diceHeld = false;
        knifeHeld = false;
        pebbleHeld = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GetComponent<RectTransform>().DOShakeAnchorPos(0.5f, 30, 20, 45).SetDelay(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GetComponent<RectTransform>().DOJumpAnchorPos(new Vector2(GetComponent<RectTransform>().anchoredPosition.x + 200f, GetComponent<RectTransform>().anchoredPosition.y - 2000f), 600f, 1, 3f).SetEase(Ease.OutCubic);
            GetComponent<RectTransform>().DORotate(new Vector3(0, 0, 3000), 3f, RotateMode.FastBeyond360).SetEase(Ease.OutCubic);
        }


    }

    public void SetupClass(string newClass)
    {
        abilityTexts[0] = GameObject.Find("MapAbilityText1").GetComponent<TextMeshProUGUI>();
        abilityTexts[1] = GameObject.Find("MapAbilityText2").GetComponent<TextMeshProUGUI>();
        abilityTexts[2] = GameObject.Find("MapAbilityText3").GetComponent<TextMeshProUGUI>();
        abilityTexts[3] = GameObject.Find("MapAbilityText4").GetComponent<TextMeshProUGUI>();
        abilityTexts[4] = GameObject.Find("MapAbilityText5").GetComponent<TextMeshProUGUI>();
        abilityTexts[5] = GameObject.Find("MapAbilityText6").GetComponent<TextMeshProUGUI>();

        if (newClass.Equals("druid"))
        {
            curClass = Class.druid;

            abilityValues[0] = 0;
            abilityTexts[0].text = "Nothing";
            upgradable[0] = false;
            offensive[0] = false;

            abilityValues[1] = 3;
            abilityTexts[1].text = "Heal yourself for [" + abilityValues[1] + "] HP";
            upgradable[1] = true;
            offensive[1] = false;

            abilityValues[2] = 2;
            abilityTexts[2].text = "Vine Attack (Deal [" + abilityValues[2] + "] DMG)";
            upgradable[2] = true;
            offensive[2] = true;

            abilityValues[3] = 1;
            abilityTexts[3].text = "Summon ATK Sapling (Deal [" + abilityValues[3] + "] DMG each turn for 3 turns";
            upgradable[3] = true;
            offensive[3] = false;

            abilityValues[4] = 1;
            abilityTexts[4].text = "Summon DEF Sapling(Grants +[" + abilityValues[4] + "] DEF for 3 turns)";
            upgradable[4] = true;
            offensive[4] = false;

            abilityValues[5] = 0;
            abilityTexts[5].text = "Ara ara(Saplings powers are x2 as effective for 4 turns)";
            upgradable[5] = false;
            offensive[5] = false;

            maxHP = currentHP = 10;
            DEF = 0;
            maxRerolls = currentRerolls = 3;
        }
        else if (newClass.Equals("brawler"))
        {
            curClass = Class.brawler;

            abilityValues[0] = 0;
            abilityTexts[0].text = "Nothing";
            upgradable[0] = false;
            offensive[0] = false;

            abilityValues[1] = 3;
            abilityTexts[1].text = "Meditate (Heal yourself for [" + abilityValues[1] + "] HP)";
            upgradable[1] = true;
            offensive[1] = false;

            abilityValues[2] = 2;
            abilityTexts[2].text = "Block (+[" + abilityValues[2] + "] DEF for 1 turn)";
            upgradable[2] = true;
            offensive[2] = false;

            abilityValues[3] = 1;
            abilityTexts[3].text = "Counter (+[" + abilityValues[3] + "] DEF, Deal [" + abilityValues[3] + "] DMG on hit, lasts 2 turns)";
            upgradable[3] = true;
            offensive[3] = false;

            abilityValues[4] = 2;
            abilityTexts[4].text = "Slap (Deal [" + abilityValues[4] + "]  DMG)";
            upgradable[4] = true;
            offensive[4] = true;

            abilityValues[5] = 2;
            abilityTexts[5].text = "Double Slap (Deal [" + abilityValues[5] + "] DMG twice)";
            upgradable[5] = true;
            offensive[5] = true;

            maxHP = currentHP = 15;
            DEF = 0;
            maxRerolls = currentRerolls = 2;
        }

        //Offensive = 2s length
        //Defensive = 1s length
        for (int i = 0; i < offensive.Length; i++)
        {
            if (offensive[i]) abilityLength[i] = 2f;
            else abilityLength[i] = 1f;
        }

        GameObject.Find("MapStatText").GetComponent<TextMeshProUGUI>().text = "Max HP: " + maxHP + "\nMax Rerolls: " + maxRerolls;
    }

    public void UpgradeAbility(int abilityNum, int addedValue)
    {
        int totalValue = Mathf.Clamp(abilityValues[abilityNum] + addedValue, 0, 20);

        //abilityTexts[abilityNum].text = abilityTexts[abilityNum].text.Replace((char)abilityValues[abilityNum], (char)totalValue);
        abilityValues[abilityNum] = totalValue;

        if(curClass == Class.druid)
        {
            switch (abilityNum)
            {
                case 1:
                    abilityTexts[1].text = "Heal yourself for [" + totalValue + "] HP";
                    break;
                case 2:
                    abilityTexts[2].text = "Vine Attack (Deal [" + totalValue + "] DMG)";
                    break;
                case 3:
                    abilityTexts[3].text = "Summon ATK Sapling (Deal [" + totalValue + "] DMG each turn for 3 turns";
                    break;
                case 4:
                    abilityTexts[4].text = "Summon DEF Sapling(Grants +[" + totalValue + "] DEF for 3 turns)";
                    break;
            }
        }
        else //if brawler
        {
            switch (abilityNum)
            {
                case 1:
                    abilityTexts[1].text = "Meditate (Heal yourself for [" + totalValue + "] HP)";
                    break;
                case 2:
                    abilityTexts[2].text = "Block (+[" + totalValue + "] DEF) for 1 turn";
                    break;
                case 3:
                    abilityTexts[3].text = "Counter (+[" + totalValue + "] DEF, Deal [" + totalValue + "] DMG on hit, lasts 2 turns)";
                    break;
                case 4:
                    abilityTexts[4].text = "Slap (Deal [" + totalValue + "]  DMG)";
                    break;
                case 5:
                    abilityTexts[5].text = "Double Slap (Deal [" + totalValue + "] DMG twice)";
                    break;
            }
        }
    }

    /*        slotText[0].text = "Heal for 5 HP";
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
        slotText[5].text = "Subtract [2] to 1 die face";*/

    public void Damage(int value, float delay)
    {
        StartCoroutine(DelayedDamage(value, delay));

        //Animations
        //Player flash red

        //Player sprite change

        //Shake
        RectTransform rect = GetComponent<RectTransform>();
        rect.DOShakeAnchorPos(0.5f, 30, 20, 45).SetDelay(delay);
    }

    public void Heal(int value)
    {
        currentHP = Mathf.Clamp(currentHP + value, 0, maxHP);
        battleController.UpdateUIText();
    }

    public void IncreaseMaxHP(int value)
    {
        maxHP += value;
        currentHP = maxHP;
    }

    public void IncreaseMaxRerolls(int value)
    {
        maxRerolls += value;
        currentRerolls = maxRerolls;
    }

    public bool IsOffensive(int abilityNum)
    {
        return offensive[abilityNum];
    }

    public int GetCurrentHP() { return currentHP; }
    public int GetMaxHP() { return maxHP; }
    public int GetCurrentRerolls(){ return currentRerolls; }
    public int GetMaxRerolls() { return maxRerolls; }
    public void IncreaseCurrentRerolls(int value) { currentRerolls += value; }

    //During battle, check for items and give buffs based on passives.

    IEnumerator DelayedDamage(int value, float delay)
    {
        yield return new WaitForSeconds(delay);
        if(pebbleHeld) currentHP = Mathf.Clamp(currentHP - (Mathf.Clamp((value - (DEF + 1)), 0, 10)), 0, maxHP);
        else currentHP = Mathf.Clamp(currentHP - (Mathf.Clamp((value - DEF), 0, 10)), 0, maxHP);

        //Brawler Counter
        if (counterOn)
        {
            //Damage Enemy
            GameObject enemy = GameObject.FindGameObjectWithTag("DungeonController").GetComponent<DungeonController>().enemy;
            enemy.GetComponent<Enemy>().Damage(abilityValues[3], 0.25f);
        }

        if (currentHP == 0)
        {
            //game over
            gameOverController.InitiateGameOver(dungeonController.playerPosition.gameObject);
        }

        battleController.UpdateUIText();
    }

    //------ABILITIES---------

    //GLOBAL
    //Nothing
    public void Nothing(float delay)
    {
        battleController.SetTimer(delay);
        Debug.Log("Player does nothing");
    }

    //DRUID

    //Heal
    public void DruidHeal(float delay)
    {
        //Small Jump
        RectTransform rect = GetComponent<RectTransform>();
        rect.DOJumpAnchorPos(new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y), 50, 1, 0.3f).SetDelay(delay);

        Heal(abilityValues[1]);

        battleController.SetTimer(delay);
        Debug.Log("Player heals");
    }

    //Vine Attack
    public void DruidVine(float delay)
    {
        RectTransform rect = GetComponent<RectTransform>();
        Vector2 originalPos = rect.anchoredPosition;

        //Wind up attack towards enemy
        rect.DOAnchorPosX(rect.anchoredPosition.x - 30f, 0.5f).SetEase(Ease.OutCubic).SetDelay(delay);
        rect.DOAnchorPosX(rect.anchoredPosition.x + 60f, 0.25f).SetEase(Ease.InCubic).SetDelay(0.5f + delay);
        rect.DOAnchorPosX(originalPos.x, 1f).SetEase(Ease.OutCubic).SetDelay(1f + delay);

        //Damage Enemy
        GameObject enemy = GameObject.FindGameObjectWithTag("DungeonController").GetComponent<DungeonController>().enemy;
        if(knifeHeld) enemy.GetComponent<Enemy>().Damage(abilityValues[4] + 1, 0.75f + delay);
        else enemy.GetComponent<Enemy>().Damage(abilityValues[4], 0.75f + delay);

        battleController.SetTimer(delay);
        Debug.Log("Player vine");
    }

    //Summon Attack Sapling
    public void SummonATKSapling(float delay)
    {
        //Small Jump
        RectTransform rect = GetComponent<RectTransform>();
        rect.DOJumpAnchorPos(new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y), 50, 1, 0.3f).SetDelay(delay);

        ATKSaplingTurnsLeft = 3;

        battleController.SetTimer(delay);
        Debug.Log("Player ATK Sapling");
    }

    //Summon DEF Sapling
    public void SummonDEFSapling(float delay)
    {
        //Small Jump
        RectTransform rect = GetComponent<RectTransform>();
        rect.DOJumpAnchorPos(new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y), 50, 1, 0.3f).SetDelay(delay);

        DEFSaplingTurnsLeft = 3;
        if (enhanceSaplingsOn)
        {
            DEF = abilityValues[4] * 2;
        }
        else
        {
            DEF = abilityValues[4];
        }

        battleController.SetTimer(delay);
        Debug.Log("Player DEF Sapling");
    }

    //Enhance buff
    public void EnhanceSaplings(float delay)
    {
        //Small Jump
        RectTransform rect = GetComponent<RectTransform>();
        rect.DOJumpAnchorPos(new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y), 50, 1, 0.3f).SetDelay(delay);

        enhanceTurnsLeft = 4;
        if (enhanceTurnsLeft > 0)
        {
            enhanceSaplingsOn = true;
        }
        else
        {
            enhanceSaplingsOn = false;
        }

        battleController.SetTimer(delay);
        Debug.Log("Player Enhance Saplings");
    }


    //Brawler

    //Meditate
    public void BrawlerHeal(float delay)
    {
        //Small Jump
        RectTransform rect = GetComponent<RectTransform>();
        rect.DOJumpAnchorPos(new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y), 50, 1, 0.3f).SetDelay(delay);

        Heal(abilityValues[1]);

        battleController.SetTimer(delay);
        Debug.Log("Player Brawl heal");
    }

    //Block
    public void BrawlerBlock(float delay)
    {
        //Small Jump
        RectTransform rect = GetComponent<RectTransform>();
        rect.DOJumpAnchorPos(new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y), 50, 1, 0.3f).SetDelay(delay);

        blockTurnsLeft = 1;
        if (counterTurnsLeft > 0)
        {
            DEF = abilityValues[2] + abilityValues[3];
        }
        else
        {
            DEF = abilityValues[2];
        }

        battleController.SetTimer(delay);
        Debug.Log("Player Block");
    }

    //Counter
    public void BrawlerCounter(float delay)
    {
        //Small Jump
        RectTransform rect = GetComponent<RectTransform>();
        rect.DOJumpAnchorPos(new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y), 50, 1, 0.3f).SetDelay(delay);

        counterTurnsLeft = 2;
        counterOn = true;
        if(blockTurnsLeft > 0)
        {
            DEF = abilityValues[3] + abilityValues[2];
        }
        else
        {
            DEF = abilityValues[3];
        }


        battleController.SetTimer(delay);
        Debug.Log("Player Counter");
    }

    //Slap
    public void BrawlerSlap(float delay)
    {
        RectTransform rect = GetComponent<RectTransform>();
        Vector2 originalPos = rect.anchoredPosition;

        //Wind up attack towards enemy
        rect.DOAnchorPosX(rect.anchoredPosition.x - 30f, 0.5f).SetEase(Ease.OutCubic).SetDelay(delay);
        rect.DOAnchorPosX(rect.anchoredPosition.x + 60f, 0.25f).SetEase(Ease.InCubic).SetDelay(0.5f + delay);
        rect.DOAnchorPosX(originalPos.x, 1f).SetEase(Ease.OutCubic).SetDelay(1f + delay);

        //Damage Enemy
        GameObject enemy = GameObject.FindGameObjectWithTag("DungeonController").GetComponent<DungeonController>().enemy;
        if(knifeHeld) enemy.GetComponent<Enemy>().Damage(abilityValues[4] + 1, 0.75f + delay);
        else enemy.GetComponent<Enemy>().Damage(abilityValues[4], 0.75f + delay);

        battleController.SetTimer(delay + 1f);
        Debug.Log("Player Slap");
    }

    //Double Slap
    public void BrawlerDoubleSlap(float delay)
    {
        RectTransform rect = GetComponent<RectTransform>();
        Vector2 originalPos = rect.anchoredPosition;

        //Wind up attack towards enemy
        rect.DOAnchorPosX(rect.anchoredPosition.x - 30f, 0.5f).SetEase(Ease.OutCubic).SetDelay(delay);
        rect.DOAnchorPosX(rect.anchoredPosition.x + 60f, 0.25f).SetEase(Ease.InCubic).SetDelay(0.5f + delay);
        rect.DOAnchorPosX(originalPos.x, 1f).SetEase(Ease.OutCubic).SetDelay(0.75f + delay);

        //Damage Enemy
        GameObject enemy = GameObject.FindGameObjectWithTag("DungeonController").GetComponent<DungeonController>().enemy;
        if(knifeHeld) enemy.GetComponent<Enemy>().Damage(abilityValues[5] + 1, 0.75f + delay);
        else enemy.GetComponent<Enemy>().Damage(abilityValues[5], 0.75f + delay);

        //Wind up attack towards enemy again
        rect.DOAnchorPosX(rect.anchoredPosition.x - 30f, 0.5f).SetEase(Ease.OutCubic).SetDelay(1.5f + delay);
        rect.DOAnchorPosX(rect.anchoredPosition.x + 60f, 0.25f).SetEase(Ease.InCubic).SetDelay(1.5f + 0.5f + delay);
        rect.DOAnchorPosX(originalPos.x, 1f).SetEase(Ease.OutCubic).SetDelay(1.5f + 0.75f + delay);

        //Damage Enemy
        if(knifeHeld) enemy.GetComponent<Enemy>().Damage(abilityValues[5] + 1, 1.5f + 0.75f + delay);
        else enemy.GetComponent<Enemy>().Damage(abilityValues[5], 1.5f + 0.75f + delay);

        battleController.SetTimer(delay + 3f);
        Debug.Log("Player x2 Slap");
    }




    //Ability that doesn't do direct damage this turn
    public float PlayDefensiveAbility(int abilityNum, float delay)
    {
        //Small Jump
        RectTransform rect = GetComponent<RectTransform>();
        rect.DOJumpAnchorPos(new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y), 50, 1, 0.3f).SetDelay(delay);

        return abilityLength[abilityNum] + delay;
    }

    //Ability that directly damages enemy
    public void PlayOffensiveAbility(int damage, float delay)
    {
        RectTransform rect = GetComponent<RectTransform>();
        Vector2 originalPos = rect.anchoredPosition;

        //Wind up attack towards enemy
        rect.DOAnchorPosX(rect.anchoredPosition.x - 30f, 0.5f).SetEase(Ease.OutCubic).SetDelay(delay);
        rect.DOAnchorPosX(rect.anchoredPosition.x + 60f, 0.25f).SetEase(Ease.InCubic).SetDelay(0.5f + delay);
        rect.DOAnchorPosX(originalPos.x, 1f).SetEase(Ease.OutCubic).SetDelay(1f + delay);

        //Damage Enemy
        GameObject enemy = GameObject.FindGameObjectWithTag("DungeonController").GetComponent<DungeonController>().enemy;
        if(knifeHeld) enemy.GetComponent<Enemy>().Damage(damage + 1, 0.75f + delay);
        else enemy.GetComponent<Enemy>().Damage(damage, 0.75f + delay);
    }

    public int GetAbilityValue(int abilityNum)
    {
        return abilityValues[abilityNum];
    }

    public void DecrementTurn()
    {
        if(ATKSaplingTurnsLeft > 0) ATKSaplingTurnsLeft--;
        if (DEFSaplingTurnsLeft > 0) DEFSaplingTurnsLeft--;
        if (enhanceTurnsLeft > 0) enhanceTurnsLeft--;
        if (blockTurnsLeft > 0) blockTurnsLeft--;
        if (counterTurnsLeft > 0) counterTurnsLeft--;
    }

    public void IncrementTurn(int value)
    {
        if (ATKSaplingTurnsLeft > 0) ATKSaplingTurnsLeft += value;
        if (DEFSaplingTurnsLeft > 0) DEFSaplingTurnsLeft += value;
        if (enhanceTurnsLeft > 0) enhanceTurnsLeft += value;
        if (blockTurnsLeft > 0) blockTurnsLeft += value;
        if (counterTurnsLeft > 0) counterTurnsLeft += value;
    }

    public void CalculateBuffs()
    {
        DecrementTurn();

        //Druid DEF
        if(DEFSaplingTurnsLeft > 0)
        {
            if (enhanceSaplingsOn)
            {
                DEF = abilityValues[4] * 2;
            }
            else
            {
                DEF = abilityValues[4];
            }
        }
        else
        {
            DEF = 0;
        }
        //Druid ATK
        if(ATKSaplingTurnsLeft > 0)
        {
            GameObject enemy = GameObject.FindGameObjectWithTag("DungeonController").GetComponent<DungeonController>().enemy;
            if (enhanceSaplingsOn)
            {
                if(knifeHeld) enemy.GetComponent<Enemy>().Damage((abilityValues[3] + 1) * 2, 1f);
                else enemy.GetComponent<Enemy>().Damage(abilityValues[3] * 2, 1f);
            }
            else
            {
                if(knifeHeld) enemy.GetComponent<Enemy>().Damage(abilityValues[3] + 1, 1f);
                else enemy.GetComponent<Enemy>().Damage(abilityValues[3], 1f);
            }
        }
        //Enhance
        if(enhanceTurnsLeft > 0)
        {
            enhanceSaplingsOn = true;
        }
        else
        {
            enhanceSaplingsOn = false;
        }

        //Brawler DEF
        if(blockTurnsLeft > 0)
        {
            if(counterTurnsLeft > 0)
            {
                DEF = abilityValues[2] + abilityValues[3];
            }
            else
            {
                DEF = abilityValues[2];
            }
        }
        else if(counterTurnsLeft > 0)
        {
            DEF = abilityValues[3];
        }
        else
        {
            DEF = 0;
            counterOn = false;
        }
    }
}
