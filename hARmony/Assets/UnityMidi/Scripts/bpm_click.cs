using UnityEngine;
using System.Collections;
using AudioSynthesis.Midi;
using UnityMidi;


public class bpm_click : MonoBehaviour
{

	//[SerializeField] StreamingAssetResouce midiSource;
	//[SerializeField] StreamingAssetResouce midiSource2;
	//	string path = "ExampleMidis/F_Maj_pt1.mid";

	StreamingAssetResouce midiSource;

	//StreamingAssetResouce midiSource3 = new StreamingAssetResouce ("ExampleMidis/F_Maj.mid");
	//StreamingAssetResouce midiSource4 = new StreamingAssetResouce ("ExampleMidis/F_Maj_pt1.mid");

	//C_Maj.mid
	//C-Maj_C-Maj7.mid
	string midiBasePath = "ExampleMidis/";
	string[] midiPath = new string[10];

	AudioClip audioClip1;
	AudioSource source;

	bool firstPlay = false;
	int count = 0;

	public MidiPlayer midi_p;
	public MidiPlayer_bak midi_pb;

	public ChordManager chordmanager;

	float wakarantime;
	public double dsptime;

	//MidiFile midifile;
	// Use this for initialization

	bool playTuggle = false;

	void Awake ()
	{
		
		SetMidiPath ();
	}

	void Start ()
	{

	}

	void SetMidiPath ()
	{
		midiPath [0] = "C_Maj.mid";
		midiPath [1] = "C-Maj_C-Maj7.mid";
		midiPath [2] = "F_Maj.mid";
		midiPath [3] = "F_Maj_pt1.mid";
	}

	int midi_count = 0;
	bool TargetFound = false;

	void LoadMidiPath ()
	{	
		string path = midiBasePath;
		//string playingchordname = chordmanager.getPlayChord ();
		if (chordmanager.getPlayChord () == "") {
			TargetFound = false;
		} else {
			TargetFound = true;
			//取ってきたコードの名前でどのmidiデータを読み取るか判断
			switch (chordmanager.getPlayChord ()) {
			case "CM":
				path += midiPath [0];
				break;
			case "FM":
				path += midiPath [2];
				break;
			default:
				path += midiPath [0];
				break;
			}
			midiSource = new StreamingAssetResouce (path);

		}
		/*
		string path = midiBasePath + midiPath [midi_count];
		midiSource = new StreamingAssetResouce (path);

		if (midiPath [midi_count + 1] != null) {
			midi_count++;
		} else {
			midi_count = 0;
		}
		*/		

	}

	public void SwitchPlayToggle ()
	{
		if (playTuggle) {
			playTuggle = false;
			wakarantime = 0;
			midi_count = 0;
			count = 0;
			midi_p.Stop ();
			midi_pb.Stop ();

		} else {
			//LoadMidiPath ();
			//if (TargetFound == true) {
			//midi_p.LoadMidi (new MidiFile (midiSource));
			//LoadMidiPath ();
			//midi_pb.LoadMidi (new MidiFile (midiSource));
			playTuggle = true;
			firstPlay = true;
			//}
		}
	}

	void FixedUpdate ()
	{
		//dsptime = AudioSettings.dspTime;

		if (playTuggle == true && TargetFound == false) {
			LoadMidiPath ();
		}

		if (playTuggle == true && TargetFound == true) {
			if (firstPlay) {
				//LoadMidiPath ();
				midi_p.LoadMidi (new MidiFile (midiSource));
				dsptime = AudioSettings.dspTime;
				midi_p.Play ();
				firstPlay = false;
			}

			if (midi_p.Sequencer.IsPlaying == true && midi_p.Sequencer.EndTime - midi_p.Sequencer.CurrentTime < 1500) {
				LoadMidiPath ();
				midi_pb.LoadMidi (new MidiFile (midiSource));
			}
			if (midi_pb.Sequencer.IsPlaying == true && midi_pb.Sequencer.EndTime - midi_pb.Sequencer.CurrentTime < 1500) {
				LoadMidiPath ();
				midi_p.LoadMidi (new MidiFile (midiSource));
			}

			wakarantime += Time.fixedDeltaTime;
			//if (wakarantime > 0.49 * 4) {
			//if (wakarantime > (midi_p.bpm * 0.00412 * 4)) {
			//if (wakarantime >= ((1 / (((float)midi_p.bpm / 60)) - 0.015) * 4)) {	
			if ((AudioSettings.dspTime - dsptime) >= ((1 / (((float)midi_p.bpm / 60)) - 0.015) * 4)) {	
					
				//Debug.Log ("calc : " + ((1 / ((float)midi_p.bpm / 60)) - 0.005));
				if (count == 0) {
					midi_pb.Play ();
					count++;
					//LoadMidiPath ();
					//midi_p.LoadMidi (new MidiFile (midiSource));

				} else {
					midi_p.Play ();
					count--;
					//LoadMidiPath ();
					//midi_pb.LoadMidi (new MidiFile (midiSource));
				}
				wakarantime = 0;
				dsptime = AudioSettings.dspTime;
			}
		}

	}
}
