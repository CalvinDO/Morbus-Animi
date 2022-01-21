using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAFlickeringlight : MonoBehaviour
{

    public Light _Light;

    public float MinTime;
    public float MaxTime;
    public float Timer;

    public AudioSource AS;
    public AudioClip LightAudio;

    private bool isFlickering = true;


    // Start is called before the first frame update
    void Start()
    {
        this.Timer = Random.Range(this.MinTime, this.MaxTime);
    }

    // Update is called once per frame
    void Update()
    {
        FlickerLight();
    }

    void FlickerLight()
    {

        if (!this.isFlickering) {
            return;
        }

        if(this.Timer > 0)
        this.Timer -= Time.deltaTime;

        if(this.Timer<=0)
        {
            this._Light.enabled = !this._Light.enabled;
            this.Timer = Random.Range(this.MinTime, this.MaxTime);
            this.AS.PlayOneShot(LightAudio);
        }
    }

    public void TurnOff() {
        this._Light.enabled = false;
        this.isFlickering = false;
    }
}
