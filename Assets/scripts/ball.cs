using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody2D rb;
    private GoalKeeper goalKeeper;
    private Vector3 initialPosition;
    public float spinDuration = 2f; 
    private float currentSpinSpeed = 1000000;

    void Start()
    {
        goalKeeper = FindAnyObjectByType<GoalKeeper>();
        rb = GetComponent<Rigidbody2D>();
        RecordInitialPosition();
    }

    private void RecordInitialPosition()
    {
        initialPosition = transform.position;
    }

    public void ResetBallToInitialPosition()
    {
        transform.position = initialPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("wall"))
        {
            goalKeeper.ballactive = true;
        }
    }

    public void Kick(Vector2 direction, float force)
    {
        rb.AddForce(direction * force, ForceMode2D.Impulse);

        float kickSpeed = rb.velocity.magnitude;

        
        float spinDuration = 360f / (kickSpeed * currentSpinSpeed);
        
        StartCoroutine(SpinAnimation());
    }

    private IEnumerator SpinAnimation()
    {
        AnimationCurve slowdownCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        float totalRotation = 360 * currentSpinSpeed;

        float startRotation = transform.eulerAngles.z;

        float currentSpinTime = 0f;
        float currentRotation = startRotation;

        while (currentSpinTime < spinDuration)
        {
            currentSpinTime += Time.deltaTime;

            float t = Mathf.Clamp01(currentSpinTime / spinDuration);

            float curveValue = slowdownCurve.Evaluate(t);

            currentRotation = Mathf.Lerp(startRotation, totalRotation, curveValue);

            transform.eulerAngles = new Vector3(0, 0, currentRotation);

            yield return null;
        }

    }
}
