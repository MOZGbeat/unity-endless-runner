using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlller : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] public Animator anim;
    private Vector3 offset;
    Quaternion defaultRotation;
    float playerY;

    void Start()
    {
        offset = transform.position - player.position;
        anim = GetComponentInChildren<Animator>();
        defaultRotation = transform.rotation;
        playerY = player.position.y;
    }

    void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(player.position.y, defaultRotation.y, defaultRotation.z);
        transform.position = new Vector3(player.position.x * 0.5f, player.position.y * 0.5f, offset.z + player.position.z);

        if (player.position.y > 10)
        {
            transform.position = new Vector3(player.position.x * 0.5f, player.position.y * 0.5f + 0.5f * (player.position.y - 10), offset.z + player.position.z);
        }
    }

    public void StartCamAnim()
    {
        anim.SetTrigger("camStartAnim");
    }
    public void ExitCamAnim()
    {
        anim.SetTrigger("camExit");
    }
}
