using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoaiStatue : Enemy
{
    int hardenTurnsLeft;

    protected override void Start()
    {
        base.Start();

        hardenTurnsLeft = 0;

        battleController = GameObject.FindGameObjectWithTag("BattleController").GetComponent<BattleController>();
    }

    //-------ABILITIES--------
    //Nothing
    public void Nothing(float delay)
    {
        Debug.Log("Moai Nothing");
        battleController.SetTimer(delay);
    }

    //Throw Pebble
    public void ThrowPebble(int value, float delay)
    {
        RectTransform rect = GameObject.FindGameObjectWithTag("DungeonController").GetComponent<DungeonController>().enemy.GetComponent<RectTransform>();
        Vector2 originalPos = rect.anchoredPosition;

        //Wind up attack towards enemy
        rect.DOAnchorPosX(rect.anchoredPosition.x + 30f, 0.5f).SetEase(Ease.OutCubic).SetDelay(delay);
        rect.DOAnchorPosX(rect.anchoredPosition.x - 60f, 0.25f).SetEase(Ease.InCubic).SetDelay(0.5f + delay);
        rect.DOAnchorPosX(originalPos.x, 1f).SetEase(Ease.OutCubic).SetDelay(1f + delay);

        //Damage Player
        GameObject player = GameObject.FindGameObjectWithTag("DungeonController").GetComponent<DungeonController>().player;
        player.GetComponent<PlayerController>().Damage(value, 0.75f + delay);

        battleController.SetTimer(delay + 1f);
        Debug.Log("Moai Pebble");
    }

    //RockSlide
    public void RockSlide(int value, float delay)
    {
        RectTransform rect = GameObject.FindGameObjectWithTag("DungeonController").GetComponent<DungeonController>().enemy.GetComponent<RectTransform>();
        Vector2 originalPos = rect.anchoredPosition;

        //Wind up attack towards enemy
        rect.DOAnchorPosX(rect.anchoredPosition.x + 30f, 0.5f).SetEase(Ease.OutCubic).SetDelay(delay);
        rect.DOAnchorPosX(rect.anchoredPosition.x - 60f, 0.25f).SetEase(Ease.InCubic).SetDelay(0.5f + delay);
        rect.DOAnchorPosX(originalPos.x, 1f).SetEase(Ease.OutCubic).SetDelay(1f + delay);

        //Replace above with this if time permits
        /*
         * Pebbles fall from above the window at random x coordinates near the player for 1.5s
         * 
         */

        //Damage Player
        GameObject player = GameObject.FindGameObjectWithTag("DungeonController").GetComponent<DungeonController>().player;
        player.GetComponent<PlayerController>().Damage(value, 0.75f + delay);

        battleController.SetTimer(delay + 1f);
        Debug.Log("Moai Rock Slide");
    }

    //Harden
    public void Harden(float delay)
    {
        battleController.SetTimer(delay);
        hardenTurnsLeft = 1;
        DEF = 1;

        Debug.Log("Moai Harden");
    }

    public override void CalculateBuffs()
    {
        DecrementTurn();

        if(hardenTurnsLeft > 0)
        {
            DEF = 1;
        }
        else
        {
            DEF = 0;
        }
    }

    public override void DecrementTurn()
    {
        if (hardenTurnsLeft > 0) hardenTurnsLeft--;
    }
}
