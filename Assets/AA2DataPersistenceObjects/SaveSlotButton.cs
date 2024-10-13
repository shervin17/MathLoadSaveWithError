using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SaveSlotButton : MonoBehaviour 
{

    private Button button;

    void Awake()
    {
        // Get the Button component attached to this GameObject
        button = GetComponent<Button>();
    }

    public void SetupButton(UnityAction action)
    {
        button.onClick.AddListener(action);
    }
}
