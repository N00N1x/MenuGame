using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHighlight : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Animator buttonAnimator;

    public void OnSelect(BaseEventData eventData)
    {
      //  HighlightButton();
    }

    public void OnDeselect(BaseEventData eventData)
    {
       // ResetButton();
    }

    public void HighlightButton()
    {
        if (buttonAnimator != null)
        {
            buttonAnimator.Play("Highlighted");
        }
    }

    void ResetButton()
    {
        if (buttonAnimator != null)
        {
            buttonAnimator.Play("Normal");
        }
    }
}