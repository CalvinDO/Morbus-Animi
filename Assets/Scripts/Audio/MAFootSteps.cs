using UnityEngine;

public class MAFootSteps : MonoBehaviour {
    [SerializeField]
    private AudioClip[] walkClips;
    [SerializeField]
    private AudioClip[] jumpClips;
    [SerializeField]
    private AudioClip[] hangClips;
    [SerializeField]
    private AudioClip[] landClips;
    [SerializeField]
    private AudioClip[] liftClips;


    private AudioSource audioSource;
    private TerrainDetector terrainDetector;

    

    private void Awake() {
        this.audioSource = GetComponent<AudioSource>();
        //this.terrainDetector = new TerrainDetector();
    }

    private void Step() {

        AudioClip clip = GetRandomWalkClip();
        audioSource.PlayOneShot(clip);
    }

    private void Jump() {

        AudioClip clip = GetRandomJumpClip();
        audioSource.PlayOneShot(clip);
    }

    private AudioClip GetRandomWalkClip() {

        if (this.walkClips.Length <= 0) {
            Debug.LogWarning("No Stone Clips assigned!");
            return null;
        }



        return this.walkClips[UnityEngine.Random.Range(0, walkClips.Length)];
    }

    private AudioClip GetRandomJumpClip() {

        if (this.jumpClips.Length <= 0) {
            Debug.LogWarning("No Stone Clips assigned!");
            return null;
        }



        return this.jumpClips[UnityEngine.Random.Range(0, jumpClips.Length)];
    }

    private void Hang() {

        Debug.Log("hang!");
        AudioClip clip = GetRandomHangClip();
        audioSource.PlayOneShot(clip);
    }

    private AudioClip GetRandomHangClip() {

        if (this.hangClips.Length <= 0) {
            Debug.LogWarning("No Stone Clips assigned!");
            return null;
        }



        return this.hangClips[UnityEngine.Random.Range(0, hangClips.Length)];
    }
    private void Land() {

        AudioClip clip = GetRandomLandClip();
        audioSource.PlayOneShot(clip);
    }

    private AudioClip GetRandomLandClip() {

        if (this.landClips.Length <= 0) {
            Debug.LogWarning("No Stone Clips assigned!");
            return null;
        }



        return this.landClips[UnityEngine.Random.Range(0, landClips.Length)];
    }
    private void Lift() {

        Debug.Log("lift!");
        AudioClip clip = GetRandomLiftClip();
        audioSource.PlayOneShot(clip);
    }

    private AudioClip GetRandomLiftClip() {

        if (this.liftClips.Length <= 0) {
            Debug.LogWarning("No Stone Clips assigned!");
            return null;
        }



        return this.liftClips[UnityEngine.Random.Range(0, liftClips.Length)];
    }

    public void DieFinished() {
        MACharacterController characterController = this.transform.GetComponentInParent<MACharacterController>();
        characterController.DieFinished();
    }
}