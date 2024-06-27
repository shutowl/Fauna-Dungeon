using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JumpyButtons : MonoBehaviour
{
    public float strength;
    public float duration;
    float timer = 0f;

    public void Jump()
    {
        if (!DOTween.IsTweening(GetComponent<RectTransform>()))
        {
            if (timer < 0)
            {
                RectTransform rect = GetComponent<RectTransform>();
                rect.DOJumpAnchorPos(new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y), strength, 1, duration);
                timer = duration;
            }
        }
        AudioManager.Instance.Play("ButtonHover");
    }

    private void Update()
    {
        if(timer >= 0)
        {
            timer -= Time.deltaTime;
        }
    }
}
