using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAPressurePlate : MonoBehaviour
{
    public int counter;
    public GameObject test;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MAPushable>())
        {
            counter++;
            //Debug.Log(counter);
            // call whatever flo wants to do with this mechanic
            test.transform.position += new Vector3(1,0,0);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<MAPushable>())
        {
            counter--;
            //Debug.Log(counter);
            if (counter == 0)
            {
                test.transform.position -= new Vector3(1, 0, 0);
            }
        }
    }
}
