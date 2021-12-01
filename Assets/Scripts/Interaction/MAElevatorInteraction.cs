using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MAElevatorInteraction : MAInteractable {
    public GameObject elevatorDoor;
    public Transform openCoordinates;
    public Transform closedCoordinates;
    float duration = 2f;

    public int sceneIndex;

    public override void MAInteract() {
        base.MAInteract();
        StartCoroutine(ChangeFloor(openCoordinates.localPosition, closedCoordinates.localPosition, duration));
    }

    IEnumerator ChangeFloor(Vector3 openPosition, Vector3 closedPosition, float duration) {
        float time = 0;

        elevatorDoor.transform.localPosition = closedPosition;

        SceneManager.LoadScene(this.sceneIndex);

        while (time < duration) {
            elevatorDoor.transform.localPosition = Vector3.Lerp(elevatorDoor.transform.localPosition, openPosition, time / duration);
            time += Time.deltaTime;
        }
        elevatorDoor.transform.localPosition = openPosition;
        yield return new WaitForSeconds(duration);
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("MainCollider")) {

            if (Input.GetKey(KeyCode.E)) {

                base.MAInteract();
                StartCoroutine(ChangeFloor(openCoordinates.localPosition, closedCoordinates.localPosition, duration));
            }
        }
    }
}
