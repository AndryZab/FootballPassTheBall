using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GoalKeeper : MonoBehaviour
{
    public GameObject ball;
    private Ball scriptball;
    public GameObject posball;
    public bool ballactive = false;
    public float ballSpeed = 5.0f;
    public int countGoals = 0;
    private soccergoal soccergoal;

    private void Start()
    {
        soccergoal = FindAnyObjectByType<soccergoal>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            if (soccergoal.goal == false)
            {
                ball.SetActive(false);
                ballactive = true;
            }
        }
    }

    void Update()
    {
        if (ballactive)
        {
            ball.SetActive(true);
            ball.transform.position = posball.transform.position;


            float randomAngle = Random.Range(-45.0f, 45.0f);
            Vector2 direction = new Vector2(Mathf.Sin(randomAngle * Mathf.Deg2Rad), -Mathf.Cos(randomAngle * Mathf.Deg2Rad)); 


            Rigidbody2D ballRb = ball.GetComponent<Rigidbody2D>();
            ballRb.velocity = direction * ballSpeed;

            ballactive = false;

        }
    }
}
