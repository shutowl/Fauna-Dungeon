using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapStatController : MonoBehaviour
{
    public float moveDuration = 1f;
    float moveTimer = 0f;
    bool cursorExitted = false;
    public float exitDuration = 1f; //Amount of time needed for inventory to automatically close when cursor exits
    float exitTimer = 1f;

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

    public void OpenStats()
    {
        if(moveTimer <= 0)
        {
            moveTimer = moveDuration;
            GetComponent<RectTransform>().DOAnchorPosX(-Mathf.Abs(GetComponent<RectTransform>().anchoredPosition.x), moveDuration).SetEase(Ease.OutCubic);
        }
    }

    public void CloseStats()
    {
        if(moveTimer <= 0)
        {
            moveTimer = moveDuration;
            GetComponent<RectTransform>().DOAnchorPosX(Mathf.Abs(GetComponent<RectTransform>().anchoredPosition.x), moveDuration).SetEase(Ease.OutCubic);
        }
    }

    public void PointerEnter()
    {
        exitTimer = exitDuration;
        cursorExitted = false;
        if(moveTimer <= 0) OpenStats();
    }

    public void PointerExit()
    {
        cursorExitted = true;
    }
}
