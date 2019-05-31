using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    public float speed;
    public float angle_speed;
    // Use this for initialization
    void Start()
    {

        speed = 1f;
        angle_speed = 100f;
    }
    // Update is called once per frame
    void Update () {
        transform.Translate(Vector3.forward * speed * Input.GetAxisRaw("Horizontal") * Time.deltaTime);
        transform.Translate(Vector3.left * speed * Input.GetAxisRaw("Vertical") * Time.deltaTime);
        transform.Rotate(Vector3.up, Input.GetAxisRaw("Horizontal") * angle_speed * Time.deltaTime);
    }
}
