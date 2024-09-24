using UnityEngine;
using UnityEngine.EventSystems;

public class DropTar : MonoBehaviour, IDropHandler
{
    public string expectedWord;  // The word this drop target expects
    public string currentDroppedWord;  // The word currently dropped onto this target

    public void OnDrop(PointerEventData eventData)
    {
        DragObj draggedItem = eventData.pointerDrag.GetComponent<DragObj>();

        if (draggedItem != null)
        {
            // Snap the dragged item to the drop target's position
            RectTransform draggedRectTransform = draggedItem.GetComponent<RectTransform>();
            RectTransform dropTargetRectTransform = GetComponent<RectTransform>();

            // Set the dragged item's position to the drop target's position
            draggedRectTransform.anchoredPosition = dropTargetRectTransform.anchoredPosition;

            // Store the dropped word in the drop target
            currentDroppedWord = draggedItem.word;

            // Check if the dropped word matches the expected word
            if (currentDroppedWord.Trim().ToLower() == expectedWord.Trim().ToLower())
            {
                // Disable the dragged item so it can't be dragged again
                draggedItem.gameObject.SetActive(false);
                Debug.Log("Word correctly placed: " + currentDroppedWord);
            }
            else
            {
                Debug.Log("Incorrect placement, try again.");
                // Optionally: you can reset the dragged item position or perform other actions
            }
        }
    }

    private void CheckSentenceCorrectness()
    {
        // Get all the drop targets in the scene
        DropTar[] dropTargets = FindObjectsOfType<DropTar>();
        bool isCorrect = true;

        // Check if the dropped words match the expected words
        foreach (DropTar dropTarget in dropTargets)
        {
            // Print the current dropped word and expected word for debugging
            Debug.Log($"Drop target: dropped word = {dropTarget.currentDroppedWord}, expected = {dropTarget.expectedWord}");

            if (dropTarget.currentDroppedWord == null ||
                dropTarget.currentDroppedWord.Trim().ToLower() != dropTarget.expectedWord.Trim().ToLower())
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            Debug.Log("Correct Sentence!");
            // Show success message or proceed to the next level
        }
        else
        {
            Debug.Log("Incorrect Sentence, try again.");
        }
    }
}
