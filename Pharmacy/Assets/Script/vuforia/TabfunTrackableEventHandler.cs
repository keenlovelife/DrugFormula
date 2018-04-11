using UnityEngine;
using Vuforia;
using UnityEngine.UI;

public class TabfunTrackableEventHandler : MonoBehaviour,
                                            ITrackableEventHandler
{

    private TrackableBehaviour mTrackableBehaviour;

    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }

    }

    public void OnTrackableStateChanged(
                                    TrackableBehaviour.Status previousStatus,
                                    TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            OnTrackingFound();
        }
        else
        {
          // OnTrackingLost();
        }
    }


    private void OnTrackingFound()
    {
        MainController.Instance.FoundedTarget(mTrackableBehaviour.TrackableName);

        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
        Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
        foreach (Renderer component in rendererComponents)
            component.enabled = true;
        foreach (Collider component in colliderComponents)
            component.enabled = true;
    }


    private void OnTrackingLost()
    {
        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
        Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

        // Disable rendering:
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = false;
        }

        // Disable colliders:
        foreach (Collider component in colliderComponents)
        {
            component.enabled = false;
        }
        var log = " >>>> " + mTrackableBehaviour.TrackableName + " 丢失！";

        Debug.Log(log);
    }
    
}
