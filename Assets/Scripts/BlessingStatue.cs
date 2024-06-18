using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlessingStatue : MonoBehaviour
{
    GameObject dungeonController;

    void Start()
    {
        dungeonController = GameObject.FindGameObjectWithTag("DungeonController");
    }

    public void GiveBlessing()
    {
        Debug.Log("Blessing Obtained (start roll)");
        GetComponent<Button>().interactable = false;

        //Show Roulette Window (roulettes between 6 random blessings)
        dungeonController.GetComponent<DungeonController>().MoveRouletteWindow(0.5f, "blessing");
    }
}
