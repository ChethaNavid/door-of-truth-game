using UnityEngine;

public class ButtonSetActiveDelay : MonoBehaviour
{
    public GameObject target;   // object to enable/disable
    public float delay = 2f;
    public bool setActive = true;

    public void OnButtonClick()
    {
        Invoke(nameof(DoAction), delay);
    }

    void DoAction()
    {
        if (target != null)
            target.SetActive(setActive);
    }
}
