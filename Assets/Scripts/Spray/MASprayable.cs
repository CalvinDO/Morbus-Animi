using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MASprayable : MonoBehaviour
{
    private MASprayCan sprayCan;
    GameObject sprayPrefab;
    private void Start()
    {
        this.sprayCan = GameObject.FindGameObjectWithTag("SprayCan").GetComponent<MASprayCan>();
    }
    public void Spray(Vector3 hitpoint, Quaternion rotation, Vector3 difference)
    {
        if (this.sprayCan.charges > 0)
        {
            //spawn spray
            sprayPrefab = new GameObject("spray");
            sprayPrefab.AddComponent<SpriteRenderer>();
            sprayPrefab.GetComponent<SpriteRenderer>().sprite = sprayCan.graffiti;
            sprayPrefab.transform.localScale -= new Vector3(1.5f, 1.5f, 1.5f);
            rotation *= Quaternion.Euler(0, -90, 0);
            float xMovement = Mathf.Abs(difference.x) * (0.01f / difference.x);
            //Debug.Log("x movement is" + xMovement);
            float zMovement = Mathf.Abs(difference.z) * (0.01f / difference.z);
            //Debug.Log("z movement is" + zMovement);
            hitpoint += new Vector3(xMovement, 0.01f, zMovement);
            GameObject spray = Instantiate(sprayPrefab, hitpoint, rotation) as GameObject;
            this.sprayCan.charges--;
        }
    }
}
