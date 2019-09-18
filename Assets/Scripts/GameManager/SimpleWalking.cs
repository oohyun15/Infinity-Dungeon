using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleWalking : MonoBehaviour {

	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.right * Time.deltaTime * 2.0f, Camera.main.transform);
	}
}
