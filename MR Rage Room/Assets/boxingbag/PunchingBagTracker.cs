using UnityEngine;

public class PunchingBagTracker : MonoBehaviour
{
    void OnEnable()
    {
        if (PunchingBagManager.Instance != null)
            PunchingBagManager.Instance.RegisterBag(gameObject);
    }

    void OnDisable()
    {
        if (PunchingBagManager.Instance != null)
            PunchingBagManager.Instance.UnregisterBag(gameObject);
    }
}
