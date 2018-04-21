using UnityEngine;

public class CardSlotController : MonoBehaviour
{
    private GameObject mCurrentCard;

    public bool isOccupied { get { return mCurrentCard != null; } }

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

    public void PlaceCard(GameObject card)
    {
        Debug.Log(gameObject.name + " receiving card " + card.name);
        mCurrentCard = card;
    }

    public GameObject TakeCard()
    {
        GameObject result = mCurrentCard;
        if (mCurrentCard != null)
        {
            Debug.Log(gameObject.name + " removing card " + mCurrentCard.name);
            mCurrentCard = null;
        }
        return mCurrentCard;
    }

    private void OnDrawGizmos()
    {
        TextGizmo.Instance.DrawText(transform.position + Vector3.down * 40f, string.Format("occupied: {0}", isOccupied));
    }
}