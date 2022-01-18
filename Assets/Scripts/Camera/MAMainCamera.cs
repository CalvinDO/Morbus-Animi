using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAMainCamera : MonoBehaviour {
    private MACharacterController characterController;

    private bool isColliding = false;


    private Vector3 cameraOffset;

    [Range(0, 1)]
    public float transparencyStartDistance;
    [Range(0, 1)]
    public float transparencyFullDistance;

    [Range(0, 1)]
    public float transparency;

    [Range(0, 10)]
    public float collisionLerpFactor;


    void Start() {
        this.characterController = this.transform.root.GetComponent<MACharacterController>();


        this.cameraOffset = this.characterController.thirdPCameraGoal.localPosition;
    }


    void Update() {


        LayerMask mask = LayerMask.GetMask("Default", "Water", "MA_NavMesh", "Wall", "MA_Entity", "MA_OrderedButton", "MA_Throwable", "LayerMask", "SeeThrough");
        RaycastHit hit;


        Vector3 lineCastStart = this.characterController.goalRotator.transform.position;
        Vector3 lineCastEnd = this.characterController.goalRotator.transform.position + this.characterController.goalRotator.transform.rotation * this.cameraOffset;

        if (Physics.Linecast(lineCastStart, lineCastEnd, out hit, mask)) {
            this.characterController.thirdPCameraGoal.localPosition = new Vector3(0, 0, -Vector3.Distance(this.characterController.goalRotator.transform.position, hit.point));
        }
        else {
            this.characterController.thirdPCameraGoal.localPosition = Vector3.Lerp(this.characterController.thirdPCameraGoal.localPosition, this.cameraOffset, this.collisionLerpFactor *  Time.deltaTime);
        }


        float currentDistance = Vector3.Distance(this.characterController.thirdPCameraGoal.position, lineCastStart);
        float currentOppacity = (currentDistance - this.transparencyFullDistance) / (this.transparencyStartDistance - this.transparencyFullDistance);

        


        if (currentDistance < this.transparencyStartDistance) {

            this.characterController.skinnedMeshRenderer.enabled = false;
           
            int materialsLength = this.characterController.skinnedMeshRenderer.materials.Length;

            Material[] materials = new Material[materialsLength];

            for (int index = 0; index < materialsLength; index++) {

                Material currentMaterial = this.characterController.skinnedMeshRenderer.materials[index];
                Color color = currentMaterial.color;
                color.a = currentOppacity;
                currentMaterial.color = color;

                materials[index] = currentMaterial;
            }

            this.characterController.skinnedMeshRenderer.materials = materials;
        }
        else {
            this.characterController.skinnedMeshRenderer.enabled = true;
        }
    }



    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

}
