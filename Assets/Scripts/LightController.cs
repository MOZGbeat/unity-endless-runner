using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    private float rotateX;
    private float rotateY;
    private float rotateZ;
    private float changeRot = 0;

    void Start()
    {
        rotateX = transform.rotation.eulerAngles.x;
        rotateY = transform.rotation.eulerAngles.y;
        rotateZ = transform.rotation.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.transform.position.z == 0)
            changeRot  = 0.02f;
        else
            changeRot = Mathf.Sqrt(Mathf.Sqrt(Player.transform.position.z)) / 1000f;
        rotateY += changeRot;
        transform.rotation = Quaternion.Euler(rotateX, rotateY, rotateZ);
    }
}
