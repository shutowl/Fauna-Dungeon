using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    GameObject dungeonController;
    public GameObject openChestText;
    public Image chestSprite;
    public Sprite openChestImage;

    private void Start()
    {
        dungeonController = GameObject.FindGameObjectWithTag("DungeonController");
    }

    public void OpenChest()
    {
        Debug.Log("Chest opened");
        GetComponent<Button>().interactable = false;
        openChestText.SetActive(false);
        chestSprite.sprite = openChestImage;
        dungeonController.GetComponent<DungeonController>().MoveItemWindow(0.5f, true);
        AudioManager.Instance.Play("ButtonClick");
    }
}
