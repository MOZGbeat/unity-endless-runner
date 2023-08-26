using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopController : MonoBehaviour
{
    [SerializeField] private GameObject shopMenu;
    [SerializeField] private GameObject shopMenuPanel;
    [SerializeField] private GameObject skin1;
    [SerializeField] private GameObject skin2;
    [SerializeField] private GameObject skin3;
    [SerializeField] private GameObject skin4;
    [SerializeField] private GameObject skin5;
    [SerializeField] private GameObject skin6;
    [SerializeField] private GameObject skin7;
    [SerializeField] private GameObject map1;
    [SerializeField] private GameObject map2;
    [SerializeField] private GameObject buySkin;
    [SerializeField] private GameObject buyMap;
    [SerializeField] private GameObject costSkin;
    [SerializeField] private GameObject costMap;
    [SerializeField] private GameObject coins;
    [SerializeField] private GameObject shopBlockSkins;
    [SerializeField] private GameObject shopBlockMap;

    GameObject[] skins = new GameObject[7];
    GameObject[] maps = new GameObject[2];
    int countSkins;
    int countMaps;
    int activeSkin = 0;
    int activeMap = 0;
    int usedSkin = 0;
    int usedMap = 0;

    int[] skin_prices;
    int[] map_prices;

    void Start()
    {
        skin_prices = new int[7] { 0, 500, 1800, 2400, 6000, 10000, 15000 };
        map_prices = new int[2] { 0, 10000 };
        skins = new GameObject[7] { skin1, skin2, skin3, skin4, skin5, skin6, skin7 };
        maps = new GameObject[2] { map1, map2 };
        PlayerPrefs.SetInt("Skin0", 1);
        PlayerPrefs.SetInt("Map0", 1);
        countSkins = skins.Length;
        countMaps= maps.Length;

        skins[activeSkin].SetActive(false);
        maps[activeMap].SetActive(false);
        activeSkin = PlayerPrefs.GetInt("usedSkin");
        activeMap = PlayerPrefs.GetInt("usedMap");
        skins[activeSkin].SetActive(true);
        maps[activeMap].SetActive(true);
        CheckBuySkin();
        CheckBuyMap();
    }

    private void Update()
    {
        ShowShopMenu();
    }
    public void ShowShop()
    {
        shopMenu.SetActive(true);
        ShowShopSkins();
    }

    public void ShowShopMenu()
    {
        CheckBuySkin();
        CheckBuyMap(); 
        if (Input.GetMouseButtonDown(0) && shopMenuPanel.activeSelf && !RectTransformUtility.RectangleContainsScreenPoint(shopMenuPanel.GetComponent<RectTransform>(), Input.mousePosition, Camera.main))
        {
            shopMenu.SetActive(false);
        }
    }

    public void NextSkin()
    {
        for (int i = 0; i < countSkins; i++)
        {
            skins[i].SetActive(false);
        }
        if (activeSkin + 1 >= countSkins)
            activeSkin = 0;
        else
            activeSkin++;
        skins[activeSkin].SetActive(true);
        CheckBuySkin();
    }
    public void PrevSkin()
    {
        for (int i = 0; i < countSkins; i++)
        {
            skins[i].SetActive(false);
        }
        if (activeSkin - 1 < 0)
            activeSkin = countSkins - 1;
        else
            activeSkin--;
        skins[activeSkin].SetActive(true);
        CheckBuySkin();
    }

    public void NextMap()
    {
        for (int i = 0; i < countMaps; i++)
        {
            maps[i].SetActive(false);
        }
        if (activeMap + 1 >= countMaps)
            activeMap = 0;
        else
            activeMap++;
        maps[activeMap].SetActive(true);
        CheckBuyMap();
    }
    public void PrevMap()
    {
        for (int i = 0; i < countMaps; i++)
        {
            maps[i].SetActive(false);
        }
        if (activeMap - 1 < 0)
            activeMap = countMaps - 1;
        else
            activeMap--;
        maps[activeMap].SetActive(true);
        CheckBuyMap();
        
    }

    private void CheckBuySkin()
    {
        if (PlayerPrefs.GetInt("Skin" + activeSkin.ToString()) == 1)
        {
            if (PlayerPrefs.GetInt("usedSkin") == activeSkin)
            {
                buySkin.GetComponent<Button>().interactable = false;
                buySkin.GetComponent<Graphic>().color = Color.green;
                buySkin.GetComponentInChildren<Text>().text = "Used";
                costSkin.GetComponent<Text>().text = "";
            }
            else
            {
                buySkin.GetComponent<Button>().interactable = true;
                buySkin.GetComponent<Graphic>().color = Color.green;
                buySkin.GetComponentInChildren<Text>().text = "Use";
                costSkin.GetComponent<Text>().text = "";
            }
        }
        else
        {
            buySkin.GetComponent<Button>().interactable = true;
            buySkin.GetComponent<Graphic>().color = Color.yellow;
            buySkin.GetComponentInChildren<Text>().text = "Buy";
            costSkin.GetComponent<Text>().text = skin_prices[activeSkin].ToString();
        }
    }

    private void CheckBuyMap()
    {
        if (PlayerPrefs.GetInt("Map" + activeMap.ToString()) == 1)
        {
            if (PlayerPrefs.GetInt("usedMap") == activeMap)
            {
                buyMap.GetComponent<Button>().interactable = false;
                buyMap.GetComponent<Graphic>().color = Color.green;
                buyMap.GetComponentInChildren<Text>().text = "Used";
                costMap.GetComponent<Text>().text = "";
            }
            else
            {
                buyMap.GetComponent<Button>().interactable = true;
                buyMap.GetComponent<Graphic>().color = Color.green;
                buyMap.GetComponentInChildren<Text>().text = "Use";
                costMap.GetComponent<Text>().text = "";
            }
        }
        else
        {
            buyMap.GetComponent<Button>().interactable = true;
            buyMap.GetComponent<Graphic>().color = Color.yellow;
            buyMap.GetComponentInChildren<Text>().text = "Buy";
            costMap.GetComponent<Text>().text = map_prices[activeMap].ToString();
        }
    }


    public void Buy_Use_Skin()
    {
        if (buySkin.GetComponentInChildren<Text>().text == "Buy")
        {
            if (PlayerPrefs.GetInt("Skin" + activeSkin.ToString()) == 0)
                if (PlayerPrefs.GetInt("coins") >= skin_prices[activeSkin])
                {
                    PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - skin_prices[activeSkin]);
                    coins.GetComponent<Text>().text = PlayerPrefs.GetInt("coins".ToString()).ToString();
                    PlayerPrefs.SetInt("Skin" + activeSkin.ToString(), 1);
                    CheckBuySkin();
                }
                else
                {
                    Debug.Log("Недостаточно монет!");
                }
        }
        else if (buySkin.GetComponentInChildren<Text>().text == "Use")
        {
            usedSkin = activeSkin;
            PlayerPrefs.SetInt("usedSkin", usedSkin);
            CheckBuySkin();
        }
    }
    public void Buy_Use_Map()
    {
        if (buyMap.GetComponentInChildren<Text>().text == "Buy")
        {
            if (PlayerPrefs.GetInt("Map" + activeMap.ToString()) == 0)
                if (PlayerPrefs.GetInt("coins") >= map_prices[activeMap])
                {
                    PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - map_prices[activeMap]);
                    coins.GetComponent<Text>().text = PlayerPrefs.GetInt("coins".ToString()).ToString();
                    PlayerPrefs.SetInt("Map" + activeMap.ToString(), 1);
                    CheckBuyMap();
                }
                else
                {
                    Debug.Log("Недостаточно монет!");
                }
        }
        else if (buyMap.GetComponentInChildren<Text>().text == "Use")
        {
            usedMap = activeMap;
            PlayerPrefs.SetInt("usedMap", usedMap);
            CheckBuyMap();
        }
    }

    public void ShowShopSkins()
    {
        shopBlockSkins.SetActive(true);
        shopBlockMap.SetActive(false);
    }

    public void ShowShopMap()
    {
        shopBlockSkins.SetActive(false);
        shopBlockMap.SetActive(true);
    }
}
