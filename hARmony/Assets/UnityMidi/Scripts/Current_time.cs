using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityMidi;

public class Current_time : MonoBehaviour
{
	public GameObject midi_p;
	public MidiPlayer midiplayer;
	int cur_time;
	// Use this for initialization
	void Start ()
	{
//		midiplayer = GetComponent<UnityMidi> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
//		cur_time = midi_p.GetComponent<Midiplayer> ().current_time;
//		cur_time = midiplayer.current_time;
		this.GetComponent<Text> ().text = "CurrentTime : " + cur_time;

	}
}
