using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] public Text scoreText;

    float score = 0;
    float scoreAdd = 0;
    float Z = 0;
    float pastZ = 0;
    public static ushort scoreCoef  {get; set;}

    private void Start()
    {
        scoreCoef = 1;
    }

    private void Update()
    {
        Z = player.position.z;
        scoreAdd = ((Z - pastZ) / 2 * scoreCoef);
        score += scoreAdd;
        scoreText.text = ((int)score).ToString();
        pastZ = Z;
    }
}
