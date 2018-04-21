using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    private Stack<GameObject> mDeck;

    // Use this for initialization
    private void Start()
    {
        mDeck = new Stack<GameObject>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void AddCardToDeck(GameObject card)
    {
        Debug.Log("Adding card " + card.name + " to deck");
        card.transform.SetParent(transform.parent);
        card.transform.position = transform.position + Vector3.forward + Vector3.down * 5.0f * mDeck.Count;
        mDeck.Push(card);
    }

    public GameObject GetCard()
    {
        if (mDeck.Count < 1)
        {
            return null;
        }
        return mDeck.Pop();
    }
}