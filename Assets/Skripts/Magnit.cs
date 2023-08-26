using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnit : MonoBehaviour
{
    public static Vector3 playerPosition { get; set; }
    public static bool MagnitOn { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin" && MagnitOn)
        {
            StartCoroutine(CoinMagnit(other.gameObject));
        }
    }

    IEnumerator CoinMagnit(GameObject coin)
    {
        float distante = Vector3.Distance(playerPosition, coin.transform.position);
        float zHelp = 0;
        while (distante > 0.01)
        {
            try
            {
                if (playerPosition.z > coin.transform.position.z)
                    zHelp = 1f;
                else
                    zHelp = 0;
                coin.transform.Translate((playerPosition - coin.transform.position) / Mathf.Sqrt(distante) + new Vector3(0, 0, zHelp), Space.World);
            }
            catch (Exception) { distante = 0; }
            yield return new WaitForSeconds(0.001f);

        }
    }
}
