using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Text coinsText;
    [SerializeField] GameObject helpBlock;
    [SerializeField] GameObject helpUnder;
    [SerializeField] GameObject InputName_text;
    [SerializeField] GameObject NameEnter_panel;
    [SerializeField] GameObject NameEnter_block;
    [SerializeField] GameObject ChangeNameBtnText;
    [SerializeField] GameObject RecordPanel;
    [SerializeField] GameObject RecordBlock;

    public static string PlayerName { get; set; }
    string[] nameRecordList = new string[10];
    int[] scoreRecordList = new int[10];


    private void Start()
    {
        int coins = PlayerPrefs.GetInt("coins");
        coinsText.text = coins.ToString();
        int score = PlayerPrefs.GetInt("lastRunScore");
        Time.timeScale = 1;
    }

    private void Update()
    {
        CheckClickClose();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void StopGame()
    {
        SceneManager.LoadScene(0);
    }

    public void HelpPanel()
    {
        helpUnder.SetActive(true);
    }

    private void CheckClickClose()
    {
        try
        {
            if (Input.GetMouseButtonDown(0) && helpBlock.activeSelf && !RectTransformUtility.RectangleContainsScreenPoint(helpBlock.GetComponent<RectTransform>(), Input.mousePosition, Camera.main))
                helpUnder.SetActive(false);
        }
        catch { }
        try
        {
            if (Input.GetMouseButtonDown(0) && RecordBlock.activeSelf && !RectTransformUtility.RectangleContainsScreenPoint(RecordBlock.GetComponent<RectTransform>(), Input.mousePosition, Camera.main))
                RecordPanel.SetActive(false);
        }
        catch { }
        try
        {
            if (Input.GetMouseButtonDown(0) && NameEnter_block.activeSelf && !RectTransformUtility.RectangleContainsScreenPoint(NameEnter_block.GetComponent<RectTransform>(), Input.mousePosition, Camera.main))
                NameEnter_panel.SetActive(false);
        }
        catch { }
    }

    public void ChangeNamePanel()
    {
        ActiveNameEnterPanel();
    }

    public void ActiveNameEnterPanel()
    {
        LosePanel.blockedOn = false;
        NameEnter_panel.SetActive(true);
        InputName_text.GetComponent<InputField>().Select();
        InputName_text.GetComponent<InputField>().ActivateInputField();
        Time.timeScale = 0;
    }
    public void SaveName()
    {
        LosePanel.blockedOn = true;
        PlayerName = InputName_text.GetComponent<InputField>().text;
        PlayerPrefs.SetString("PlayerName", PlayerName);
        ChangeNameBtnText.GetComponent<Text>().text = PlayerName;
        NameEnter_panel.SetActive(false);
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt("RecordCount", PlayerPrefs.GetInt("RecordCount") + 1);
        PlayerPrefs.SetInt("RecordScore_" + PlayerPrefs.GetInt("RecordCount").ToString(), PlayerPrefs.GetInt("lastRunScore"));
        PlayerPrefs.SetString("RecordName_" + PlayerPrefs.GetInt("RecordCount").ToString(), ChangeNameBtnText.GetComponent<Text>().text);
        StopGame();
        RecordList();
    }

    public void RecordList()
    {
        int recordCount = PlayerPrefs.GetInt("RecordCount");

        for (int i = recordCount - 1; i >= 1; i--)
        {
            for (int k = recordCount; k >= i + 1; k--)
            {
                if (PlayerPrefs.GetInt("RecordScore_" + i.ToString()) < PlayerPrefs.GetInt("RecordScore_" + k.ToString()))
                {
                    int tmpScore = PlayerPrefs.GetInt("RecordScore_" + i.ToString());
                    PlayerPrefs.SetInt("RecordScore_" + i.ToString(), PlayerPrefs.GetInt("RecordScore_" + k.ToString()));
                    PlayerPrefs.SetInt("RecordScore_" + k.ToString(), tmpScore);
                    string tmpName = PlayerPrefs.GetString("RecordName_" + i.ToString());
                    PlayerPrefs.SetString("RecordName_" + i.ToString(), PlayerPrefs.GetString("RecordName_" + k.ToString()));
                    PlayerPrefs.SetString("RecordName_" + k.ToString(), tmpName);
                }
            }
        }

        for (int i = 1; i >= recordCount - 1; i--)
        {
            nameRecordList[recordCount - i] = PlayerPrefs.GetString("RecordName_" + i.ToString());
            scoreRecordList[recordCount - i] = PlayerPrefs.GetInt("RecordScore_" + i.ToString());
        }

    }

    public void ShowRecordPanel()
    {
        GameObject[] nameListElement = new GameObject[10];
        GameObject[] scoreListElement = new GameObject[10];
        int nameCounter = 0;
        int scoreCounter = 0;

        RecordPanel.SetActive(true);
        for (int i = 0; i < RecordBlock.GetComponentsInChildren<Text>().Length; i++)
        {
            if (RecordBlock.GetComponentsInChildren<Text>()[i].gameObject.tag == "RecordName_item")
            {
                nameListElement[nameCounter] = RecordBlock.GetComponentsInChildren<Text>()[i].gameObject;
                nameCounter++;
            }
            if (RecordBlock.GetComponentsInChildren<Text>()[i].gameObject.tag == "RecordScore_item")
            {
                scoreListElement[scoreCounter] = RecordBlock.GetComponentsInChildren<Text>()[i].gameObject;
                scoreCounter++;
            }
        }
        for (int i = 0; i < nameListElement.Length; i++)
        {
            nameListElement[i].GetComponent<Text>().text = PlayerPrefs.GetString("RecordName_" + (i + 1).ToString());
            if (PlayerPrefs.GetInt("RecordScore_" + (i + 1).ToString()) > 0)
                scoreListElement[i].GetComponent<Text>().text = PlayerPrefs.GetInt("RecordScore_" + (i + 1).ToString()).ToString();
            else
                scoreListElement[i].GetComponent<Text>().text = "";
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
