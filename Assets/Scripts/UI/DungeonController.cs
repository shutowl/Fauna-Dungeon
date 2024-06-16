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
    public TextMeshProUGUI statText;
    public TextMeshProUGUI[] abilityTexts;
    public TextMeshProUGUI classText;
    public GameObject[] classButtons;
    public Button startButton;
    public GameObject[] playerClasses;  //0 = Druid, 1 = Brawler
    public Transform playerPosition;
    public GameObject player;
    bool runOngoing = false;

    [Header("Windows")]
    public GameObject statWindow;
    public GameObject mapStatWindow;
    public GameObject selectClassWindow;
    public GameObject mapWindow;
    public GameObject inventoryWindow;

    void Start()
    {
        curtain.SetActive(true);
        curtain.GetComponent<RectTransform>().DOAnchorPosY(Mathf.Abs(curtain.GetComponent<RectTransform>().anchoredPosition.y), 1f).SetEase(Ease.InCubic).SetDelay(0.5f);
        Destroy(curtain, 3f);

        statWindow.SetActive(false);
        startButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        //-------------CLASS SELECTION------------------
        if (Input.GetMouseButtonDown(0) && !runOngoing)
        {
            //Druid Selected
            if (EventSystem.current.currentSelectedGameObject == classButtons[0])
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

                SpawnPlayer(playerClasses[0]);
            }
            //Brawler Selected
            else if (EventSystem.current.currentSelectedGameObject == classButtons[1])
            {
                statWindow.SetActive(true);
                classText.text = "Brawler Abilities";

                abilityTexts[0].text = "Nothing";
                abilityTexts[1].text = "Nothing";
                abilityTexts[2].text = "Block (+[2] DEF)";
                abilityTexts[3].text = "Counter (+[1] DEF, Deal [1] DMG on hit, lasts 2 turns)";
                abilityTexts[4].text = "Slap (Deal [2] DMG)";
                abilityTexts[5].text = "Double Slap (Deal [2] DMG twice)";

                startButton.gameObject.SetActive(true);

                SpawnPlayer(playerClasses[1]);
            }
            else if (EventSystem.current.currentSelectedGameObject == null)
            {
                if (this.player != null) Destroy(this.player);

                statWindow.SetActive(false);
                startButton.gameObject.SetActive(false);
            }
        }
    }


    public void ReturnToMenu()
    {
        StartCoroutine(DelayedLoadScene(0f, "Menu"));
    }

    public void StartRun()
    {
        //Move all windows downwards
        //selectClassWindow.GetComponent<RectTransform>().DOAnchorPosY(selectClassWindow.GetComponent<RectTransform>().anchoredPosition.y - 1080, 2f).SetEase(Ease.InOutCubic);
        mapWindow.GetComponent<RectTransform>().DOAnchorPosY(mapWindow.GetComponent<RectTransform>().anchoredPosition.y - mapWindow.GetComponent<RectTransform>().offsetMin.y, 2f).SetEase(Ease.InOutCubic);
        inventoryWindow.GetComponent<RectTransform>().DOAnchorPosY(-inventoryWindow.GetComponent<RectTransform>().anchoredPosition.y, 2f).SetEase(Ease.InOutCubic);
        mapStatWindow.GetComponent<RectTransform>().DOAnchorPosY(-mapStatWindow.GetComponent<RectTransform>().anchoredPosition.y, 2f).SetEase(Ease.InOutCubic);

        //Move player
        playerPosition.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, -370), 2f).SetEase(Ease.InOutCubic);

        runOngoing = true;
    }

    IEnumerator DelayedLoadScene(float sec, string scene)
    {
        yield return new WaitForSeconds(sec);
        SceneManager.LoadSceneAsync(scene);
    }

    void SpawnPlayer(GameObject player)
    {
        if(this.player != null) Destroy(this.player);

        this.player = Instantiate(player, playerPosition);
        RectTransform playerRect = this.player.GetComponent<RectTransform>();
        playerRect.anchoredPosition = Vector2.zero;
        playerRect.DOJumpAnchorPos(new Vector2(playerRect.anchoredPosition.x, playerRect.anchoredPosition.y), 10, 1, 0.2f);
    }
}
