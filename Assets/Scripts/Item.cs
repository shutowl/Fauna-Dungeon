using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    GameObject dungeonController;
    public string itemName;
    [TextArea(7, 20)]
    public string itemDesc;

    private void Start()
    {
        dungeonController = GameObject.FindGameObjectWithTag("DungeonController");
    }

    public void ShowDescription(bool visible)
    {
        dungeonController.GetComponent<DungeonController>().itemDescWindow.SetActive(visible);
        dungeonController.GetComponent<DungeonController>().ChangeItemDescription(itemName, itemDesc);
    }

    public void SelectItem()
    {
        Debug.Log(itemName + " selected!");

        dungeonController.GetComponent<DungeonController>().ObtainItem(this.gameObject);
        dungeonController.GetComponent<DungeonController>().CloseItemWindow(2f);
    }
}
