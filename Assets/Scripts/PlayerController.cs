using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction

{
    LEFT = 0,
    RIGHT = 1
}

public enum PlayerState
{
    RUNNING,
    IDLE
}

public class PlayerController : MonoBehaviour
{
    private Rigidbody body;
    public float kRunSpeed = 1.0f;

    private float mRunStart;
    private float mRunDuration;
    private Direction mRunDir;
    private PlayerState mState = PlayerState.IDLE;

    // Use this for initialization
    private void Start()
    {
        Debug.Log("Hello player here");
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void DoRun()
    {
        // We're outta running time
        if (mRunStart + mRunDuration < Time.time)
        {
            mState = PlayerState.IDLE;
        }
        else
        {
            Vector3 newPosition = transform.position;
            switch (mRunDir)
            {
                case Direction.LEFT:
                    newPosition = transform.position + Vector3.left * kRunSpeed * Time.deltaTime;
                    break;
                case Direction.RIGHT:
                    newPosition = transform.position + Vector3.right * kRunSpeed * Time.deltaTime;
                    break;
            }
            body.MovePosition(newPosition);
        }
    }

    private void FixedUpdate()
    {
        if (mState == PlayerState.RUNNING)
        {
            DoRun();
        }
    }

    #region User Actions

    public void ActionJump()
    {
        Debug.Log("Player Jumpin");
        body.AddForce(Vector3.up * 10f, ForceMode.Impulse);
    }

    public void ActionRunLeft(float dist)
    {
        ActionRun(Direction.LEFT, dist);
    }

    public void ActionRunRight(float dist)
    {
        ActionRun(Direction.RIGHT, dist);
    }

    private void ActionRun(Direction dir, float dist)
    {
        mState = PlayerState.RUNNING;
        mRunDir = dir;
        mRunStart = Time.time;
        mRunDuration = dist;
    }

    #endregion User Actions
}