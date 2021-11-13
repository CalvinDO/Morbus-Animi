using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAClimbInteraction : MAInteractable
{
    public GameObject destination;
    public float duration;
    public float finishDuration;
    public enum ClimbLocation { top, bottom};
    public ClimbLocation currentLocation;
    GameObject player;
    public override void MAInteract()
    {
        base.MAInteract();
        if (currentSelection == objectType.climb)
        {
            Climb();
        }
    }

    IEnumerator LerpPositionTopToBottom(Vector3 startPosition, Vector3 ladderTop, Vector3 targetPosition,float duration, float finishDuration)
    {
        float time = 0;

        player.transform.position = startPosition;

        while (time < finishDuration)
        {
            player.transform.position = Vector3.Lerp(startPosition, ladderTop, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        player.transform.position = ladderTop;

        time = 0;

        while (time < duration)
        {
            player.transform.position = Vector3.Lerp(ladderTop, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        time = 0;

        player.transform.position = targetPosition;

        while (time < finishDuration)
        {
            player.transform.position = Vector3.Lerp(targetPosition, targetPosition + (this.transform.forward * 0.4f), time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        player.transform.position = targetPosition + (this.transform.forward * 0.4f);
    }

    IEnumerator LerpPositionBottomToTop(Vector3 startPosition, Vector3 ladderTop, Vector3 targetPosition, float duration, float finishDuration)
    {
        float time = 0;

        player.transform.position = startPosition;

        while (time < duration)
        {
            player.transform.position = Vector3.Lerp(startPosition, ladderTop, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        player.transform.position = ladderTop;

        time = 0;

        while (time < finishDuration)
        {
            player.transform.position = Vector3.Lerp(ladderTop, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        player.transform.position = targetPosition;
    }

    private void Climb()
    {
        player = GameObject.Find("SmallNorah");
        if (currentLocation == ClimbLocation.bottom)
        {
            StartCoroutine(LerpPositionBottomToTop(player.transform.position, destination.transform.position, (destination.transform.position + (this.transform.forward * 0.4f)), duration, finishDuration));
        }
        else StartCoroutine(LerpPositionTopToBottom(player.transform.position, (player.transform.position + (this.transform.forward * 0.4f)), destination.transform.position, duration, finishDuration));
    }
}
