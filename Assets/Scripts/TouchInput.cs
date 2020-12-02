using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TouchInput : Singleton<TouchInput>
{
    public UnityAction<RaycastHit> tapAction;
    public UnityAction<float> pinchAction;

    [SerializeField] private LayerMask tapLayerMask = 0;

    private Camera cam = null;
    private RaycastHit[] hitBuffer = new RaycastHit[10];
    private Vector2 initialGap = Vector2.zero;
    private bool suspendUntilPinchReleased = false;

    private void Awake()
    {
        cam = Camera.main;
        Instance = this;
    }

    private void Update()
    {
        if (suspendUntilPinchReleased)
        {
            DeterminePinchEvent();
            return;
        }

        switch (Input.touchCount)
        {
            case 0:
                return;

            case 1:
                DetermineTapEvent();
                break;

            case 2:
                DeterminePinchEvent();
                break;
        }
    }

    private void DetermineTapEvent()
    {
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Ended)
        {
            // should prevent tap event calls when UI is in the way
            if (EventSystem.current.IsPointerOverGameObject())
            {
                //Debug.Log("Clicked on UI");
                return;
            }

            Ray ray = cam.ScreenPointToRay(touch.position);
            if (Physics.RaycastNonAlloc(ray, hitBuffer, 1000f, tapLayerMask) > 0)
            {
                tapAction?.Invoke(hitBuffer[0]);
            }
        }
    }

    private void DeterminePinchEvent()
    {

        switch (Input.touchCount)
        {
            case 0:
                //state = "not pinching";
                suspendUntilPinchReleased = false;
                return;

            case 1:
                //state = "waiting for release";
                return;

            case 2:
                //state = "two fingers not moving";
                suspendUntilPinchReleased = true;
                break;
        }

        Touch first = Input.GetTouch(0);
        Touch second = Input.GetTouch(1);

        if (first.phase == TouchPhase.Began || second.phase == TouchPhase.Began)
        {
            initialGap = second.position - first.position;
            return;
        }

        if (first.phase == TouchPhase.Moved || second.phase == TouchPhase.Moved)
        {
            //state = "pinch action invoked";
            Vector2 currentGap = second.position - first.position;
            float delta = currentGap.sqrMagnitude / initialGap.sqrMagnitude;
            pinchAction?.Invoke(delta);
            initialGap = currentGap;
        }
    }
}