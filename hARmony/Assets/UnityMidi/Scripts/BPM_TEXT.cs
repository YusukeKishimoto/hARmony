using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityTempo;

public class BPM_TEXT : MonoBehaviour
{

	public TempoManager tmp;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		this.GetComponent<Text> ().text = "" + tmp.bpm;
	}
}
