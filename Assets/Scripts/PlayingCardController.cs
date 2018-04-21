﻿using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Power cards have:
/// Direction + Power
/// </summary>
public enum CardType
{
    RunLeft,
    RunRight,
    JumpLow,
    JumpHigh,
    Block
}

public class PlayingCardController : MonoBehaviour
{
    public Sprite[] mReplacementSprites;
    public CardType cardType;
    public int cardPower;

    private PlayerController mPlayer;
    private GameController mGameController;

    #region Drag Properties

    private Vector3 mDragOffset;

    // If this is getting picked up, keep tabs on the starter slot in to clear it out
    private Transform mSourceTarget;

    private Transform mDropTarget;
    private Vector3 kDropOffset = new Vector3(0, 0, -1.0f);

    #endregion Drag Properties

    // Use this for initialization
    private void Start()
    {
        mPlayer = GameObject.Find("Player").GetComponent<PlayerController>();
        mGameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    public void LoadAsset()
    {
        Image img = GetComponent<Image>();
        switch (cardType)
        {
            case CardType.JumpHigh:
                img.sprite = mReplacementSprites[0];
                break;
            case CardType.JumpLow:
                img.sprite = mReplacementSprites[1];
                break;
            case CardType.RunRight:
                img.sprite = mReplacementSprites[2];
                break;
            case CardType.RunLeft:
                img.sprite = mReplacementSprites[3];
                break;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 50, Color.blue);
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(string.Format("{0} >>> Entering >>> {1}", gameObject.name, other.name));
        mDropTarget = other.transform;
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log(string.Format("{0} <<< Exiting <<< {1}", gameObject.name, other.name));
        if (mDropTarget == other.transform)
        {
            mDropTarget = null;
        }
    }

    public void OnBeginDrag()
    {
        mSourceTarget = mDropTarget;
        mDragOffset = Input.mousePosition - transform.position;
    }

    public void OnDrag()
    {
        transform.position = Input.mousePosition - mDragOffset;
    }

    public void OnEndDrag()
    {
        if (mDropTarget)
        {
            GoToDropTarget();
        }
    }

    private void GoToDropTarget()
    {
        // First clear the old CSC if it is one
        if (mSourceTarget != null)
        {
            CardSlotController oldCSC = mSourceTarget.GetComponent<CardSlotController>();
            if (oldCSC)
            {
                oldCSC.TakeCard();
            }
        }
        switch (mDropTarget.tag)
        {
            case "Card Slot":
                CardSlotController newCSC = mDropTarget.GetComponent<CardSlotController>();
                if (newCSC != null)
                {
                    newCSC.PlaceCard(gameObject);
                }
                break;
            case "PlayZone":
                Debug.Log("Playing card: " + gameObject.name);
                mGameController.ActivateCard(this);
                break;
        }

        transform.position = mDropTarget.transform.position + kDropOffset;
    }

    private void OnDrawGizmos()
    {
        TextGizmo.Instance.DrawText(transform.position, string.Format("{0},{1}", cardType, cardPower));
    }
}