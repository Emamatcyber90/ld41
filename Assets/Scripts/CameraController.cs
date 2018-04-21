using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 kCameraOffset = new Vector3(0, -3, -10);
    private GameObject mPlayer;
    private Vector3 mVelocity = Vector3.zero;

    // Use this for initialization
    private void Start()
    {
        mPlayer = GameObject.Find("Player");
    }

    // Update is called once per frame
    private void Update()
    {
        // Move toward the player
        Vector3 targetPosition = (mPlayer.transform.position + kCameraOffset);
        Debug.DrawLine(transform.position, targetPosition, Color.yellow);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref mVelocity, 0.1f);
    }
}