using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class SecondChancePanel : MonoBehaviour
{
    [SerializeField] GameObject secondChancePanel;
    [SerializeField] GameObject CoinText;
    public static Vector3 playerPosition { get; set; }


    public void ResumeGame(GameObject price)
    {
        if (PlayerPrefs.GetInt("coins") < Convert.ToInt32(price.GetComponent<Text>().text))
        {
            EndRun();
        }
        else
        {
            int priceValue = Convert.ToInt32(price.GetComponent<Text>().text);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - priceValue);
            CoinText.GetComponent<Text>().text = PlayerPrefs.GetInt("coins").ToString();

            secondChancePanel.SetActive(false);
            price.GetComponent<Text>().text = (priceValue * 2).ToString();

            GameObject[] obstacleItems = GameObject.FindGameObjectsWithTag("obstacle");
            foreach (GameObject item in obstacleItems)
            {
                if (Vector3.Distance(playerPosition, item.transform.position) < 70)
                    Destroy(item);
            }
            Time.timeScale = 1;
        }
    }
    
    public void EndRun()
    {
        Time.timeScale = 1;
        PlayerController.endRunTrigger = true;
    }
}
