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

    private void Start()
    {
        abilityTexts = new TextMeshProUGUI[6];
        abilityValues = new int[6];
        upgradable = new bool[6];
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

            abilityValues[1] = 3;
            abilityTexts[1].text = "Heal yourself for [" + abilityValues[1] + "] HP";
            upgradable[1] = true;

            abilityValues[2] = 2;
            abilityTexts[2].text = "Vine Attack (Deal [" + abilityValues[2] + "] DMG)";
            upgradable[2] = true;

            abilityValues[3] = 1;
            abilityTexts[3].text = "Summon ATK Sapling (Deal [" + abilityValues[3] + "] DMG each turn for 3 turns";
            upgradable[3] = true;

            abilityValues[4] = 1;
            abilityTexts[4].text = "Summon DEF Sapling(Grants +[" + abilityValues[4] + "] DEF for 3 turns)";
            upgradable[4] = true;

            abilityValues[5] = 0;
            abilityTexts[5].text = "Ara ara(Saplings powers are x2 as effective for 4 turns)";
            upgradable[5] = false;
        }
        else if (newClass.Equals("brawler"))
        {
            curClass = Class.brawler;

            abilityValues[0] = 0;
            abilityTexts[0].text = "Nothing";
            upgradable[0] = false;

            abilityValues[1] = 3;
            abilityTexts[1].text = "Meditate (Heal yourself for [" + abilityValues[1] + "] HP)";
            upgradable[1] = true;

            abilityValues[2] = 2;
            abilityTexts[2].text = "Block (+[" + abilityValues[2] + "] DEF)";
            upgradable[2] = true;

            abilityValues[3] = 1;
            abilityTexts[3].text = "Counter (+[" + abilityValues[3] + "] DEF, Deal [" + abilityValues[3] + "] DMG on hit, lasts 2 turns)";
            upgradable[3] = true;

            abilityValues[4] = 1;
            abilityTexts[4].text = "Slap (Deal [" + abilityValues[4] + "]  DMG)";
            upgradable[4] = true;

            abilityValues[5] = 2;
            abilityTexts[5].text = "Double Slap (Deal [" + abilityValues[5] + "] DMG twice)";
            upgradable[5] = true;

        }
        Debug.Log(abilityValues[0] + " " + abilityValues[1] + " " + abilityValues[2] + " " + abilityValues[3] + " " + abilityValues[4] + " " + abilityValues[5]);
    }

    public void UpgradeAbility(int abilityNum, int addedValue)
    {
        int totalValue = abilityValues[abilityNum] + addedValue;

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
                    abilityTexts[2].text = "Block (+[" + totalValue + "] DEF)";
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

    public void Damage(int value)
    {

    }

    public void Heal(int value)
    {

    }

    public void IncreaseMaxHP(int value)
    {

    }


    //During battle, check for items and give buffs based on passives.
}
