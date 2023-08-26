using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonused : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (tag == "Coin")
            transform.Rotate(0, 0, 40 * Time.deltaTime);
        if (tag == "bon_DJump")
            transform.Rotate(0, 2, 0 * Time.deltaTime);
        if (tag == "bon_ScoreX2")
            transform.Rotate(0, 2, 0 * Time.deltaTime);
        if (tag == "bon_CoinX2")
            transform.Rotate(2, 0, 0 * Time.deltaTime);
        if (tag == "bon_Magnit")
            transform.Rotate(2, 0, 0 * Time.deltaTime);
    }
}
