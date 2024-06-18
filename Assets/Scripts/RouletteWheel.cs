using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class RouletteWheel : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public GameObject[] slotWindow;
    public TextMeshProUGUI[] slotText;
    public Button spinButton;
    public Color unselectedColor;
    public Color selectdColor;
    private float spinRate = 0.03f;         //The rate at which slots are selected. Gets slower over time till it stops completely
    float spinRateTimer = 0f;
    int slotIndex;
    bool spinning;

    float spinDuration = 3f;
    float spinTimer = 0f;

    private void Start()
    {
        spinning = false;
        spinTimer = spinDuration;
        spinDuration = Random.Range(2.5f, 3.5f);
        slotIndex = 0;
    }

    private void Update()
    {
        if (spinning)
        {
            spinDuration -= Time.deltaTime;
            spinRateTimer -= Time.deltaTime;

            if(spinRateTimer <= 0)
            {
                slotWindow[slotIndex].GetComponent<Image>().color = unselectedColor;
                slotIndex = (slotIndex + 1) % 6;
                slotWindow[slotIndex].GetComponent<Image>().color = selectdColor;

                spinRate += 0.005f;

                spinRateTimer = spinRate;
            }

            if(spinDuration <= 0)
            {
                spinning = false;

                Debug.Log("Landed on " + slotIndex);
                //result = slotIndex
            }
        }
    }

    public void ResetSlots()
    {
        foreach(GameObject slot in slotWindow)
        {
            slot.GetComponent<Image>().color = unselectedColor;
        }
    }

    public void SetupBlessings()
    {
        slotText[0].text = "Heal for 5 HP";
        slotText[1].text = "Increase Max HP by 3";
        slotText[2].text = "Add +1 to 2 dice faces";
        slotText[3].text = "Add +2 to 1 dice face";
        slotText[4].text = "Copy a dice face onto another dice face";
        slotText[5].text = "Grants a chance for dice face to activate twice";
    }

    public void SetupCurses()
    {

    }

    public void SetupAttacks()
    {

    }

    public void Spin()
    {
        spinning = true;
    }
}
