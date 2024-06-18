using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurseStatue : MonoBehaviour
{
    GameObject dungeonController;

    void Start()
    {
        dungeonController = GameObject.FindGameObjectWithTag("DungeonController");
    }

    public void GiveCurse()
    {
        Debug.Log("Curse Obtained (start roll)");
        GetComponent<Button>().interactable = false;

        //Show Roulette Window (roulettes between 6 curses)
        dungeonController.GetComponent<DungeonController>().MoveRouletteWindow(0.5f, "curse");
    }
}
