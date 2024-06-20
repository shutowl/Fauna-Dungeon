using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public int HP;
    [SerializeField] int DEF;

    public GameObject itemDropped;

    public string[] abilities;
    public int[] abilityValues;
    public float[] abilityLength;
    public bool[] offensive;

    DungeonController dungeonController;

    Image sprite;

    protected virtual void Start()
    {
        //dungeonController = GameObject.FindGameObjectWithTag("DungeonController").GetComponent<DungeonController>();
    }

    public void Damage(int value, float delay)
    {
        StartCoroutine(DelayedDamage(value, delay));

        //Shake
        RectTransform rect = GameObject.FindGameObjectWithTag("DungeonController").GetComponent<DungeonController>().enemy.GetComponent<RectTransform>();
        rect.DOShakeAnchorPos(0.5f, 30, 20, 45).SetDelay(delay);
    }

    public bool IsOffensive(int abilityNum)
    {
        return offensive[abilityNum];
    }

    //Ability that doesn't do direct damage this turn
    public float PlayDefensiveAbility(int abilityNum, float delay)
    {
        //Small Jump
        RectTransform rect = GameObject.FindGameObjectWithTag("DungeonController").GetComponent<DungeonController>().enemy.GetComponent<RectTransform>();
        rect.DOJumpAnchorPos(new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y), 50, 1, 0.3f).SetDelay(delay);
        return abilityLength[abilityNum] + delay;
    }

    //Ability that directly damages enemy
    public float PlayOffensiveAbility(int abilityNum, float delay)
    {
        RectTransform rect = GameObject.FindGameObjectWithTag("DungeonController").GetComponent<DungeonController>().enemy.GetComponent<RectTransform>();
        Vector2 originalPos = rect.anchoredPosition;

        //Wind up attack towards enemy
        rect.DOAnchorPosX(rect.anchoredPosition.x + 30f, 0.5f).SetEase(Ease.OutCubic).SetDelay(delay);
        rect.DOAnchorPosX(rect.anchoredPosition.x - 60f, 0.25f).SetEase(Ease.InCubic).SetDelay(0.5f + delay);
        rect.DOAnchorPosX(originalPos.x, 1f).SetEase(Ease.OutCubic).SetDelay(1f + delay);

        //Damage Player
        GameObject player = GameObject.FindGameObjectWithTag("DungeonController").GetComponent<DungeonController>().player;
        player.GetComponent<PlayerController>().Damage(abilityValues[abilityNum], 0.75f + delay);

        return abilityLength[abilityNum] + delay;
    }

    IEnumerator DelayedDamage(int value, float delay)
    {
        yield return new WaitForSeconds(delay);
        HP = Mathf.Clamp(HP - Mathf.Clamp(value - DEF, 0, 100), 0, 100);


        if (HP <= 0)
        {
            //Die after a delay
            //Drop item if regular enemy
        }

        Debug.Log("Enemy took " + value + " damage");
    }
}
