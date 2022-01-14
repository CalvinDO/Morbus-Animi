using UnityEngine;

public class MAFootSteps : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] stoneClips;

    private AudioSource audioSource;
    private TerrainDetector terrainDetector;

    private void Awake()
    {
        this.audioSource = GetComponent<AudioSource>();
        //this.terrainDetector = new TerrainDetector();
    }

    private void Step()
    {

        Debug.Log("step!");
        AudioClip clip = GetRandomClip();
        audioSource.PlayOneShot(clip);
    }

    private AudioClip GetRandomClip()
    {

        if (this.stoneClips.Length <= 0) {
            Debug.LogWarning("No Stone Clips assigned!");
            return null;
        }


        
        return this.stoneClips[UnityEngine.Random.Range(0, stoneClips.Length)];    
    }
}