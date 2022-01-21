using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flickeringlight : MonoBehaviour
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
        Timer = Random.Range(MinTime, MaxTime);
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

        if(Timer > 0)
        Timer -= Time.deltaTime;

        if(Timer<=0)
        {
            _Light.enabled = !_Light.enabled;
            Timer = Random.Range(MinTime, MaxTime);
            AS.PlayOneShot(LightAudio);
        }
    }

    public void TurnOff() {
        this._Light.enabled = false;
        this.isFlickering = false;
    }
}
