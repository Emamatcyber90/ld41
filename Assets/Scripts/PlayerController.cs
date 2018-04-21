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
    public float kJumpPower = 30f;
    public float kRunSpeed = 1.0f;

    private float mRunStart;
    private float mRunDuration;
    private Direction mRunDir;
    private PlayerState mState = PlayerState.IDLE;

    public const float kControlCheckDist = 0.5f;
    private bool mIsGrounded = false;
    private LayerMask mGroundedIgnoreMask;

    // Use this for initialization
    private void Start()
    {
        Debug.Log("Hello player here");
        body = GetComponent<Rigidbody>();
        mGroundedIgnoreMask = ~LayerMask.GetMask("player");
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 dirVec = -transform.up;
        const float kGroundCheckDist = 1.0f;
        Vector3 castPointLeft = transform.position + Vector3.left * 0.5f;
        Vector3 castPointCenter = transform.position;
        Vector3 castPointRight = transform.position + Vector3.right * 0.5f;
        Debug.DrawRay(castPointLeft, dirVec * kGroundCheckDist, Color.red);
        Debug.DrawRay(castPointCenter, dirVec * kGroundCheckDist, Color.red);
        Debug.DrawRay(castPointRight, dirVec * kGroundCheckDist, Color.red);
        bool leftGround = Physics.Raycast(castPointLeft, dirVec, kGroundCheckDist, mGroundedIgnoreMask);
        bool centerGround = Physics.Raycast(castPointCenter, dirVec, kGroundCheckDist, mGroundedIgnoreMask);
        bool rightGround = Physics.Raycast(castPointRight, dirVec, kGroundCheckDist, mGroundedIgnoreMask);
        mIsGrounded = leftGround || centerGround || rightGround;
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
            // Cast a capsule toward the direction
            Vector3 dirVec = Vector3.zero;
            Vector3 newPosition = transform.position;
            switch (mRunDir)
            {
                case Direction.LEFT:
                    dirVec = Vector3.left;
                    break;
                case Direction.RIGHT:
                    dirVec = Vector3.right;
                    break;
            }
            // Instead of doing a capsule just #CLAMJAM it
            Vector3 castPointBottom = transform.position + (-transform.up * 0.25f);
            Vector3 castPointCenter = transform.position;
            Vector3 castPointTop = transform.position + transform.up * 0.25f;
            Debug.DrawRay(castPointBottom, dirVec * kControlCheckDist, Color.blue);
            Debug.DrawRay(castPointCenter, dirVec * kControlCheckDist, Color.blue);
            Debug.DrawRay(castPointTop, dirVec * kControlCheckDist, Color.blue);
            bool bottomCastHit = Physics.Raycast(castPointBottom, dirVec, kControlCheckDist, mGroundedIgnoreMask);
            bool centerCastHit = Physics.Raycast(castPointBottom, dirVec, kControlCheckDist, mGroundedIgnoreMask);
            bool topCastHit = Physics.Raycast(castPointBottom, dirVec, kControlCheckDist, mGroundedIgnoreMask);
            newPosition += dirVec * kRunSpeed * Time.deltaTime;
            if (!(bottomCastHit || centerCastHit || topCastHit))
            {
                body.MovePosition(newPosition);
            }
        }
    }

    private void FixedUpdate()
    {
        if (mState == PlayerState.RUNNING)
        {
            DoRun();
        }
    }

    public void ActOnCard(PlayingCardController card)
    {
        Debug.Log("Player acting on card: " + card.gameObject.name);
        switch (card.cardType)
        {
            case CardType.RunLeft:
                ActionRunLeft(card.cardPower);
                break;
            case CardType.RunRight:
                ActionRunRight(card.cardPower);
                break;
            case CardType.JumpLow:
                ActionJump();
                break;
            case CardType.JumpHigh:
                ActionJump();
                break;
            case CardType.Block:
                break;
        }
    }

    #region User Actions

    public void ActionJump()
    {
        Debug.Log("Player Jumpin");
        if (mIsGrounded)
        {
            body.AddForce(Vector3.up * kJumpPower, ForceMode.Impulse);
        }
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
        mRunDuration = 1.0f;
    }

    #endregion User Actions

    public void OnDrawGizmos()
    {
        TextGizmo.Instance.DrawText(transform.position, mState.ToString());
        TextGizmo.Instance.DrawText(transform.position + transform.up * 0.5f, string.Format("isGrounded: {0}", mIsGrounded));
    }
}