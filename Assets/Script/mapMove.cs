using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapMove : MonoBehaviour {

    // Use this for initialization
    Transform tf;
	void Start () {
        tf = transform;
	}
	
	// Update is called once per frame
	void Update () {
        tf.position -= new Vector3(1,0,0)*Time.deltaTime*0.5f; 

    }
}
