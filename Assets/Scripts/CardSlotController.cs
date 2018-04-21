using UnityEngine;

public class CardSlotController : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        RectTransform rT = GetComponent<RectTransform>();
        collider.size = new Vector3(rT.rect.width, rT.rect.height, 20);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}