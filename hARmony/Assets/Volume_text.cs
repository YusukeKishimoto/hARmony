using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Volume_text : MonoBehaviour {
	float volume;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//volume = ;
		this.GetComponent<Text>().text = "Volume : ";
	}

}
