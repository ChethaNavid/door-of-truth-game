using UnityEngine;

public class ClearInstructionsButton : MonoBehaviour
{
    public void ClearInstructionFlag()
    {
        PlayerPrefs.DeleteKey("HasShownInstructions");
        PlayerPrefs.Save();

        Debug.Log("Instruction flag cleared. Instructions will show again next time.");
    }
}
