using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

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
    int flyTurnsLeft;
    public GameObject jumpscareImage;

    protected override void Start()
    {
        base.Start();

        battleController = GameObject.FindGameObjectWithTag("BattleController").GetComponent<BattleController>();

        flyTurnsLeft = 0;
    }

    //-------ABILITIES--------
    //Nothing
    public void Nothing(float delay)
    {
        Debug.Log("Owl Nothing");
        battleController.SetTimer(delay);
        AudioManager.Instance.Play("Hoot");
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
        flyTurnsLeft = 1;
        invulnerable = true;
        Debug.Log("Owl Fly");
    }

    //Jumpscare
    public void Jumpscare(int value, float delay)
    {
        //Jumpscare 
        jumpscareImage.GetComponent<Image>().DOColor(new Color(1, 1, 1, 1), 1).SetEase(Ease.Flash, 10, 0.5f).SetDelay(delay);
        //Damage Player
        GameObject player = GameObject.FindGameObjectWithTag("DungeonController").GetComponent<DungeonController>().player;
        player.GetComponent<PlayerController>().Damage(value, delay);

        battleController.SetTimer(delay + 1f);
        AudioManager.Instance.Play("Horror", delay);
        Debug.Log("Owl Jumpscare");
    }

    public override void CalculateBuffs()
    {
        DecrementTurn();

        if (flyTurnsLeft > 0) invulnerable = true;
        else invulnerable = false;
    }

    public override void DecrementTurn()
    {
        if (flyTurnsLeft > 0) flyTurnsLeft--;
    }
}
