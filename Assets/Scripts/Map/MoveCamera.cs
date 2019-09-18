using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    // public bool LockCam = true;

    public static MoveCamera instance = null;

    public float MaxPos = 29f;
    public float MinPos = 3.5f;


    private bool isMoving = false;
    private Vector3 StartPosition, CurrentPosition, EndPosition;
    private float dir;
    private float StartTime;
    private float distance;
    private float ElapsedTime;
    public float Velocity;
    private float targetPosition;
    private float temp;


    private void Awake()
    {
        if (instance == null) instance = this;

        else if (instance != this) Destroy(gameObject);
    }

    private void Start()
    {
        if (SoundManager.instance.Depth == 0)
        {
            isMoving = true;

            StartCoroutine(AutoMove(Time.fixedTime, MinPos));
        }

        else transform.position = SoundManager.instance.CamPos;
    }

    private void Update()
    {
        if (MapController.isOpen
            || isMoving) return;

        if (Input.GetMouseButtonDown(0))
        {
            StopAllCoroutines();

            //  Debug.Log("Start Touch");

            StartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            StartTime = Time.fixedTime;
        }
        else if (Input.GetMouseButton(0))
        {
            CurrentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            dir = StartPosition.y - CurrentPosition.y;

            if (dir > Mathf.Epsilon && transform.position.y < MaxPos
                || dir < Mathf.Epsilon && transform.position.y > MinPos) transform.Translate(Vector3.up * dir);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // Debug.Log("End Touch");

            EndPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            distance = StartPosition.y - EndPosition.y;

            ElapsedTime = Time.fixedTime - StartTime;

            Velocity = Mathf.Abs(2 * distance / ElapsedTime) - 1f < Mathf.Epsilon ? 1f : 2 * distance / ElapsedTime;

            // Debug.Log("Start Velocity: "+ Velocity);

            StartCoroutine(Move(Time.fixedTime));
        }
    }

    private IEnumerator Move(float endTime)
    {
        // 스크롤 최대 시간 3초
        while (Time.fixedTime - endTime < 3f)
        {
            if (dir > Mathf.Epsilon && transform.position.y < MaxPos
                || dir < Mathf.Epsilon && transform.position.y > MinPos)
            {
                transform.Translate(Vector3.up * Velocity * Time.deltaTime);

                Velocity = Velocity > 0f ? Velocity - 0.3f : Velocity + 0.3f;
            }
            yield return null;
        }
        Velocity = 0;
    }
    private IEnumerator AutoMove(float start, float endPosition)
    {
        yield return new WaitForSeconds(0.5f);

        while (Time.fixedTime - start < 5f)
        {
            transform.position = new Vector3(transform.position.x,
                Mathf.SmoothDamp(transform.position.y, endPosition, ref temp, 1f, 10f),
                transform.position.z);

            yield return null;
        }
        yield return new WaitForSeconds(0.5f);

        while (Time.fixedTime - start < 10.5f)
        {
            transform.position = new Vector3(transform.position.x,
                Mathf.SmoothDamp(transform.position.y, MaxPos, ref temp, 1f, 10f),
                transform.position.z);

            yield return null;
        }
        isMoving = false;
    }
}
