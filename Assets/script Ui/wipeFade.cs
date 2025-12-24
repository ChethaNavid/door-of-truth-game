using UnityEngine;

public class FadeController : MonoBehaviour
{
    public Animator animator;

    public void PlayFade()
    {
        animator.Play("MoveUpAnimation");
    }
}
