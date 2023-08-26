using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    AudioSource audioSource;
    Vector3 dir;
    public List<AudioClip> audioClips;
    [SerializeField] float jumpForce;
    [SerializeField] float gravity;
    [SerializeField] int coins;
    [SerializeField] GameObject cameraObj;
    [SerializeField] GameObject losePanel;
    [SerializeField] GameObject secondChancePanel;
    [SerializeField] GameObject effectCoin;
    [SerializeField] GameObject effect_Bon_DJump;
    [SerializeField] GameObject effect_Bon_ScoreX2;
    [SerializeField] GameObject effect_Bon_CoinX2;
    [SerializeField] GameObject effect_Bon_Magnit;
    [SerializeField] GameObject effect_Bon_Immortal;
    [SerializeField] Text coinsText;
    [SerializeField] Score scoreScript;
    [SerializeField] GameObject skin1;
    [SerializeField] GameObject skin2;
    [SerializeField] GameObject skin3;
    [SerializeField] GameObject skin4;
    [SerializeField] GameObject skin5;
    [SerializeField] GameObject skin6;
    [SerializeField] GameObject Effect;
    [SerializeField] GameObject BonPanel_DJump;
    [SerializeField] GameObject BonPanel_ScoreX2;
    [SerializeField] GameObject BonPanel_CoinX2;
    [SerializeField] GameObject BonPanel_Magnit;
    [SerializeField] GameObject BonPanel_Immortal;
    [SerializeField] GameObject PlayerName;
    [SerializeField] GameObject EndScore;

    bool startRun;
    bool bonuse_DoubleJump;
    bool bonuse_ScoreX2;
    bool bonuse_CoinX2;
    bool bonuse_Magnit;
    bool bonuse_Immortal;
    bool jump;
    bool doubleJump;
    bool down;
    public static bool endRunTrigger { get; set; }
    int bonuse_DoubleJumpSec = 0;
    int bonuse_ScoreX2Sec = 0;
    int bonuse_CoinX2Sec = 0;
    int bonuse_MagnitSec = 0;
    int bonuse_ImmortalSec = 0;
    int lineToMove = 1;
    float speed;
    public float lineDistance = 4;
    const float maxSpeed = 90;
    Animator anim;
    CameraControlller trigCamStartAnim;
    GameObject Bon_Panel;
    GameObject Bon_Immortal_Panel;


    void Start()
    {
        if (PlayerPrefs.GetInt("usedSkin") != 0)
        {
            if (PlayerPrefs.GetInt("usedSkin") == 1)
                this.GetComponentInChildren<SkinnedMeshRenderer>().materials = skin1.GetComponentInChildren<SkinnedMeshRenderer>().materials;
            if (PlayerPrefs.GetInt("usedSkin") == 2)
                this.GetComponentInChildren<SkinnedMeshRenderer>().materials = skin2.GetComponentInChildren<SkinnedMeshRenderer>().materials;
            if (PlayerPrefs.GetInt("usedSkin") == 3)
                this.GetComponentInChildren<SkinnedMeshRenderer>().materials = skin3.GetComponentInChildren<SkinnedMeshRenderer>().materials;
            if (PlayerPrefs.GetInt("usedSkin") == 4)
                this.GetComponentInChildren<SkinnedMeshRenderer>().materials = skin4.GetComponentInChildren<SkinnedMeshRenderer>().materials;
            if (PlayerPrefs.GetInt("usedSkin") == 5)
                this.GetComponentInChildren<SkinnedMeshRenderer>().materials = skin5.GetComponentInChildren<SkinnedMeshRenderer>().materials;
            if (PlayerPrefs.GetInt("usedSkin") == 6)
            {
                this.GetComponentInChildren<SkinnedMeshRenderer>().materials = skin6.GetComponentInChildren<SkinnedMeshRenderer>().materials;
                Effect.SetActive(true);
            }
            else
                Effect.SetActive(false);
        }

        speed = 0;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
        trigCamStartAnim = cameraObj.GetComponent<CameraControlller>();
        //Time.timeScale = 1;
        coins = PlayerPrefs.GetInt("coins");
        coinsText.text = coins.ToString();

        Bon_Panel = BonPanel_DJump.GetComponentsInParent<RectTransform>()[0].gameObject;
        Bon_Panel.SetActive(false);
        BonPanel_Immortal.SetActive(false);
    }

    void Update()
    {
        coins = PlayerPrefs.GetInt("coins");
        //Enter TAP
        if (SwipeController.tap || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
            StartCoroutine(StartRun());
        
        if (!startRun)
            return;
        //Swipe RIGHT
        if (SwipeController.swipeRight || Input.GetKeyDown(KeyCode.RightArrow))
            if (lineToMove < 2)
            {
                lineToMove++;
                SoundPlay("goSide", 0.4f);
            }

        //Swipe LEFT
        if (SwipeController.swipeLeft || Input.GetKeyDown(KeyCode.LeftArrow))
            if (lineToMove > 0)
            {
                lineToMove--;
                SoundPlay("goSide", 0.4f);
            }

        //Swipe UP
        if (SwipeController.swipeUp || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!doubleJump && jump && bonuse_DoubleJump)
            {
                StopCoroutine(DoubleJump());
                StartCoroutine(DoubleJump());
                return;
            }
            if (!jump)
            {
                StartCoroutine(Jump());
            }
        }
        if (!jump && !doubleJump)
        {
            dir.y = -20;
        }

        //Swipe DOWN
        if (SwipeController.swipeDown || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!down)
                StartCoroutine(Down());
        }
        //


        if (controller.isGrounded && !down && !jump)
            anim.SetBool("run", true);
        else
            anim.SetBool("run", false);

        //–‡ÒÒ˜∏Ú ÔÓÁËˆËË
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (lineToMove == 0)
            targetPosition += Vector3.left * lineDistance;
        else if (lineToMove == 2)
            targetPosition += Vector3.right * lineDistance;

        if (transform.position != targetPosition)
        {
            Vector3 diff = targetPosition - transform.position;
            Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
            if (moveDir.sqrMagnitude < diff.sqrMagnitude)
                controller.Move(moveDir);
            else
            {
                controller.Move(diff);
            }
        }
    }


    void FixedUpdate()
    {
        if (endRunTrigger)
        {
            endRunTrigger = false;
            EndRun();
        }
        if (!startRun)
            return;
        dir.z = speed;
        dir.y += gravity * Time.fixedDeltaTime;
        controller.Move(dir * Time.fixedDeltaTime);

        // œÂÂ‰‡˜‡ ÔÓÁËˆËË Ë„ÓÍ‡ ‚ ÒÍËÔÚ Magnit
        if (bonuse_Magnit)
            Magnit.playerPosition = transform.position;
    }
    

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "obstacle")
        {
            if (bonuse_Immortal)
            {
                Destroy(hit.gameObject);
                return;
            }

            SecondChancePanel.playerPosition = transform.position;

            Time.timeScale = 0;
            secondChancePanel.SetActive(true);
        }
    }

    public void EndRun()
    {
        SoundPlay("maleLose", 0.8f);
        trigCamStartAnim.ExitCamAnim();
        startRun = false;
        StopAllCoroutines();
        GetComponent<Animator>().enabled = false;
        controller.enabled = false;

        secondChancePanel.SetActive(false);
        losePanel.SetActive(true);
        int lastRunScore = int.Parse(scoreScript.scoreText.text);
        PlayerPrefs.SetInt("lastRunScore", lastRunScore);
        PlayerName.GetComponent<Text>().text = PlayerPrefs.GetString("PlayerName");
        EndScore.GetComponent<Text>().text = lastRunScore.ToString();

        bonuse_CoinX2 = false;
        bonuse_ScoreX2 = false;
        bonuse_Magnit = false;
        Magnit.MagnitOn = false;
        bonuse_DoubleJump = false;
        bonuse_Immortal = false;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin")
        {
            coins = coins + 1 + Convert.ToInt32(bonuse_CoinX2);
            PlayerPrefs.SetInt("coins", coins);
            coinsText.text = coins.ToString();
            GameObject ghostCoin = new GameObject();
            ghostCoin.transform.position = other.gameObject.transform.position;
            Instantiate(effectCoin, ghostCoin.transform);
            Destroy(other.gameObject);
            Destroy(ghostCoin, 1f);
        }
        if (other.gameObject.tag == "bon_DJump")
        {
            GameObject ghost_bon_DJump = new GameObject();
            ghost_bon_DJump.transform.position = other.gameObject.transform.position;
            Instantiate(effect_Bon_DJump, ghost_bon_DJump.transform);
            Destroy(other.gameObject);
            Destroy(ghost_bon_DJump, 1.5f);

            bonuse_DoubleJumpSec = 15;
            if (!bonuse_DoubleJump)
                StartCoroutine(Bon_DJump());
        }
        if (other.gameObject.tag == "bon_ScoreX2")
        {
            GameObject ghost_bon_ScoreX2 = new GameObject();
            ghost_bon_ScoreX2.transform.position = other.gameObject.transform.position;
            Instantiate(effect_Bon_ScoreX2, ghost_bon_ScoreX2.transform);
            Destroy(other.gameObject);
            Destroy(ghost_bon_ScoreX2, 1.5f);

            bonuse_ScoreX2Sec = 15;
            if (!bonuse_ScoreX2)
                StartCoroutine(Bon_ScoreX2());
        }
        if (other.gameObject.tag == "bon_CoinX2")
        {
            GameObject ghost_bon_CoinX2 = new GameObject();
            ghost_bon_CoinX2.transform.position = other.gameObject.transform.position;
            Instantiate(effect_Bon_CoinX2, ghost_bon_CoinX2.transform);
            Destroy(other.gameObject);
            Destroy(ghost_bon_CoinX2, 1.5f);

            bonuse_CoinX2Sec = 15;
            if (!bonuse_CoinX2)
                StartCoroutine(Bon_CoinX2());
        }
        if (other.gameObject.tag == "bon_Magnit")
        {
            GameObject ghost_bon_Magnit = new GameObject();
            ghost_bon_Magnit.transform.position = other.gameObject.transform.position;
            Instantiate(effect_Bon_Magnit, ghost_bon_Magnit.transform);
            Destroy(other.gameObject);
            Destroy(ghost_bon_Magnit, 1.5f);

            bonuse_MagnitSec = 15;
            if (!bonuse_Magnit)
                StartCoroutine(Bon_Magnit());
        }
        if (other.gameObject.tag == "bon_Immortal")
        {
            GameObject ghost_bon_Immortal = new GameObject();
            ghost_bon_Immortal.transform.position = other.gameObject.transform.position;
            Instantiate(effect_Bon_Immortal, ghost_bon_Immortal.transform);
            Destroy(other.gameObject);
            Destroy(ghost_bon_Immortal, 1.5f);

            bonuse_ImmortalSec = 15;
            if (!bonuse_Immortal)
                StartCoroutine(Bon_Immortal());
        }
    }
    
    

    IEnumerator SpeedIncrease()
    {
        yield return new WaitForSeconds((speed + 1) * (speed + 1) * (speed + 1) / 15000);

        if (speed < maxSpeed)
        {
            speed += 3 * (float)Math.Sqrt(transform.position.z + 1) / (transform.position.z + 1);
            StartCoroutine(SpeedIncrease());
        }
    }

    IEnumerator Jump()
    {
        SoundPlay("jump", 1f);
        jump = true;
        anim.SetTrigger("jump");

        dir.y = jumpForce;
        yield return new WaitForSeconds(1.0f);

        if (!doubleJump)
            jump = false;
    }
    IEnumerator DoubleJump()
    {
        SoundPlay("jump", 1f);
        doubleJump = true;
        jump = true;
        anim.SetTrigger("doubleJump");

        dir.y = jumpForce * 1.3f;
        yield return new WaitForSeconds(1.3f);

        jump = false;
        doubleJump = false;
    }

    IEnumerator Down()
    {
        SoundPlay("down", 0.4f);
        down = true;
        anim.SetTrigger("down");
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.04f);
            controller.center -= new Vector3(0, 0.178f, -0.18f);
            controller.height -= 0.345f;
            controller.radius -= 0.036f;
        }

        yield return new WaitForSeconds(0.75f);

        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.03f);
            controller.center += new Vector3(0, 0.178f, -0.18f);
            controller.height += 0.345f;
            controller.radius += 0.036f;
        }

        down = false;
    }

    IEnumerator StartRun()
    {
        anim.SetTrigger("jumpOff");
        trigCamStartAnim.StartCamAnim();

        yield return new WaitForSeconds(1.5f);

        startRun = true;
        StartCoroutine(SpeedIncrease());
    }

    IEnumerator Bon_DJump()
    {
        Bon_Panel.SetActive(true);
        BonPanel_DJump.GetComponentInChildren<VisualEffect>().enabled = true;
        BonPanel_DJump.gameObject.SetActive(true);

        bonuse_DoubleJump = true;
        BonPanel_DJump.SetActive(true);
        while (bonuse_DoubleJumpSec > 0)
        {
            BonPanel_DJump.gameObject.GetComponentInChildren<Text>().text = bonuse_DoubleJumpSec.ToString();
            yield return new WaitForSeconds(1f);
            bonuse_DoubleJumpSec--;
        }

        BonPanel_DJump.gameObject.GetComponentInChildren<Text>().text = "0";
        bonuse_DoubleJump = false;
        BonPanel_DJump.GetComponentInChildren<VisualEffect>().enabled = false;
        BonPanel_DJump.gameObject.SetActive(false);

        //// —œ»—Œ  ¬—≈’ ¡ŒÕ”—Œ¬. ≈—À» »’ Õ≈“, œ¿Õ≈À‹ «¿– €¬¿≈“—ﬂ
        if (!bonuse_DoubleJump && !bonuse_ScoreX2 && !bonuse_CoinX2 && !bonuse_Magnit)
            Bon_Panel.SetActive(false);
    }
    IEnumerator Bon_ScoreX2()
    {
        Score.scoreCoef = 2;
        Bon_Panel.SetActive(true);

        BonPanel_ScoreX2.GetComponentInChildren<VisualEffect>().enabled = true;
        BonPanel_ScoreX2.gameObject.SetActive(true);

        bonuse_ScoreX2 = true;
        BonPanel_ScoreX2.SetActive(true);
        while (bonuse_ScoreX2Sec > 0)
        {
            BonPanel_ScoreX2.gameObject.GetComponentInChildren<Text>().text = bonuse_ScoreX2Sec.ToString();
            yield return new WaitForSeconds(1f);
            bonuse_ScoreX2Sec--;
        }

        BonPanel_ScoreX2.gameObject.GetComponentInChildren<Text>().text = "0";
        bonuse_ScoreX2 = false;
        BonPanel_ScoreX2.GetComponentInChildren<VisualEffect>().enabled = false;
        BonPanel_ScoreX2.gameObject.SetActive(false);

        Score.scoreCoef = 1;

        //// —œ»—Œ  ¬—≈’ ¡ŒÕ”—Œ¬. ≈—À» »’ Õ≈“, œ¿Õ≈À‹ «¿– €¬¿≈“—ﬂ
        if (!bonuse_DoubleJump && !bonuse_ScoreX2 && !bonuse_CoinX2 && !bonuse_Magnit)
            Bon_Panel.SetActive(false);

    }
    IEnumerator Bon_CoinX2()
    {
        Bon_Panel.SetActive(true);

        BonPanel_CoinX2.GetComponentInChildren<VisualEffect>().enabled = true;
        BonPanel_CoinX2.gameObject.SetActive(true);

        bonuse_CoinX2 = true;
        BonPanel_CoinX2.SetActive(true);
        while (bonuse_CoinX2Sec > 0)
        {
            BonPanel_CoinX2.gameObject.GetComponentInChildren<Text>().text = bonuse_CoinX2Sec.ToString();
            yield return new WaitForSeconds(1f);
            bonuse_CoinX2Sec--;
        }

        BonPanel_CoinX2.gameObject.GetComponentInChildren<Text>().text = "0";
        bonuse_CoinX2 = false;
        BonPanel_CoinX2.GetComponentInChildren<VisualEffect>().enabled = false;
        BonPanel_CoinX2.gameObject.SetActive(false);

        //// —œ»—Œ  ¬—≈’ ¡ŒÕ”—Œ¬. ≈—À» »’ Õ≈“, œ¿Õ≈À‹ «¿– €¬¿≈“—ﬂ
        if (!bonuse_DoubleJump && !bonuse_ScoreX2 && !bonuse_CoinX2 && !bonuse_Magnit)
            Bon_Panel.SetActive(false);
    }
    IEnumerator Bon_Magnit()
    {
        Bon_Panel.SetActive(true);

        BonPanel_Magnit.GetComponentInChildren<VisualEffect>().enabled = true;
        BonPanel_Magnit.gameObject.SetActive(true);

        Magnit.MagnitOn = true;
        bonuse_Magnit = true;
        BonPanel_Magnit.SetActive(true);
        while (bonuse_MagnitSec > 0)
        {
            BonPanel_Magnit.gameObject.GetComponentInChildren<Text>().text = bonuse_MagnitSec.ToString();
            yield return new WaitForSeconds(1f);
            bonuse_MagnitSec--;
        }

        BonPanel_Magnit.gameObject.GetComponentInChildren<Text>().text = "0";
        Magnit.MagnitOn = false;
        bonuse_Magnit = false;
        BonPanel_Magnit.GetComponentInChildren<VisualEffect>().enabled = false;
        BonPanel_Magnit.gameObject.SetActive(false);

        //// —œ»—Œ  ¬—≈’ ¡ŒÕ”—Œ¬. ≈—À» »’ Õ≈“, œ¿Õ≈À‹ «¿– €¬¿≈“—ﬂ
        if (!bonuse_DoubleJump && !bonuse_ScoreX2 && !bonuse_CoinX2 && !bonuse_Magnit)
            Bon_Panel.SetActive(false);
    }
    IEnumerator Bon_Immortal()
    {
        BonPanel_Immortal.SetActive(true);
        BonPanel_Immortal.GetComponentInChildren<VisualEffect>().enabled = true;
        BonPanel_Immortal.gameObject.SetActive(true);

        bonuse_Immortal = true;
        BonPanel_Immortal.SetActive(true);
        while (bonuse_ImmortalSec > 0)
        {
            BonPanel_Immortal.gameObject.GetComponentInChildren<Text>().text = bonuse_ImmortalSec.ToString();
            yield return new WaitForSeconds(1f);
            bonuse_ImmortalSec--;
        }

        BonPanel_Immortal.gameObject.GetComponentInChildren<Text>().text = "0";
        bonuse_Immortal = false;
        BonPanel_Immortal.GetComponentInChildren<VisualEffect>().enabled = false;
        BonPanel_Immortal.gameObject.SetActive(false);

        //// —œ»—Œ  ¬—≈’ ¡ŒÕ”—Œ¬. ≈—À» »’ Õ≈“, œ¿Õ≈À‹ «¿– €¬¿≈“—ﬂ
        if (!bonuse_Immortal)
            BonPanel_Immortal.SetActive(false);
    }

    void SoundPlay(string nameSound, float volume)
    {
        foreach (AudioClip clipEl in audioClips)
        {
            if (clipEl.name == nameSound)
            {
                audioSource.volume = volume;
                audioSource.clip = clipEl;
                audioSource.Play();
            }
        }
    }
}
