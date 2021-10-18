using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MALeverInteraction : MAInteractable
{
    public int leverIndex;
    public PuzzleManager puzzleManager;
    public bool isLast;
    public GameObject door;
    public bool isFlipped;

    public AudioClip correctSound;

    public AudioSource leverSwitchSound;


    public override void MAInteract()
    {
        if (this.puzzleManager == null) {
            return;
        }
        base.MAInteract();
        if (!isFlipped)
        {
            if (currentSelection == interactType.lever)
            {
                FlipLever();
                checkIfSolved();
            }
        }
    }

    void FlipLever()
    {

        Debug.Log(leverIndex);
        Debug.Log(this.puzzleManager.counter);

        this.leverSwitchSound.PlayOneShot(this.leverSwitchSound.clip);

        if (leverIndex == this.puzzleManager.counter)
        {
            this.puzzleManager.counter++;
            Debug.Log("flipped switch number " + this.puzzleManager.counter);
            isFlipped = true;
        }
        else
        {
            Debug.Log("this lever doesn't do anything yet");
        }
        textDisplay.SetActive(false);
    }

    void checkIfSolved()
    {
        if (isLast && (leverIndex + 1) == this.puzzleManager.counter)
        {

            Debug.Log("the door opened!");
            this.leverSwitchSound.PlayOneShot(this.correctSound);
            //Destroy(this.door);
            this.door.transform.Rotate(new Vector3(0, 0, 90));
        }
    }
}
