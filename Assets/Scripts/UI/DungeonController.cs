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
    public enum DungeonState
    {
        classSelect,
        map,
        room
    }
    public enum RoomType
    {
        item,
        blessing,
        curse,
        enemy,
    }
    public DungeonState currentState;
    public RoomType currentRoom;

    public GameObject curtain;
    public TextMeshProUGUI statText;
    public TextMeshProUGUI[] abilityTexts;
    public TextMeshProUGUI classText;
    public GameObject[] classButtons;
    public Button startButton;
    public GameObject[] playerClasses;  //0 = Druid, 1 = Brawler
    public Transform playerPosition;
    public GameObject player;

    [Header("Windows")]
    public GameObject statWindow;
    public GameObject mapStatWindow;
    public GameObject selectClassWindow;
    public GameObject mapWindow;
    public GameObject inventoryWindow;
    public GameObject roomWindow;

    [Header("Map")]
    public GameObject pickRoomText;
    int currentFloor = 0;
    public GameObject[] roomButtons;
    GameObject lastRoom;
    public Vector2 bottomMapPos;

    [Header("Room")]
    public Transform playerRoomPosition;
    public Transform roomObjectPosition;
    public Transform enemyRoomPosition;
    int roomStep = 0;
    float roomTimer = 0f;
    public GameObject chest;
    public GameObject blessingStatue;
    public GameObject curseStatue;
    public GameObject[] enemies;

    [Header("Item")]
    public GameObject itemWindow;
    public GameObject itemDescWindow;
    public GameObject[] itemSlots;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescText;
    public List<GameObject> itemPool;

    [Header("Inventory")]
    public GameObject[] inventorySlots;
    bool[] inventoryFilled;

    [Header("Roulette")]
    public GameObject rouletteWindow;

    [Header("Player")]
    string playerClass;

    void Start()
    {
        currentState = DungeonState.classSelect;
        curtain.SetActive(true);
        MoveCurtain(false, 0.5f);
        //Destroy(curtain, 3f);

        statWindow.SetActive(false);
        startButton.gameObject.SetActive(false);

        //Deactivate each room button so player can't just skip rooms and break the game
        foreach(GameObject room in roomButtons)
        {
            room.GetComponent<Button>().interactable = false;
            room.GetComponent<JumpyButtons>().enabled = false;
        }

        //Inventory starts empty
        inventoryFilled = new bool[6];
        inventoryFilled[0] = false;
        inventoryFilled[1] = false;
        inventoryFilled[2] = false;
        inventoryFilled[3] = false;
        inventoryFilled[4] = false;
        inventoryFilled[5] = false;

        itemDescWindow.SetActive(false);
    }

    private void Update()
    {
        if (currentState == DungeonState.classSelect)
        {
            //-------------CLASS SELECTION------------------
            if (Input.GetMouseButtonDown(0))
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

                    playerClass = "druid";

                    startButton.gameObject.SetActive(true);

                    SpawnPlayer(playerClasses[0]);
                }
                //Brawler Selected
                else if (EventSystem.current.currentSelectedGameObject == classButtons[1])
                {
                    statWindow.SetActive(true);
                    classText.text = "Brawler Abilities";

                    abilityTexts[0].text = "Nothing";
                    abilityTexts[1].text = "Meditate (Heal yourself for [3] HP)";
                    abilityTexts[2].text = "Block (+[2] DEF)";
                    abilityTexts[3].text = "Counter (+[1] DEF, Deal [1] DMG on hit, lasts 2 turns)";
                    abilityTexts[4].text = "Slap (Deal [2] DMG)";
                    abilityTexts[5].text = "Double Slap (Deal [2] DMG twice)";

                    playerClass = "brawler";

                    startButton.gameObject.SetActive(true);

                    SpawnPlayer(playerClasses[1]);
                }
                else if (EventSystem.current.currentSelectedGameObject == null)
                {
                    playerClasses[0].SetActive(false);
                    playerClasses[1].SetActive(false);

                    statWindow.SetActive(false);
                    startButton.gameObject.SetActive(false);
                }
            }
        }
        //------------MAP TRAVERSAL-------------
        else if(currentState == DungeonState.map)
        {
            //Guaranteed item room
            if (currentFloor == 0)
            {
                if(EventSystem.current.currentSelectedGameObject == roomButtons[0])
                {
                    MoveToRoom(0);
                    currentState = DungeonState.room;
                    currentRoom = RoomType.item;

                    roomButtons[0].GetComponent<Button>().interactable = false;
                    roomButtons[0].GetComponent<JumpyButtons>().enabled = false;
                }
            }
            //Choose between 4 rooms
            else if(currentFloor == 1)
            {
                roomButtons[1].GetComponent<Button>().interactable = true;
                roomButtons[2].GetComponent<Button>().interactable = true;
                roomButtons[3].GetComponent<Button>().interactable = true;
                roomButtons[4].GetComponent<Button>().interactable = true;
                roomButtons[1].GetComponent<JumpyButtons>().enabled = true;
                roomButtons[2].GetComponent<JumpyButtons>().enabled = true;
                roomButtons[3].GetComponent<JumpyButtons>().enabled = true;
                roomButtons[4].GetComponent<JumpyButtons>().enabled = true;

                if (EventSystem.current.currentSelectedGameObject == roomButtons[1])
                {
                    MoveToRoom(1);
                    currentState = DungeonState.room;
                    currentRoom = RoomType.blessing;

                    roomButtons[1].GetComponent<Button>().interactable = false;
                    roomButtons[2].GetComponent<Button>().interactable = false;
                    roomButtons[3].GetComponent<Button>().interactable = false;
                    roomButtons[4].GetComponent<Button>().interactable = false;
                    roomButtons[1].GetComponent<JumpyButtons>().enabled = false;
                    roomButtons[2].GetComponent<JumpyButtons>().enabled = false;
                    roomButtons[3].GetComponent<JumpyButtons>().enabled = false;
                    roomButtons[4].GetComponent<JumpyButtons>().enabled = false;
                }
                if (EventSystem.current.currentSelectedGameObject == roomButtons[2])
                {
                    MoveToRoom(2);
                    currentState = DungeonState.room;
                    currentRoom = RoomType.curse;

                    roomButtons[1].GetComponent<Button>().interactable = false;
                    roomButtons[2].GetComponent<Button>().interactable = false;
                    roomButtons[3].GetComponent<Button>().interactable = false;
                    roomButtons[4].GetComponent<Button>().interactable = false;
                    roomButtons[1].GetComponent<JumpyButtons>().enabled = false;
                    roomButtons[2].GetComponent<JumpyButtons>().enabled = false;
                    roomButtons[3].GetComponent<JumpyButtons>().enabled = false;
                    roomButtons[4].GetComponent<JumpyButtons>().enabled = false;
                }
                if (EventSystem.current.currentSelectedGameObject == roomButtons[3])
                {
                    MoveToRoom(3);
                    currentState = DungeonState.room;
                    currentRoom = RoomType.item;

                    roomButtons[1].GetComponent<Button>().interactable = false;
                    roomButtons[2].GetComponent<Button>().interactable = false;
                    roomButtons[3].GetComponent<Button>().interactable = false;
                    roomButtons[4].GetComponent<Button>().interactable = false;
                    roomButtons[1].GetComponent<JumpyButtons>().enabled = false;
                    roomButtons[2].GetComponent<JumpyButtons>().enabled = false;
                    roomButtons[3].GetComponent<JumpyButtons>().enabled = false;
                    roomButtons[4].GetComponent<JumpyButtons>().enabled = false;
                }
                if (EventSystem.current.currentSelectedGameObject == roomButtons[4])
                {
                    MoveToRoom(4);
                    currentState = DungeonState.room;
                    currentRoom = RoomType.blessing;

                    roomButtons[1].GetComponent<Button>().interactable = false;
                    roomButtons[2].GetComponent<Button>().interactable = false;
                    roomButtons[3].GetComponent<Button>().interactable = false;
                    roomButtons[4].GetComponent<Button>().interactable = false;
                    roomButtons[1].GetComponent<JumpyButtons>().enabled = false;
                    roomButtons[2].GetComponent<JumpyButtons>().enabled = false;
                    roomButtons[3].GetComponent<JumpyButtons>().enabled = false;
                    roomButtons[4].GetComponent<JumpyButtons>().enabled = false;
                }
            }
            //Guaranteed enemy room
            else if(currentFloor == 2)
            {
                if (EventSystem.current.currentSelectedGameObject == roomButtons[5])
                {
                    MoveToRoom(5);
                }
            }
        }
        //-----------ROOMS---------------
        else if(currentState == DungeonState.room)
        {
            //---------ITEM ROOM---------
            if(currentRoom == RoomType.item)
            {
                if (roomStep == 0)
                {
                    //Place Chest
                    SpawnChest(chest);
                    //Open curtain
                    MoveCurtain(true, 2.5f);
                    //Move player again
                    playerPosition.transform.DOMove(playerRoomPosition.transform.position + new Vector3(-Screen.width, 0), 1f).SetDelay(3.5f);

                    inventoryWindow.GetComponent<InventoryController>().Enabled(false);

                    roomTimer = 1f;
                    roomStep = 1;
                }
                else if(roomStep == 1)
                {
                    //Room Logic
                    //Incremented by CloseItemWindow or (something else)
                }
                else if(roomStep == 2)
                {
                    roomTimer -= Time.deltaTime;

                    if(roomTimer <= 0)
                    {
                        //Close curtain
                        MoveCurtain(false, 1f);
                        //Move room back
                        roomWindow.GetComponent<RectTransform>().DOAnchorPosX(1920, 0.1f).SetDelay(2f);
                        //Move player back
                        playerPosition.transform.DOMove(lastRoom.transform.position, 0.1f).SetDelay(2.2f);
                        //Open curtain
                        MoveCurtain(true, 2.3f);
                        //Move Map and Player down a bit
                        mapWindow.GetComponent<RectTransform>().DOAnchorPos(mapWindow.GetComponent<RectTransform>().anchoredPosition + bottomMapPos, 2f).SetEase(Ease.InOutCubic).SetDelay(3f);
                        playerPosition.GetComponent<RectTransform>().DOAnchorPosY(bottomMapPos.y, 2f).SetEase(Ease.InOutCubic).SetDelay(3f);

                        inventoryWindow.GetComponent<InventoryController>().Enabled(true);

                        currentFloor++;
                        currentState = DungeonState.map;
                    }
                }
            }
            //---------BLESSING ROOM------------
            else if(currentRoom == RoomType.blessing)
            {
                if (roomStep == 0)
                {
                    //Place Statue
                    SpawnBlessing(blessingStatue);
                    //Open curtain
                    MoveCurtain(true, 2.5f);
                    //Move Player
                    playerPosition.transform.DOMove(playerRoomPosition.transform.position + new Vector3(-Screen.width, 0), 1f).SetDelay(3.5f);

                    roomTimer = 1f;
                    roomStep = 1;
                }
                else if (roomStep == 1)
                {
                    //Room Logic
                    //Incremented by 
                }
                else if (roomStep == 2)
                {
                    roomTimer -= Time.deltaTime;

                    if (roomTimer <= 0)
                    {
                        //Close curtain
                        MoveCurtain(false, 1f);
                        //Move room back
                        roomWindow.GetComponent<RectTransform>().DOAnchorPosX(1920, 0.1f).SetDelay(2f);
                        //Move player back
                        playerPosition.transform.DOMove(lastRoom.transform.position, 0.1f).SetDelay(2.2f);
                        //Open curtain
                        MoveCurtain(true, 2.3f);
                        //Move Map and Player down a bit
                        mapWindow.GetComponent<RectTransform>().DOAnchorPos(mapWindow.GetComponent<RectTransform>().anchoredPosition + bottomMapPos, 2f).SetEase(Ease.InOutCubic).SetDelay(3f);
                        playerPosition.GetComponent<RectTransform>().DOAnchorPosY(bottomMapPos.y, 2f).SetEase(Ease.InOutCubic).SetDelay(3f);

                        currentFloor++;
                        currentState = DungeonState.map;
                    }
                }
            }
            //---------CURSE ROOM-------------
            else if (currentRoom == RoomType.curse)
            {
                if (roomStep == 0)
                {
                    //Place Statue
                    SpawnCurse(curseStatue);
                    //Open curtain
                    MoveCurtain(true, 2.5f);
                    //Move Player
                    playerPosition.transform.DOMove(playerRoomPosition.transform.position + new Vector3(-Screen.width, 0), 1f).SetDelay(3.5f);

                    roomTimer = 1f;
                    roomStep = 1;
                }
                else if (roomStep == 1)
                {
                    //Room Logic
                    //Incremented by 
                }
                else if (roomStep == 2)
                {
                    roomTimer -= Time.deltaTime;

                    if (roomTimer <= 0)
                    {
                        //Close curtain
                        MoveCurtain(false, 1f);
                        //Move room back
                        roomWindow.GetComponent<RectTransform>().DOAnchorPosX(1920, 0.1f).SetDelay(2f);
                        //Move player back
                        playerPosition.transform.DOMove(lastRoom.transform.position, 0.1f).SetDelay(2.2f);
                        //Open curtain
                        MoveCurtain(true, 2.3f);
                        //Move Map and Player down a bit
                        mapWindow.GetComponent<RectTransform>().DOAnchorPos(mapWindow.GetComponent<RectTransform>().anchoredPosition + bottomMapPos, 2f).SetEase(Ease.InOutCubic).SetDelay(3f);
                        playerPosition.GetComponent<RectTransform>().DOAnchorPosY(bottomMapPos.y, 2f).SetEase(Ease.InOutCubic).SetDelay(3f);

                        currentFloor++;
                        currentState = DungeonState.map;
                    }
                }
            }
            //---------ENEMY ROOM---------------
            else if (currentRoom == RoomType.enemy)
            {

            }
        }
    }


    public void ReturnToMenu()
    {
        StartCoroutine(DelayedLoadScene(0f, "Menu"));
    }

    //Transition from SelectClass to Map
    public void StartRun()
    {
        //Move all windows downwards
        //selectClassWindow.GetComponent<RectTransform>().DOAnchorPosY(selectClassWindow.GetComponent<RectTransform>().anchoredPosition.y - 1080, 2f).SetEase(Ease.InOutCubic);
        mapWindow.GetComponent<RectTransform>().DOAnchorPosY(mapWindow.GetComponent<RectTransform>().anchoredPosition.y - mapWindow.GetComponent<RectTransform>().offsetMin.y, 2f).SetEase(Ease.InOutCubic);
        inventoryWindow.GetComponent<RectTransform>().DOAnchorPosY(-inventoryWindow.GetComponent<RectTransform>().anchoredPosition.y, 2f).SetEase(Ease.InOutCubic);
        mapStatWindow.GetComponent<RectTransform>().DOAnchorPosY(-mapStatWindow.GetComponent<RectTransform>().anchoredPosition.y, 2f).SetEase(Ease.InOutCubic);

        //Move player
        playerPosition.GetComponent<RectTransform>().DOAnchorPos(bottomMapPos, 2f).SetEase(Ease.InOutCubic);

        MovePickRoomText(true, 2f);

        roomButtons[0].GetComponent<Button>().interactable = true;
        roomButtons[0].GetComponent<JumpyButtons>().enabled = true;

        //Setup Player statistics
        player.GetComponent<PlayerController>().SetupClass(playerClass);

        currentState = DungeonState.map;
    }

    IEnumerator DelayedLoadScene(float sec, string scene)
    {
        yield return new WaitForSeconds(sec);
        SceneManager.LoadSceneAsync(scene);
    }

    void SpawnPlayer(GameObject player)
    {
        if (player == playerClasses[0])
        {
            playerClasses[0].SetActive(true);
            playerClasses[1].SetActive(false);
        }
        else
        {
            playerClasses[1].SetActive(true);
            playerClasses[0].SetActive(false);
        }
        this.player = player;
        RectTransform playerRect = this.player.GetComponent<RectTransform>();
        playerRect.anchoredPosition = Vector2.zero;
        playerRect.DOJumpAnchorPos(new Vector2(playerRect.anchoredPosition.x, playerRect.anchoredPosition.y), 10, 1, 0.2f);
    }

    void SpawnChest(GameObject chest)
    {
        GameObject newChest = this.chest;

        newChest = Instantiate(chest, roomObjectPosition);
        newChest.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    void SpawnBlessing(GameObject blessingStatue)
    {
        GameObject newStatue = this.blessingStatue;

        newStatue = Instantiate(blessingStatue, roomObjectPosition);
        newStatue.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, newStatue.GetComponent<RectTransform>().anchoredPosition.y);
    }

    void SpawnCurse(GameObject curseStatue)
    {
        GameObject newStatue = this.curseStatue;

        newStatue = Instantiate(curseStatue, roomObjectPosition);
        newStatue.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, newStatue.GetComponent<RectTransform>().anchoredPosition.y);
    }

    void MovePickRoomText(bool onscreen, float delay)
    {
        if (onscreen)
        {
            pickRoomText.GetComponent<RectTransform>().DOAnchorPosY(-Mathf.Abs(pickRoomText.GetComponent<RectTransform>().anchoredPosition.y), 1f).SetEase(Ease.OutCubic).SetDelay(delay);
        }
        else
        {
            pickRoomText.GetComponent<RectTransform>().DOAnchorPosY(Mathf.Abs(pickRoomText.GetComponent<RectTransform>().anchoredPosition.y), 1f).SetEase(Ease.OutCubic).SetDelay(delay);

        }
    }

    void MoveToRoom(int room)
    {
        //remove any objects if any exist
        if (roomObjectPosition.transform.childCount > 0) Destroy(roomObjectPosition.transform.GetChild(0).gameObject, 1f);
        if (enemyRoomPosition.transform.childCount > 0) Destroy(enemyRoomPosition.transform.GetChild(0).gameObject, 1f);

        roomStep = 0;
        //Record last room visited
        lastRoom = roomButtons[room];
        //Remove pick room text
        MovePickRoomText(false, 0);
        //Move player first
        playerPosition.transform.DOMove(roomButtons[room].transform.position, 1f).SetEase(Ease.OutCubic);
        //Close curtain
        MoveCurtain(false, 1f);
        //Move stage and player
        roomWindow.GetComponent<RectTransform>().DOAnchorPosX(0, 0).SetDelay(2f);
        playerPosition.transform.DOMove(playerRoomPosition.transform.position + new Vector3(-700-Screen.width, 0), 0.1f).SetDelay(2.1f);
        /*
        //Open curtain
        curtain.GetComponent<RectTransform>().DOAnchorPosY(Mathf.Abs(curtain.GetComponent<RectTransform>().anchoredPosition.y), 1f).SetEase(Ease.InCubic).SetDelay(2.5f);
        //Move player again
        playerPosition.transform.DOMove(playerRoomPosition.transform.position + new Vector3(-1920, 0), 1f).SetDelay(3.5f);
        */
    }

    public void MoveItemWindow(float delay, bool chest)
    {
        if (chest)
        {
            itemSlots[0].SetActive(true);
            itemSlots[1].SetActive(true);
            itemSlots[2].SetActive(true);

            //Randomize all items
            int rng1 = Random.Range(0, itemPool.Count);
            ChangeItem(0, itemPool[rng1]);
            int rng2 = Random.Range(0, itemPool.Count);
            while(rng2 == rng1)
            {
                rng2 = Random.Range(0, itemPool.Count);
            }
            ChangeItem(1, itemPool[rng2]);
            int rng3 = Random.Range(0, itemPool.Count);
            while (rng3 == rng1 || rng3 == rng2)
            {
                rng3 = Random.Range(0, itemPool.Count);
            }
            ChangeItem(2, itemPool[rng3]);

        }
        //enemy item drop (only 1 item drop)
        else
        {
            itemSlots[0].SetActive(false);
            itemSlots[1].SetActive(true);
            itemSlots[2].SetActive(false);

            //Randomize only middle item
            int rng = Random.Range(0, itemPool.Count);
            ChangeItem(1, itemPool[rng]);
        }
        itemWindow.GetComponent<RectTransform>().DOAnchorPosY(-Mathf.Abs(itemWindow.GetComponent<RectTransform>().anchoredPosition.y), 1f).SetEase(Ease.OutCubic).SetDelay(delay);

        GameObject[] allItems = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject x in allItems)
        {
            x.GetComponent<Button>().interactable = true;
        }
    }

    public void CloseItemWindow(float delay)
    {
        itemWindow.GetComponent<RectTransform>().DOAnchorPosY(Mathf.Abs(itemWindow.GetComponent<RectTransform>().anchoredPosition.y), 1f).SetEase(Ease.OutCubic).SetDelay(delay);
        roomStep++;
    }

    public void ChangeItem(int itemNum, GameObject item)
    {
        //itemNum = position
        //item = actual item

        GameObject createdItem = Instantiate(item, itemSlots[itemNum].transform);
        createdItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    public void ChangeItemDescription(string itemName, string itemDesc)
    {
        itemNameText.text = itemName;
        itemDescText.text = itemDesc;
    }

    public void ObtainItem(GameObject item)
    {
        for(int i = 0; i < inventoryFilled.Length; i++)
        {
            if(inventoryFilled[i] == false)
            {
                item.transform.DOMove(inventorySlots[i].transform.position, 1f).SetEase(Ease.OutCubic);
                item.transform.SetParent(inventorySlots[i].transform);
                inventoryFilled[i] = true;
                
                for(int j = 0; j < itemPool.Count; j++)
                {
                    if (item.GetComponent<Item>().itemName.Equals(itemPool[j].GetComponent<Item>().itemName))
                    {
                        itemPool.RemoveAt(j);
                        break;
                    }
                }

                break;
            }
        }

        GameObject[] allItems = GameObject.FindGameObjectsWithTag("Item");
        foreach(GameObject x in allItems)
        {
            x.GetComponent<Button>().interactable = false;
        }

        if (itemSlots[0].transform.childCount > 0) Destroy(itemSlots[0].transform.GetChild(0).gameObject, 2f);
        if (itemSlots[1].transform.childCount > 0) Destroy(itemSlots[1].transform.GetChild(0).gameObject, 2f);
        if (itemSlots[2].transform.childCount > 0) Destroy(itemSlots[2].transform.GetChild(0).gameObject, 2f);
    }

    public void MoveCurtain(bool onscreen, float delay)
    {
        if (onscreen)
        {
            curtain.GetComponent<RectTransform>().DOAnchorPosY(Mathf.Abs(curtain.GetComponent<RectTransform>().anchoredPosition.y), 1f).SetEase(Ease.InCubic).SetDelay(delay);
        }
        else
        {
            curtain.GetComponent<RectTransform>().DOAnchorPosY(-curtain.GetComponent<RectTransform>().anchoredPosition.y, 1f).SetEase(Ease.OutCubic).SetDelay(delay);
        }
    }

    public void MoveRouletteWindow(float delay, string type)
    {
        rouletteWindow.GetComponent<RectTransform>().DOAnchorPosY(-540f, 1f).SetEase(Ease.OutCubic).SetDelay(delay);
        FindObjectOfType<RouletteWheel>().ResetSlots();

        if (type.Equals("blessing"))
        {
            FindObjectOfType<RouletteWheel>().SetupBlessings();
        }
        if (type.Equals("curse"))
        {
            FindObjectOfType<RouletteWheel>().SetupCurses();
        }
        if (type.Equals("player"))
        {

        }
        if (type.Equals("enemy"))
        {

        }
    }

    public void SetNextRoomTimer(float time)
    {
        roomStep++;
        roomTimer = time;
        rouletteWindow.GetComponent<RectTransform>().DOAnchorPosY(540f, 1f).SetEase(Ease.InOutCubic).SetDelay(1f);
    }
}
