using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LosePanel : MonoBehaviour
{
    [SerializeField] Text recordText;

    public static bool blockedOn { get; set; }

    private void Start()
    {
        blockedOn = true;
        int lastRunScore = PlayerPrefs.GetInt("lastRunScore");
        int recordScore = PlayerPrefs.GetInt("recordScore");

        if (lastRunScore > recordScore)
        {
            recordScore = lastRunScore;
            PlayerPrefs.SetInt("recordScore", recordScore);
        }
        recordText.text = recordScore.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && blockedOn)
            RestartLevel();
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(1);
    }
}
