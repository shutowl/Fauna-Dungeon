using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapStatController : MonoBehaviour
{
    public float moveDuration = 1f;
    float moveTimer = 0f;
    bool cursorExitted = false;
    public float exitDuration = 1f; //Amount of time needed for inventory to automatically close when cursor exits
    float exitTimer = 1f;
    bool keepOpen = false;
    int upgradeValue = 0;
    int upgradesLeft = 0;

    public GameObject[] abilityButtons;

    private void Update()
    {
        if (moveTimer > 0)
        {
            moveTimer -= Time.deltaTime;
        }

        if (cursorExitted)
        {
            exitTimer -= Time.deltaTime;
        }
        if(exitTimer < 0)
        {
            CloseStats();
            exitTimer = exitDuration;
            cursorExitted = false;
        }
    }

    public void OpenStats(bool keepOpen)
    {
        if(moveTimer <= 0)
        {
            moveTimer = moveDuration;
            GetComponent<RectTransform>().DOAnchorPosX(-Mathf.Abs(GetComponent<RectTransform>().anchoredPosition.x), moveDuration).SetEase(Ease.OutCubic);
        }

        this.keepOpen = keepOpen;
    }

    public void CloseStats()
    {
        if(moveTimer <= 0)
        {
            moveTimer = moveDuration;
            GetComponent<RectTransform>().DOAnchorPosX(Mathf.Abs(GetComponent<RectTransform>().anchoredPosition.x), moveDuration).SetEase(Ease.OutCubic);
            keepOpen = false;
        }
    }

    public void PointerEnter()
    {
        if (!keepOpen)
        {
            exitTimer = exitDuration;
            cursorExitted = false;
            if (moveTimer <= 0) OpenStats(false);
        }
    }

    public void PointerExit()
    {
        if (!keepOpen)
        {
            cursorExitted = true;
        }
    }

    //------UPGRADE-------- (negative for curse)
    public void SetUpgradeValue(int value, int upgradesLeft)
    {
        upgradeValue = value;
        this.upgradesLeft = upgradesLeft;
    }

    public void UpgradeAbility(int abilityNum)
    {
        upgradesLeft--;
        FindAnyObjectByType<PlayerController>().UpgradeAbility(abilityNum, upgradeValue);
        EventSystem.current.currentSelectedGameObject.GetComponent<Button>().interactable = false;

        if(upgradesLeft <= 0)
        {
            CloseStats();
            FindAnyObjectByType<DungeonController>().SetNextRoomTimer(1f);

            foreach (GameObject button in abilityButtons)
            {
                button.GetComponent<Button>().interactable = false;
            }
        }
    }
}
