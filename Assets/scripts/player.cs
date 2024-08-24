using UnityEngine;
using UnityEngine.UI;
using SimpleInputNamespace;
using TMPro;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine.TextCore.Text;
using System.Collections.Generic;
using System.Collections;
using UnityEditor.AssetImporters;

public class Player : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody2D rb;
    public GameObject Indicator;
    private Animator animator;
    private static Player activePlayer;

    private bool isSelected = false;
    private bool playerIsRun = false;
    private SpriteRenderer sprite;
    private Ball ball;
    public float maxKickForce = 20.0f;
    private Vector2 initialTouchPosition;
    private bool isKicking = false;

    public GameObject runpos;
    public GameObject balltransform;
    public GameObject posball;
    private bool hasTriggered = false;

    private bool ballkick = false;

    public float maxStamina = 100.0f;
    public float currentStamina;
    public float staminaDecreaseRate = 10.0f;
    public float staminaRecoveryRate = 5.0f;
    public float staminaDecreaseWhileHoldingBall = 5.0f;
    public float staminaDecreaseOnKick = 15.0f;
    public Image staminaBar;
    private MagneteSkill magneteSkill;
    private float attractRadius = 1;
    private float attractForce = 19;
    private GoalKeeper goalKeeper;
    public enum PlayerType
    {
        Player1,
        Player2,
        Player3,
        Player4,
        Player5
    }

    public PlayerType playerType;
    private Audiomanager audiomanager;
    private static Dictionary<PlayerType, Vector2> initialPositions = new Dictionary<PlayerType, Vector2>();

    void Start()
    {
        goalKeeper = FindAnyObjectByType<GoalKeeper>();
        audiomanager = FindAnyObjectByType<Audiomanager>();
        magneteSkill = FindObjectOfType<MagneteSkill>();
        ball = FindObjectOfType<Ball>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if (Indicator != null)
        {
            Indicator.SetActive(false);
        }

        currentStamina = maxStamina;
        RecordInitialPositions();
        upgradescharacter();

    }
    public static void RecordInitialPositions()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null)
            {
                initialPositions[playerScript.playerType] = player.transform.position;
            }
        }
    }
    public static void ResetAllPlayersToInitialPositions()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null && initialPositions.ContainsKey(playerScript.playerType))
            {
                player.transform.position = initialPositions[playerScript.playerType];
            }
        }
    }
    public bool AllOtherPlayersWithoutBall()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            Player playerMovement = player.GetComponent<Player>();
            if (playerMovement != null && playerMovement != this && playerMovement.hasTriggered)
            {
                return false;
            }
        }

        return true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball") && AllOtherPlayersWithoutBall())
        {
            hasTriggered = true; 
            
            

        }
    }
   




    private void OnDrawGizmos()
    {
        if (magneteSkill != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attractRadius);
        }
    }

    private void FixedUpdate()
    {
        if (!magneteSkill.activateSkill) return;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float closestDistance = float.MaxValue;
        GameObject closestPlayer = null;

        foreach (GameObject player in players)
        {
            if (player != gameObject)
            {
                float distance = Vector2.Distance(ball.transform.position, player.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlayer = player;
                }
            }
        }

        if (closestPlayer != null)
        {
            Vector3 direction = closestPlayer.transform.position - ball.transform.position;
            ball.GetComponent<Rigidbody2D>().AddForce(direction.normalized * attractForce * Time.fixedDeltaTime);
        }
    }

    void Update()
    {
        if (hasTriggered)
        {
            ballkick = false;
            Select();
        }
       
        if (ballkick == true)
        {
            Deselect();
        }

        if (hasTriggered && balltransform != null)
        {
            balltransform.transform.position = posball.transform.position;
        }

        
        animationsettings();
        staminaBar.fillAmount = currentStamina / maxStamina;

        if (isSelected)
        {
            float horizontal = SimpleInput.GetAxis("Horizontal");
            float vertical = SimpleInput.GetAxis("Vertical");

            if (horizontal != 0 || vertical != 0)
            {
                if (currentStamina > 0)
                {
                    playerIsRun = true;
                    float staminaDrain = staminaDecreaseRate;
                    if (hasTriggered)
                    {
                        staminaDrain += staminaDecreaseWhileHoldingBall;
                    }
                    currentStamina -= staminaDrain * Time.deltaTime;
                    currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
                }
                if (currentStamina < 15)
                {
                    playerIsRun = false;
                }
            }
            else
            {
                playerIsRun = false;
            }

            if (horizontal < 0)
            {
                sprite.flipX = true;
            }
            else if (horizontal > 0)
            {
                sprite.flipX = false;
            }

            Vector2 direction = new Vector2(horizontal, vertical).normalized;
            rb.velocity = playerIsRun ? direction * speed : Vector2.zero;

            if (hasTriggered)
            {
                HandleTouchInput();
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    Vector3 clickPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    clickPosition.z = 0f;

                    Collider2D hitCollider = Physics2D.OverlapPoint(clickPosition);

                    if (hitCollider != null && hitCollider.gameObject == gameObject)
                    {
                        if (!hasTriggered)
                        {
                            ballkick = false;
                            Select();
                        }
                    }
                    else
                    {
                        playerIsRun = false;
                        Deselect();
                    }
                }
            }
        }

        if (!playerIsRun)
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
        
    }
   
    private void upgradescharacter()
    {
        string playerPrefix = "";
        switch (playerType)
        {
            case PlayerType.Player1:
                playerPrefix = "Player1";
                break;
            case PlayerType.Player2:
                playerPrefix = "Player2";
                break;
            case PlayerType.Player3:
                playerPrefix = "Player3";
                break;
            case PlayerType.Player4:
                playerPrefix = "Player4";
                break;
            case PlayerType.Player5:
                playerPrefix = "Player5";
                break;
        }

        string[] keys = {
            "SpeedLevel",
            "MaxStamina",
            "MaxKickForce",
            "StaminaDecreaseOnKick",
            "StaminaDecreaseWhileHoldingBall",
            "StaminaDecreaseRate",
            "StaminaRecoveryRate"
        };

        foreach (string key in keys)
        {
            string playerPrefKey = $"{playerPrefix}_{key}";
            if (PlayerPrefs.HasKey(playerPrefKey))
            {
                float value = PlayerPrefs.GetFloat(playerPrefKey);
                switch (key)
                {
                    case "SpeedLevel":
                        speed = value;
                        break;
                    case "MaxStamina":
                        maxStamina = value;
                        break;
                    case "MaxKickForce":
                        maxKickForce = value;
                        break;
                    case "StaminaDecreaseOnKick":
                        staminaDecreaseOnKick = value;
                        break;
                    case "StaminaDecreaseWhileHoldingBall":
                        staminaDecreaseWhileHoldingBall = value;
                        break;
                    case "StaminaDecreaseRate":
                        staminaDecreaseRate = value;
                        break;
                    case "StaminaRecoveryRate":
                        staminaRecoveryRate = value;
                        break;
                }
            }
        }
    }

    private void Select()
    {
        if (activePlayer != null && activePlayer != this)
        {
            activePlayer.Deselect();
        }
        isSelected = true;
        activePlayer = this;
        if (!hasTriggered)
        {
            playerIsRun = true;
        }

        if (Indicator != null)
        {
            Indicator.SetActive(true);
        }
    }

    private void Deselect()
    {
        isSelected = false;
        if (activePlayer == this)
        {
            activePlayer = null;
        }

        playerIsRun = false;

        if (Indicator != null)
        {
            Indicator.SetActive(false);
        }
    }

    private void animationsettings()
    {
        animator.SetFloat("velocity", playerIsRun ? 1 : -1);
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                if (Vector2.Distance(touchPosition, transform.position) < 1.0f)
                {
                    initialTouchPosition = touch.position;
                    isKicking = true;
                }
            }
            else if (touch.phase == TouchPhase.Ended && isKicking)
            {
                Vector2 finalTouchPosition = touch.position;
                Vector2 direction = (finalTouchPosition - initialTouchPosition).normalized;

                
                float distance = Vector2.Distance(initialTouchPosition, finalTouchPosition);
                distance = Mathf.Clamp(distance, 0, 1); 

                float kickForce = maxKickForce * distance; 

                audiomanager.PlaySFX(audiomanager.ballkick);
                ball.Kick(direction, kickForce);
                isKicking = false;
                hasTriggered = false;
                ballkick = true;

                currentStamina -= staminaDecreaseOnKick;
                currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            }
        }
    }


}
