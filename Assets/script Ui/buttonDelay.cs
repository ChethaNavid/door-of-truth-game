using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonToggleObjectWithDelay : MonoBehaviour
{
    [Header("Assign the button here")]
    public Button targetButton;

    [Header("Assign the GameObject(s) you want to toggle")]
    public GameObject[] objectsToToggle;

    [Header("Toggle mode")]
    public bool setActiveOnClick = true; // true = active, false = inactive, toggle method available

    [Header("Delay in seconds before action")]
    public float delay = 0f;

    private void Start()
    {
        if (targetButton != null)
        {
            targetButton.onClick.AddListener(OnButtonClick);
        }
    }

    private void OnButtonClick()
    {
        StartCoroutine(ToggleObjectsWithDelay());
    }

    private IEnumerator ToggleObjectsWithDelay()
    {
        // Wait for the delay
        yield return new WaitForSeconds(delay);

        // Perform the toggle action
        foreach (GameObject obj in objectsToToggle)
        {
            if (obj != null)
            {
                if (setActiveOnClick)
                    obj.SetActive(true);
                else
                    obj.SetActive(false);
            }
        }
    }

    // Optional: toggle instead of just active/inactive
    public void ToggleObjects()
    {
        foreach (GameObject obj in objectsToToggle)
        {
            if (obj != null)
                obj.SetActive(!obj.activeSelf);
        }
    }
}
