using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    GameObject dungeonController;
    public string itemName;
    [TextArea(7, 20)]
    public string itemDesc;
    bool inInventory;
    [SerializeField] int inventorySlotNum;

    private void Start()
    {
        inInventory = false;
        dungeonController = GameObject.FindGameObjectWithTag("DungeonController");
    }

    public void ShowDescription(bool visible)
    {
        dungeonController.GetComponent<DungeonController>().itemDescWindow.SetActive(visible);
        dungeonController.GetComponent<DungeonController>().ChangeItemDescription(itemName, itemDesc);
    }

    public void SelectItem()
    {
        if (!inInventory)
        {
            inventorySlotNum = dungeonController.GetComponent<DungeonController>().ObtainItem(this.gameObject);
            dungeonController.GetComponent<DungeonController>().CloseItemWindow(0.5f);

            dungeonController = GameObject.FindGameObjectWithTag("DungeonController");

            //Player holds item (Passive Abilities)
            //Dice
            if (itemName.Equals("Chaotic Dice"))
            {
                dungeonController.GetComponent<DungeonController>().player.GetComponent<PlayerController>().IncreaseMaxRerolls(1);
            }
            //Knife
            if(itemName.Equals("Familiar Knife"))
            {
                dungeonController.GetComponent<DungeonController>().player.GetComponent<PlayerController>().knifeHeld = true;
            }
            //Pebble
            if (itemName.Equals("Pebble"))
            {
                dungeonController.GetComponent<DungeonController>().player.GetComponent<PlayerController>().pebbleHeld = true;
            }

            inInventory = true;
            Debug.Log(itemName + " obtained!");
            AudioManager.Instance.Play("GetItem");
        }
        else
        {
            //Use item in battle (Active Abilities)

            //Controllers
            dungeonController = GameObject.FindGameObjectWithTag("DungeonController");
            BattleController battleController = GameObject.FindGameObjectWithTag("BattleController").GetComponent<BattleController>();
            PlayerController playerController = dungeonController.GetComponent<DungeonController>().player.GetComponent<PlayerController>();

            //Blue Clock (add 3 turns to active turn based moves)
            if (itemName.Equals("Blue Clock"))
            {
                dungeonController.GetComponent<DungeonController>().player.GetComponent<PlayerController>().IncrementTurn(3);
                AudioManager.Instance.Play("GetItem");
            }

            //Cat plushie (heal 5 hp)
            else if(itemName.Equals("Cat Plushie"))
            {
                dungeonController.GetComponent<DungeonController>().player.GetComponent<PlayerController>().Heal(5);            }

            //Chaotic Dice (4 random actions)
            else if(itemName.Equals("Chaotic Dice"))
            {
                switch (Random.Range(0, 4))
                {
                    case 0: dungeonController.GetComponent<DungeonController>().player.GetComponent<PlayerController>().Heal(3); break;
                    case 1: dungeonController.GetComponent<DungeonController>().player.GetComponent<PlayerController>().Damage(3, 1f); break;
                    case 2: dungeonController.GetComponent<DungeonController>().enemy.GetComponent<Enemy>().Damage(2, 1f); break;
                    case 3: dungeonController.GetComponent<DungeonController>().enemy.GetComponent<Enemy>().Damage(4, 1f); break;
                }
                switch (Random.Range(0, 4))
                {
                    case 0: dungeonController.GetComponent<DungeonController>().player.GetComponent<PlayerController>().Heal(3); break;
                    case 1: dungeonController.GetComponent<DungeonController>().player.GetComponent<PlayerController>().Damage(3, 1.5f); break;
                    case 2: dungeonController.GetComponent<DungeonController>().enemy.GetComponent<Enemy>().Damage(2, 1.5f); break;
                    case 3: dungeonController.GetComponent<DungeonController>().enemy.GetComponent<Enemy>().Damage(4, 1.5f); break;
                }
                switch (Random.Range(0, 4))
                {
                    case 0: dungeonController.GetComponent<DungeonController>().player.GetComponent<PlayerController>().Heal(3); break;
                    case 1: dungeonController.GetComponent<DungeonController>().player.GetComponent<PlayerController>().Damage(3, 2f); break;
                    case 2: dungeonController.GetComponent<DungeonController>().enemy.GetComponent<Enemy>().Damage(2, 2f); break;
                    case 3: dungeonController.GetComponent<DungeonController>().enemy.GetComponent<Enemy>().Damage(4, 2f); break;
                }
                switch (Random.Range(0, 4))
                {
                    case 0: dungeonController.GetComponent<DungeonController>().player.GetComponent<PlayerController>().Heal(3); break;
                    case 1: dungeonController.GetComponent<DungeonController>().player.GetComponent<PlayerController>().Damage(3, 2.5f); break;
                    case 2: dungeonController.GetComponent<DungeonController>().enemy.GetComponent<Enemy>().Damage(2, 2.5f); break;
                    case 3: dungeonController.GetComponent<DungeonController>().enemy.GetComponent<Enemy>().Damage(4, 2.5f); break;
                }
                dungeonController.GetComponent<DungeonController>().player.GetComponent<PlayerController>().IncreaseMaxRerolls(-1);
                battleController.UpdateUIText();
            }

            //Knife (deal 4 dmg)
            else if (itemName.Equals("Familiar Knife"))
            {
                dungeonController.GetComponent<DungeonController>().player.GetComponent<PlayerController>().PlayOffensiveAbility(4, 1f);
                dungeonController.GetComponent<DungeonController>().player.GetComponent<PlayerController>().knifeHeld = false;
            }

            //Pebble (randomly deal 1-4 dmg)
            else if (itemName.Equals("Pebble"))
            {
                dungeonController.GetComponent<DungeonController>().enemy.GetComponent<Enemy>().Damage(Random.Range(1, 5), 1f);
                dungeonController.GetComponent<DungeonController>().player.GetComponent<PlayerController>().pebbleHeld = false;
            }

            //Disable item buttons on use (one item per use)
            GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
            foreach (GameObject button in items)
            {
                button.GetComponent<Button>().interactable = true;
            }

            Debug.Log(itemName + " used!");
            dungeonController.GetComponent<DungeonController>().inventoryFilled[inventorySlotNum] = false;
            ShowDescription(false);
            battleController.SetTimer(1f);
            dungeonController.GetComponent<DungeonController>().inventoryWindow.GetComponent<InventoryController>().CloseInventory();
            Destroy(this.gameObject);
            AudioManager.Instance.Play("ButtonClick");
        }
    }
}
