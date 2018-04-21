using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardPileController : MonoBehaviour
{
    private Stack<GameObject> mDiscardPile;

    // Use this for initialization
    private void Start()
    {
        mDiscardPile = new Stack<GameObject>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void AddToDiscard(GameObject card)
    {
    }
}