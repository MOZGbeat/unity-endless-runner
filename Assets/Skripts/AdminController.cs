using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminController : MonoBehaviour
{
    [SerializeField] private GameObject adminPanel;
    [SerializeField] private GameObject authBlock;
    [SerializeField] private InputField passwordText;
    [SerializeField] private GameObject adminMenu;
    [SerializeField] private Text coins;
    [SerializeField] private Text score;

    string passAdmin = "12345";
    bool auth = false;

    void Start()
    {
        
    }

    private void Update()
    {
        CheckClickClose();

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
            CheckAuthPass();
    }

    public void AuthorizationShow()
    {
        adminPanel.SetActive(true);
        if (!auth)
            authBlock.SetActive(true);
        else
            adminMenu.SetActive(true);
        passwordText.Select();
        passwordText.ActivateInputField();
    }

    public void CheckAuthPass()
    {
        if (passwordText.text == passAdmin)
        {
            authBlock.SetActive(false);
            adminMenu.SetActive(true);
        }
        else
        {
            adminPanel.SetActive(false);
            Debug.Log("Неверный пароль");
        }
    }

    public void UpMoney()
    {
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + 1000);
        coins.text = PlayerPrefs.GetInt("coins").ToString();
    }

    public void NullMoney()
    {
        PlayerPrefs.SetInt("coins", 0);
        coins.text = "0";
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("Skin0", 1);
        PlayerPrefs.SetInt("Map0", 1);
        PlayerPrefs.SetInt("RecordCount", 0);

        coins.text = "0";
        score.text = "0";
    }

    private void CheckClickClose()
    {
        if (Input.GetMouseButtonDown(0) && authBlock.activeSelf && !RectTransformUtility.RectangleContainsScreenPoint(authBlock.GetComponent<RectTransform>(), Input.mousePosition, Camera.main))
        {
            this.gameObject.SetActive(false);
        }
        if (Input.GetMouseButtonDown(0) && adminMenu.activeSelf && !RectTransformUtility.RectangleContainsScreenPoint(adminMenu.GetComponent<RectTransform>(), Input.mousePosition, Camera.main))
        {
            this.gameObject.SetActive(false);
        }
    }
}
