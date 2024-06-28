using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class DamageIndicator : MonoBehaviour
{
    public TextMeshProUGUI dmgText;
    public Image icon;
    public int direction = 1; //1 = right, -1 = left


    void Start()
    {
        if(direction >= 0)
        {
            transform.DOJump(new Vector2(transform.position.x + 100, transform.position.y + 50f), 100f, 1, 2f).SetEase(Ease.OutCubic);
            transform.DORotate(new Vector3(0, 0, -30f), 2f);
        }
        else
        {
            transform.DOJump(new Vector2(transform.position.x - 100, transform.position.y + 50f), 100f, 1, 2f).SetEase(Ease.OutCubic);
            transform.DORotate(new Vector3(0, 0, 30f), 2f);
        }
        dmgText.DOFade(0, 1f);
        icon.DOFade(0, 1f);
        Destroy(this.gameObject, 5f);
    }

    public void SetText(string text)
    {
        dmgText.text = text;
    }

    public void SetText(int value)
    {
        dmgText.text = value.ToString();
    }

    public void SetDirection(int value)
    {
        direction = value;
    }
}
