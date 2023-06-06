using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float turnSpeed = 180f;
    [SerializeField] private Rigidbody myRb;
    [SerializeField] private GameObject gameOverObject;
    [SerializeField] private TMP_Text scoreTracker;
    [SerializeField] private TMP_Text timeTracker;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject gameOverTitle;
    [SerializeField] private GameObject gameCompleteTitle;
    [SerializeField] private TMP_Text timeText;
 
    [Header("Level Management")]
    [SerializeField] private int maxScore = 10;
    [SerializeField] private float minRadiusOfField = 20f;
    [SerializeField] private float maxRadiusOfField = 50f;
    [SerializeField] private GameObject collectiblePrefab;
    [SerializeField] private ThreatFollower myTf;
    private Gamepad gamepad;

    private float score = 0f;
    private float timer = 0f;
    private bool isDisabled = false;

    private float collectFill = 0f;
    private float collectFillMax = 3f;
    [SerializeField] private GameObject fillGraphic;
    [SerializeField] private Image fillImage;

    [SerializeField] private AudioSource myAudio;
    // Start is called before the first frame update
    void Start()
    {
        gameOverObject.SetActive(false);
        gamepad = InputSystem.GetDevice<Gamepad>();
        SpawnObjects();
    }

    private void SpawnObjects()
    {
        for(int i = 0; i < maxScore; i++)
        {
            float angle = Random.Range(0f,360f);
            float distance = Random.Range(minRadiusOfField,maxRadiusOfField);
            Vector3 randPos = new Vector3(Mathf.Cos(angle)*distance, 0f, Mathf.Sin(angle)*distance);
            Instantiate(collectiblePrefab, randPos, transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire2"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if(isDisabled)
        {
            return;
        }
        timer += Time.deltaTime;
        timeTracker.text = Mathf.Floor(timer) + "." + Mathf.Floor((timer*10f)%10f);
        Vector3 direction = ((transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"))).normalized;
        transform.Rotate(0f, Gamepad.current.rightStick.x.ReadValue() * Time.deltaTime * turnSpeed, 0f);
        myRb.velocity = direction * moveSpeed;
    }

    private void OnCollisionStay(Collision other) {
        if(other.gameObject.tag == "Threat")
        {
            KillPlayer();
        }
        else if(other.gameObject.tag == "Collectible" && !(other.gameObject.GetComponent<ServerManager>().isTagged()))
        {
            fillGraphic.SetActive(true);
            fillImage.fillAmount = collectFill/collectFillMax;
            if(collectFill <= collectFillMax)
            {
                collectFill += Time.deltaTime;
            }
            else
            {
                other.gameObject.GetComponent<ServerManager>().SetTagged();
                score += 1f;
                myAudio.pitch = 1f + (0.5f * score/maxScore);
                scoreTracker.text = "Score: " + score;
                myTf.Activate();
                if(score/maxScore > 0.5f)
                {
                    myTf.StartFrontAttacker();
                }
                if(score==maxScore)
                {
                    gameCompleteTitle.SetActive(true);
                    gameComplete();
                }
            }
        }
    }
    
    private void OnCollisionExit(Collision other)
    {
        collectFill = 0f;
        fillGraphic.SetActive(false);
    }

    public void KillPlayer()
    {
        gameOverTitle.SetActive(true);
        gameComplete();
    }

    private void gameComplete()
    {
        isDisabled = true;
        gameOverObject.SetActive(true);
        scoreText.text = "Score: " + score;
        timeText.text = "Time: " + timer;
        print("gameover");
        myTf.gameObject.SetActive(false);
    }
}
