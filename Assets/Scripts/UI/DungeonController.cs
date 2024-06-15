using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DungeonController : MonoBehaviour
{
    public GameObject curtain;
    public GameObject statWindow;
    public TextMeshProUGUI statText;
    public TextMeshProUGUI[] abilityTexts;
    public TextMeshProUGUI classText;
    public GameObject[] classButtons;
    public Button startButton;

    void Start()
    {
        curtain.SetActive(true);
        curtain.GetComponent<RectTransform>().DOAnchorPosY(Mathf.Abs(curtain.GetComponent<RectTransform>().anchoredPosition.y), 1f).SetEase(Ease.InCubic).SetDelay(1f);

        statWindow.SetActive(false);
        startButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        //-------------CLASS SELECTION------------------
        //Druid Selected
        if(EventSystem.current.currentSelectedGameObject == classButtons[0])
        {
            statWindow.SetActive(true);
            classText.text = "Druid Abilities";

            abilityTexts[0].text = "Nothing";
            abilityTexts[1].text = "Heal yourself for [3] HP";
            abilityTexts[2].text = "Vine Attack (Deal [2] DMG)";
            abilityTexts[3].text = "Summon ATK Sapling (Deal [1] DMG each turn for 3 turns)";
            abilityTexts[4].text = "Summon DEF Sapling (Grants +[1] DEF for 3 turns)";
            abilityTexts[5].text = "Ara ara (Sapling powers are twice as effective for 4 turns)";

            startButton.gameObject.SetActive(true);
        }
        //Brawler Selected
        else if (EventSystem.current.currentSelectedGameObject == classButtons[1])
        {
            statWindow.SetActive(true);
            classText.text = "Brawler Abilities";

            abilityTexts[0].text = "Nothing";
            abilityTexts[1].text = "Nothing";
            abilityTexts[2].text = "Block (+[2] DEF)";
            abilityTexts[3].text = "Counter (+[1] DEF, Deal [1] DMG on hit)";
            abilityTexts[4].text = "Slap (Deal [2] DMG)";
            abilityTexts[5].text = "Double Slap (Deal [2] DMG twice)";

            startButton.gameObject.SetActive(true);
        }
        else if(EventSystem.current.currentSelectedGameObject == null)
        {
            statWindow.SetActive(false);
            startButton.gameObject.SetActive(false);
        }
    }


    public void ReturnToMenu()
    {
        StartCoroutine(DelayedLoadScene(0f, "Menu"));
    }

    IEnumerator DelayedLoadScene(float sec, string scene)
    {
        yield return new WaitForSeconds(sec);
        SceneManager.LoadSceneAsync(scene);
    }
}
