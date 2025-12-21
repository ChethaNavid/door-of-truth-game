using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneSwitcher : MonoBehaviour
{
    public void ChangeSceneWithDelay(int index)
    {
        StartCoroutine(LoadAfterDelay(index));
    }

    IEnumerator LoadAfterDelay(int index)
    {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene(index);
    }
}
