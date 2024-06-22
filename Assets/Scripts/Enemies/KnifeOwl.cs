using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/*
Abilities:

1:  Nothing (Hoots)
2:  1 DMG (Peck)
3:  1 DMG (Peck)
4:  2 DMG (Slash)
5:  Dodge attacks for 1 turn (Fly)
6: Jumpscare (Deals 3 DMG)
‌
Item Drop: Familiar Knife (Mumei knife)
*/

public class KnifeOwl : Enemy
{
    protected override void Start()
    {
        base.Start();

        battleController = GameObject.FindGameObjectWithTag("BattleController").GetComponent<BattleController>();
        /*
         * Set values in prefab instead
         * 
        abilityValues[0] = 0;
        abilityValues[1] = 1;
        abilityValues[2] = 1;
        abilityValues[3] = 2;
        abilityValues[4] = 1;
        abilityValues[5] = 3;

        abilityLength[0] = 1;
        abilityLength[1] = 2;
        abilityLength[2] = 2;
        abilityLength[3] = 2;
        abilityLength[4] = 1;
        abilityLength[5] = 2;

        offensive[0] = false;
        offensive[1] = true;
        offensive[2] = true;
        offensive[3] = true;
        offensive[4] = false;
        offensive[5] = true;

        */
    }

    //-------ABILITIES--------
    //Nothing
    public void Nothing(float delay)
    {
        Debug.Log("Owl Nothing");
        battleController.SetTimer(delay);
    }

    //Peck
    public void Peck(int value, float delay)
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
        Debug.Log("Owl Peck");
    }

    //Slash
    public void Slash(int value, float delay)
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
        Debug.Log("Owl Slash");
    }

    //Fly
    public void Fly(float delay)
    {
        battleController.SetTimer(delay);
        Debug.Log("Owl Fly");
    }

    //Jumpscare
    public void Jumpscare(int value, float delay)
    {
        //Jumpscare 

        //Damage Player
        GameObject player = GameObject.FindGameObjectWithTag("DungeonController").GetComponent<DungeonController>().player;
        player.GetComponent<PlayerController>().Damage(value, delay);

        battleController.SetTimer(delay + 1f);
        Debug.Log("Owl Jumpscare");
    }
}
