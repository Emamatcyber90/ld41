using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Starting,
    Initializing,
    Playing,
    Paused,
    Menu
}

public class GameController : MonoBehaviour
{
    public GameState mState = GameState.Playing;

    private DeckController mDeck;
    private HandController mHand;
    private DiscardPileController mDiscardPile;
    private PlayerController mPlayer;
    private GameObject mCamera;

    public GameObject PREFAB_CARD;

    // Use this for initialization
    private void Start()
    {
        mDeck = GameObject.Find("Deck").GetComponent<DeckController>();
        mHand = GameObject.Find("Action Hand").GetComponent<HandController>();
        mDiscardPile = GameObject.Find("Discard Pile").GetComponent<DiscardPileController>();
        mPlayer = GameObject.Find("Player").GetComponent<PlayerController>();
        mCamera = GameObject.Find("Main Camera");

        Debug.Assert(mDeck != null);
        Debug.Assert(mHand != null);
        Debug.Assert(mDiscardPile != null);
        Debug.Assert(mPlayer != null);
        Debug.Assert(mCamera != null);

        Debug.Assert(PREFAB_CARD != null, "You must assign the Card Prefab in the inspector");
    }

    // Update is called once per frame
    private void Update()
    {
        switch (mState)
        {
            case GameState.Starting:
                // Initialize the game
                InitializeGameplay();
                Invoke("RecursivelyAddCardsToHand", 2.5f);
                mState = GameState.Initializing;
                break;
            case GameState.Initializing:
                break;
            case GameState.Playing:
                if (mHand.HasRoom && !mDeck.IsEmpty)
                {
                    AddCardToHand();
                }
                break;
        }
    }

    private void StartGameCallback()
    {
        mState = GameState.Playing;
        Debug.Log("Starting game!");
    }

    /// <summary>
    /// Stupid function to recursively invoke itself until no cards exist
    /// </summary>
    private void RecursivelyAddCardsToHand()
    {
        if (mHand.HasRoom)
        {
            Invoke("RecursivelyAddCardsToHand", 0.25f);
            AddCardToHand();
        }
        else
        {
            Debug.Log("Finished recursively adding cards to hand");
            Invoke("StartGameCallback", 1.0f);
        }
    }

    private void InitializeGameplay()
    {
        // Create 10 cards for your starter deck
        GameObject[] newDeck = new GameObject[]
        {
            MakeCard(CardType.RunLeft, 1),
            MakeCard(CardType.RunLeft, 1),
            MakeCard(CardType.RunLeft, 2),
            MakeCard(CardType.RunRight, 1),
            MakeCard(CardType.RunRight, 1),
            MakeCard(CardType.RunRight, 2),
            MakeCard(CardType.RunRight, 2),
            MakeCard(CardType.JumpLow, 1),
            MakeCard(CardType.JumpLow, 1),
            MakeCard(CardType.JumpLow, 1)
        };
        foreach (GameObject newCard in newDeck)
        {
            mDeck.AddCardToDeck(newCard);
        }
    }

    private GameObject MakeCard(CardType type, int power = -1)
    {
        // Random for now
        GameObject result = Instantiate(PREFAB_CARD);
        PlayingCardController card = result.GetComponent<PlayingCardController>();
        card.cardType = type;
        card.cardPower = power;
        card.LoadAsset();
        Debug.Log("New card created: " + card.cardType + ", " + card.cardPower);
        result.name = string.Format("card_{0}_{1}", card.cardType, card.cardPower);
        return result;
    }

    public void AddCardToHand()
    {
        if (mHand.HasRoom)
        {
            GameObject newCard = mDeck.GetCard();
            if (newCard != null)
            {
                mHand.TakeCardIntoHand(newCard);
            }
        }
    }

    public void ActivateCard(PlayingCardController card)
    {
        mPlayer.ActOnCard(card);
        mDiscardPile.AddToDiscard(card.gameObject);
    }

    public void ReshuffleDiscard()
    {
        if (!mDeck.IsEmpty)
        {
            Debug.LogWarning("Called Reshuffle but the deck isn't empty");
            return;
        }
        Debug.Log("Shuffling Discard into deck!");
        List<GameObject> newDeck = new List<GameObject>();
        while (!mDiscardPile.IsEmpty)
        {
            newDeck.Add(mDiscardPile.RemoveFromDiscard());
        }
        // Randomly place each card back into the deck
        newDeck.Sort((left, right) => 1 - Random.Range(0, 3));
        foreach (GameObject go in newDeck)
        {
            mDeck.AddCardToDeck(go, true);
        }
    }
}