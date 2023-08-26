using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusGenerator : MonoBehaviour
{
    public GameObject[] bonuses;

    // Start is called before the first frame update
    void Start()
    {
        GameObject nextTile;
        if (Random.Range(1, 5) == 1)
        {
            nextTile = Instantiate(bonuses[Random.Range(0, bonuses.Length)], this.transform.position, this.transform.rotation);
            if (nextTile.tag == "bon_CoinX2") 
                nextTile.transform.Rotate(new Vector3(0, 0, 90));
        }
    }
}
