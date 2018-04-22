using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    private GameController mGameController;
    private Renderer mRenderer;

    // Use this for initialization
    private void Start()
    {
        mRenderer = GetComponent<Renderer>();
        mGameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    private void Update()
    {
        Vector2 offset = mRenderer.material.mainTextureOffset;
        offset.x += 0.01f;
        mRenderer.material.mainTextureOffset = offset;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player Completed the game");
        Application.OpenURL("https://twitter.com/intent/tweet?text=I%20can't%20believe%20I%20beat%20%40zambini845%20's%20ridiculous%20game&hashtags=LDJAM%2CLDJAM41");
    }
}