using UnityEngine;
using UnityEngine.EventSystems;

public class HUDButtonTrigger : MonoBehaviour, IPointerDownHandler
{
    public PauseMenuController pauseController;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (pauseController != null)
        {
            pauseController.PauseGame();
        }
    }
}