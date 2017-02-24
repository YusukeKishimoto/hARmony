using UnityEngine;
using System.Collections;

public class AR_BOX : MonoBehaviour
{

	public GameObject cube;

	public string Chord;

	// Use this for initialization
	void Start ()
	{
		Chord = "C";
	}

	// Update is called once per frame
	void Update ()
	{
	}
	// 衝突している間呼ばれ続ける
	void OnTriggerStay (Collider collider)
	{
		
	}

	// 接触した時に呼ばれる
	void OnTriggerEnter (Collider collider)
	{
		//print ("Attacked");
		cube.GetComponent<Renderer> ().material.color = new Color32 (32, 32, 32, 255);
	}

	// 離れた時に呼ばれる
	void OnTriggerExit (Collider collider)
	{
		cube.GetComponent<Renderer> ().material.color = new Color32 (255, 255, 255, 255);
		//print ("Removed");
	}
}
