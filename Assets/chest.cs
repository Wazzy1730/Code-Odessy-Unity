using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    private Animator chestAnimator;
    private bool isOpen = false;

    private void Start()
    {
        chestAnimator = GetComponent<Animator>(); // Get reference to Animator component
    }

    public void Interact()
    {
        if (!isOpen)
        {
            chestAnimator.Play("openChest"); // Play open chest animation
            isOpen = true;
        }
    }

    public void StopInteract()
    {
        if (isOpen)
        {
            chestAnimator.Play("closeChest"); // Play close chest animation
            isOpen = false;
        }
    }
}
