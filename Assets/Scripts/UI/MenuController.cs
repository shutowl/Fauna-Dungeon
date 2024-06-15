using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MenuController : MonoBehaviour
{
    public Button[] buttons;
    public GameObject logo;
    public GameObject curtain;
    public float buttonMoveDuration = 2f;
    float buttonMoveTimer = 0f;

    private void Start()
    {
        logo.GetComponent<RectTransform>().anchoredPosition = new Vector3(logo.GetComponent<RectTransform>().anchoredPosition.x, Mathf.Abs(logo.GetComponent<RectTransform>().anchoredPosition.y));
        buttons[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(-buttons[0].GetComponent<RectTransform>().anchoredPosition.x, buttons[0].GetComponent<RectTransform>().anchoredPosition.y);
        buttons[1].GetComponent<RectTransform>().anchoredPosition = new Vector3(-buttons[1].GetComponent<RectTransform>().anchoredPosition.x, buttons[1].GetComponent<RectTransform>().anchoredPosition.y);
        buttons[2].GetComponent<RectTransform>().anchoredPosition = new Vector3(-buttons[2].GetComponent<RectTransform>().anchoredPosition.x, buttons[2].GetComponent<RectTransform>().anchoredPosition.y);
        MoveButtons(true);
    }

    private void Update()
    {
        if(buttonMoveTimer > 0)
        {
            buttonMoveTimer -= Time.deltaTime;
            logo.GetComponent<TextHover>().enabled = false;
        }
        else if(buttonMoveTimer <= 0 && logo.GetComponent<RectTransform>().anchoredPosition.y < 0)
        {
            logo.GetComponent<TextHover>().enabled = true;
        }
    }

    public void StartGame()
    {

        MoveCurtain();
        MoveButtons(false);
        StartCoroutine(DelayedLoadScene(buttonMoveDuration, "Dungeon"));
    }

    public void MoveButtons(bool onscreen)
    {
        if(buttonMoveTimer <= 0)
        {
            buttonMoveTimer = buttonMoveDuration;
            //Move buttons from offscreen to onscreen
            if (onscreen)
            {
                logo.GetComponent<RectTransform>().DOAnchorPosY(-logo.GetComponent<RectTransform>().anchoredPosition.y, 1.5f).SetEase(Ease.InOutBack);
                buttons[0].GetComponent<RectTransform>().DOAnchorPosX(Mathf.Abs(buttons[0].GetComponent<RectTransform>().anchoredPosition.x), 1f).SetEase(Ease.InOutBack).SetDelay(0.25f);
                buttons[1].GetComponent<RectTransform>().DOAnchorPosX(Mathf.Abs(buttons[1].GetComponent<RectTransform>().anchoredPosition.x), 1f).SetEase(Ease.InOutBack).SetDelay(0.5f);
                buttons[2].GetComponent<RectTransform>().DOAnchorPosX(Mathf.Abs(buttons[2].GetComponent<RectTransform>().anchoredPosition.x), 1f).SetEase(Ease.InOutBack).SetDelay(0.75f);

            }
            //Move buttons from onscreen to offscreen
            else
            {
                logo.GetComponent<RectTransform>().DOAnchorPosY(Mathf.Abs(logo.GetComponent<RectTransform>().anchoredPosition.y), 1.5f).SetEase(Ease.InOutBack);
                buttons[0].GetComponent<RectTransform>().DOAnchorPosX(-buttons[0].GetComponent<RectTransform>().anchoredPosition.x, 1f).SetEase(Ease.InOutBack).SetDelay(0.25f);
                buttons[1].GetComponent<RectTransform>().DOAnchorPosX(-buttons[1].GetComponent<RectTransform>().anchoredPosition.x, 1f).SetEase(Ease.InOutBack).SetDelay(0.5f);
                buttons[2].GetComponent<RectTransform>().DOAnchorPosX(-buttons[2].GetComponent<RectTransform>().anchoredPosition.x, 1f).SetEase(Ease.InOutBack).SetDelay(0.75f);
            }
        }
    }

    public void MoveCurtain()
    {
        if(buttonMoveTimer <= 0)
        {
            curtain.GetComponent<RectTransform>().DOAnchorPosY(-curtain.GetComponent<RectTransform>().anchoredPosition.y, 1f).SetEase(Ease.OutCubic).SetDelay(1f);
        }
    }

    IEnumerator DelayedLoadScene(float sec, string scene)
    {
        yield return new WaitForSeconds(sec);
        SceneManager.LoadSceneAsync(scene);
    }
}
