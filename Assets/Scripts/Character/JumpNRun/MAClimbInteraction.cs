using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAClimbInteraction : MAInteractable
{
    public GameObject destination;
    public Vector3 forwardAtTop;
    public float duration;
    public float finishDuration;
    GameObject player;
    public override void MAInteract()
    {
        base.MAInteract();
        if (currentSelection == interactType.climb)
        {
            Climb();
        }
    }

    IEnumerator LerpPosition(Vector3 startPosition, Vector3 ladderTopPosition, Vector3 targetPosition, float duration, float finishDuration)
    {
        float time = 0;

        player.transform.position = startPosition;

        while (time < duration)
        {
            player.transform.position = Vector3.Lerp(startPosition, ladderTopPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        player.transform.position = ladderTopPosition;

        time = 0;

        while (time < finishDuration)
        {
            player.transform.position = Vector3.Lerp(ladderTopPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        player.transform.position = targetPosition;
    }

    private void Climb()
    {
        player = GameObject.Find("ThirdPersonPlayer");
        StartCoroutine(LerpPosition(player.transform.position, destination.transform.position, (destination.transform.position + forwardAtTop), duration, finishDuration));
    }
}
