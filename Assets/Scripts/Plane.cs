using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    [SerializeField] GameObject Vint;
    Vector3 VitnRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vint.transform.RotateAround(Vint.transform.position,new Vector3(0,0,5), 10f);
        this.transform.Translate(new Vector3(-0.2f, 0, 0));

    }

    private void FixedUpdate()
    {
    }
}
