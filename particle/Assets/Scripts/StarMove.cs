using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMove : MonoBehaviour {

    private float revolutionSpeed;
    private float rotateSpeed;

	// Use this for initialization
	void Start () {
        rotateSpeed = 10f;
    }
	
	// Update is called once per frame
	void Update () {
		this.gameObject.transform.Rotate(new Vector3(1, 2, 0), rotateSpeed * Time.deltaTime);
    }
}
