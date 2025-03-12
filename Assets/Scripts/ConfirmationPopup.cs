using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class ConfirmationPopup : MonoBehaviour
{
    public Animator confirmationPopupAnimator;
    public GameObject backgroundBlocker;
    public Image fadePanel;
    public float animationDuration = 0.2f;
    public float fadeDuration = 1.5f;

    private bool isAnimating = false;

    void Start()
    {
        backgroundBlocker.SetActive(false);
        fadePanel.gameObject.SetActive(false);
    }

    public void OpenPopup()
    {
        if (!isAnimating)
        {
            confirmationPopupAnimator.SetBool("IsOpen", true);
            backgroundBlocker.SetActive(true);
        }
    }

    public void ClosePopup()
    {
        if (!isAnimating)
        {
            backgroundBlocker.SetActive(false);
            StartCoroutine(ClosePopupRoutine());
        }
    }

    IEnumerator ClosePopupRoutine()
    {
        isAnimating = true;
        confirmationPopupAnimator.SetBool("IsOpen", false);

        yield return new WaitForSeconds(animationDuration);

        isAnimating = false;
    }

    public void OnYesButtonClicked()
    {
        if (!isAnimating)
        {
            StartCoroutine(FadeOutAndQuit());
        }
    }

    IEnumerator FadeOutAndQuit()
    {
        isAnimating = true;

        fadePanel.gameObject.SetActive(true);
        fadePanel.color = new Color(0, 0, 0, 0);

        // Fade to black
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadePanel.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadePanel.color = new Color(0, 0, 0, 1);

        yield return new WaitForSeconds(0.5f);

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}