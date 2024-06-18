using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InventoryController : MonoBehaviour
{
    public float moveDuration = 1f;
    float moveTimer = 0f;
    bool cursorExitted = false;
    public float exitDuration = 1f; //Amount of time needed for inventory to automatically close when cursor exits
    float exitTimer = 1f;
    new bool enabled = true;

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
            CloseInventory();
            exitTimer = exitDuration;
            cursorExitted = false;
        }
    }

    public void OpenInventory()
    {
        if(moveTimer <= 0 && enabled)
        {
            moveTimer = moveDuration;
            GetComponent<RectTransform>().DOAnchorPosX(297.66f, moveDuration).SetEase(Ease.OutCubic);
        }
    }

    public void CloseInventory()
    {
        if(moveTimer <= 0 && enabled)
        {
            moveTimer = moveDuration;
            GetComponent<RectTransform>().DOAnchorPosX(-297.66f, moveDuration).SetEase(Ease.OutCubic);
        }
    }

    public void PointerEnter()
    {
        exitTimer = exitDuration;
        cursorExitted = false;
        if(moveTimer <= 0) OpenInventory();
    }

    public void PointerExit()
    {
        cursorExitted = true;
    }

    public void Enabled(bool enabled)
    {
        this.enabled = enabled;
    }
}
