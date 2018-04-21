using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    public Vector3 Center
    {
        get
        {
            RectTransform rT = GetComponent<RectTransform>();
            return transform.position + new Vector3(rT.rect.width * 0.5f, rT.rect.height * 0.5f, 0);
        }
    }

    private Stack<GameObject> mDeck;

    public bool IsEmpty { get { return mDeck.Count == 0; } }

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
        card.transform.position = Center + Vector3.forward + Vector3.down * 5.0f * mDeck.Count;
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