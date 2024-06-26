using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverText;
    GameObject player;
    public GameObject[] buttons;
    public Image backgroundColor;

    private void Start()
    {
        gameObject.SetActive(false);
        foreach (GameObject button in buttons)
        {
            button.SetActive(false);
        }
    }

    public void InitiateGameOver(GameObject player)
    {
        gameObject.SetActive(true);
        this.player = player;
        GameObject.FindGameObjectWithTag("DungeonController").GetComponent<DungeonController>().playerPosition.transform.SetParent(this.transform);
        GameObject.FindGameObjectWithTag("DungeonController").GetComponent<DungeonController>().playerPosition.transform.DOMove(Vector2.zero, 3).SetEase(Ease.OutCubic);
        backgroundColor.DOColor(new Color(0, 0, 0, 0.5f), 2f).SetEase(Ease.OutCubic);

        gameOverText.GetComponent<RectTransform>().DOAnchorPosY(-200, 2).SetEase(Ease.OutBounce).SetDelay(3f);

        StartCoroutine(ShowButtons(5f));
    }

    IEnumerator ShowButtons(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (GameObject button in buttons)
        {
            button.SetActive(true);
        }
    }

    public void Restart()
    {
        GameObject.FindGameObjectWithTag("DungeonController").GetComponent<DungeonController>().MoveCurtain(true, 0f);
        StartCoroutine(DelayedLoadScene(2f, "Dungeon"));
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadSceneAsync("Menu");
    }

    IEnumerator DelayedLoadScene(float sec, string scene)
    {
        yield return new WaitForSeconds(sec);
        SceneManager.LoadSceneAsync(scene);
    }
}
