using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MAElevatorInteraction : MAInteractable
{
    public GameObject elevatorDoor;
    public Transform openCoordinates;
    public Transform closedCoordinates;
    float duration = 2f;
    public override void MAInteract()
    {
        base.MAInteract();
        StartCoroutine(ChangeFloor(openCoordinates.localPosition, closedCoordinates.localPosition, duration));
    }

    IEnumerator ChangeFloor(Vector3 openPosition, Vector3 closedPosition, float duration)
    {
        float time = 0;

        elevatorDoor.transform.localPosition = openPosition;

        while (time < duration)
        {
            elevatorDoor.transform.localPosition = Vector3.Lerp(elevatorDoor.transform.localPosition, closedPosition, time / duration);
            time += Time.deltaTime;
        }
        elevatorDoor.transform.localPosition = closedPosition;
        yield return new WaitForSeconds(duration);

        SceneManager.LoadScene("SceneTransitionTest");

        time = 0;

        elevatorDoor.transform.position = closedPosition;

        while (time < duration)
        {
            elevatorDoor.transform.localPosition = Vector3.Lerp(elevatorDoor.transform.localPosition, openPosition, time / duration);
            time += Time.deltaTime;
        }
        elevatorDoor.transform.localPosition = openPosition;
    }
}
