using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class OptionsMenu : MonoBehaviour
{
    public Animator optionsMenuAnimator;
    public float animationDuration = 0.2f;

    private bool isAnimating = false;

    public void OpenOptionsMenu()
    {
        if (!isAnimating)
        {
            optionsMenuAnimator.SetBool("IsOpen", true);
        }
    }

    public void CloseOptionsMenu()
    {
        if (!isAnimating)
        {
            StartCoroutine(CloseOptionsMenuRoutine());
        }
    }

    IEnumerator CloseOptionsMenuRoutine()
    {
        isAnimating = true;
        optionsMenuAnimator.SetBool("IsOpen", false);

        yield return new WaitForSeconds(animationDuration);

        isAnimating = false;
    }
}