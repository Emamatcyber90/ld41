using UnityEngine;

public class PlayingCardController : MonoBehaviour
{
    #region Drag Properties

    private Vector3 mDragOffset;
    private Transform mDropTarget;
    private Vector3 kDropOffset = new Vector3(0, 0, -1.0f);

    #endregion Drag Properties

    // Use this for initialization
    private void Start()
    {
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
        transform.position = mDropTarget.transform.position + kDropOffset;
    }
}