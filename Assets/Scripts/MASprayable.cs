using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MASprayable : MonoBehaviour
{
    private MASprayCan sprayCan;
    private void Start()
    {
        sprayCan = GameObject.FindGameObjectWithTag("SprayCan").GetComponent<MASprayCan>();
    }
    // Start is called before the first frame update
    public virtual void Spray()
    {
        Debug.Log(sprayCan.name);
        Debug.Log(sprayCan.graffiti.name);
    }
}
