/*

using UnityEngine;
using System.IO;
using System.Collections;
using AudioSynthesis;
using AudioSynthesis.Bank;
using AudioSynthesis.Synthesis;
using AudioSynthesis.Sequencer;
using AudioSynthesis.Midi;

namespace UnityMidi
{
	[RequireComponent (typeof(AudioSource))]
	[RequireComponent (typeof(AudioSource))]
	public class MidiPlayer : MonoBehaviour
	{
		[SerializeField] StreamingAssetResouce bankSource;
		[SerializeField] StreamingAssetResouce midiSource;
		[SerializeField] StreamingAssetResouce midiSource2;
		[SerializeField] bool loadOnAwake = true;
		[SerializeField] bool playOnAwake = true;
		[SerializeField] int channel = 1;
		[SerializeField] int sampleRate = 44100;
		[SerializeField] int bufferSize = 1024;
		[SerializeField] bool playTuggle = false;
		PatchBank bank;
		MidiFile midi;
		Synthesizer synthesizer;
		private AudioSource audioSource;
		private AudioSource audioSource2;

		MidiFileSequencer sequencer;
		MidiFileSequencer sequencer_bak;

		int bufferHead;
		float[] currentBuffer;

		//付け加えた機能
		bool playState = false;

		//false->sequencer true->sequencer_bak
		bool seqTogle = false;

		float timeCount = 0;
		float timeDuration = 0;
		bool isActiveTapTempo = false;
		float[] bpmarray;
		float totalbpm = 0;
		int bpmcount = 0;
		int bpm_accuracy = 15;
		bool firsttap = true;
		int bpm = 120;

		public int current_time;
		public int end_time;

		public float wakarantime;

		public AudioSource AudioSource { get { return audioSource; } }

		public AudioSource AudioSource2 { get { return audioSource2; } }

		public MidiFileSequencer Sequencer { get { return sequencer; } }

		public PatchBank Bank { get { return bank; } }

		public MidiFile MidiFile { get { return midi; } }

		public bool play_tuggle { get { return playTuggle; } }

		public void Update ()
		{
			if (playTuggle == true && playState == false) {
				Play ();
				playState = true;
			}
			if (playTuggle == false) {
				Stop ();
				playState = false;
			}


		}

		public void FixedUpdate ()
		{
			if (timeCount - timeDuration > 1.8) {
				clearTempo ();
			}
			if (isActiveTapTempo) {
				timeCount += 1.0f * Time.deltaTime;
			}

			if (seqTogle == true) {
				current_time = sequencer.CurrentTime;
			} else {
				current_time = sequencer_bak.CurrentTime;
			}


			if (end_time - current_time < 140 && playState == true) {
				Play ();
			}
			wakarantime += Time.deltaTime;
			if (wakarantime > 0.5) {
				Debug.Log ("!");
				wakarantime = 0;
			}

		}

		public void SwitchToggle ()
		{
			if (playTuggle)
				playTuggle = false;
			else
				playTuggle = true;
		}

		public void SwitchSeqToggle ()
		{
		
			if (seqTogle)
				seqTogle = false;
			else
				seqTogle = true;
			Debug.Log (seqTogle);
		}

		public void Awake ()
		{
			GameObject.DontDestroyOnLoad (this.gameObject);
			synthesizer = new Synthesizer (sampleRate, channel, bufferSize, 1);

			sequencer = new MidiFileSequencer (synthesizer);
			sequencer_bak = new MidiFileSequencer (synthesizer);

			AudioSource[] audioSources = GetComponents<AudioSource> ();
			audioSource = audioSources [0];
			audioSource2 = audioSources [1];

			current_time = 0;
			end_time = 0;
			seqTogle = false;

			//最初に再生されるmidiファイル
			//LoadMidi (new MidiFile (midiSource), 1);


			if (loadOnAwake) {
				LoadBank (new PatchBank (bankSource));
				//LoadMidi (new MidiFile (midiSource), 1);
			}

			if (playOnAwake) {
//				Play ();
				playState = true;
			}

		}

		public void LoadBank (PatchBank bank)
		{
			this.bank = bank;
			synthesizer.UnloadBank ();
			synthesizer.LoadBank (bank);
		}

		public void LoadMidi (MidiFile midi, int number)
		{
			this.midi = midi;
			if (seqTogle == false) {
				sequencer.Stop ();
				sequencer.UnloadMidi ();
				sequencer.LoadMidi (midi, bpm);
			}
			if (seqTogle == true) {
				sequencer_bak.Stop ();
				sequencer_bak.UnloadMidi ();
				sequencer_bak.LoadMidi (midi, bpm);
			}
			Debug.Log ("midi loaded");


		}

		public void Play ()
		{
			
			if (seqTogle == false) {
				LoadMidi (new MidiFile (midiSource), 1);
				end_time = sequencer.EndTime;
				sequencer.Play ();
				Debug.Log ("seq1 play");
				audioSource.Play ();
			} else {
				LoadMidi (new MidiFile (midiSource2), 2);
				end_time = sequencer_bak.EndTime;
				sequencer_bak.Play ();
				Debug.Log ("seq2 play");
				audioSource2.Play ();
			}

			SwitchSeqToggle ();

//			audioSource.Play ();
//			audioSource2.Play ();

		}

		public void Stop ()
		{
			sequencer.Stop ();
			sequencer_bak.Stop ();
			audioSource.Stop ();
			audioSource2.Stop ();

			Awake ();
		}
		//テンポを計測する機能
		public void tapTempo ()
		{
			if (firsttap) {
				isActiveTapTempo = true;
				timeDuration = timeCount;
				bpmarray = new float[bpm_accuracy];
				firsttap = false;
			} else {
				//前のタップ時刻 - 今のタップ時刻 = タップ間の時間
				timeDuration = timeCount - timeDuration;
				//秒からBPMに直して代入
				bpmarray [bpmcount] = 1 / timeDuration * 60;
				//今までのbpmを足す
				for (int i = 0; i < bpmarray.Length; i++) {
					totalbpm += bpmarray [i];
				}
				//bpmの平均値を出力
				if (bpmarray [bpmarray.Length - 1] != 0) {
					//  Debug.Log(totalbpm/bpmarray.Length);
					//  bpm = (int)totalbpm/bpmarray.Length;
					bpm = Mathf.RoundToInt (totalbpm / bpmarray.Length);
				} else {
					//  Debug.Log(totalbpm/(bpmcount+1));
					//  bpm = (int)totalbpm/(bpmcount+1);
					bpm = Mathf.RoundToInt (totalbpm / (bpmcount + 1));
				}
				Debug.Log (bpm);
				timeDuration = timeCount;
				if (bpmcount < bpmarray.Length)
					bpmcount++;
				if (bpmcount == bpmarray.Length)
					bpmcount = 0;
				totalbpm = 0;
			}
		}

		public void clearTempo ()
		{
			bpmarray = new float[bpm_accuracy];
			isActiveTapTempo = false;
			firsttap = true;
			bpmcount = 0;
			totalbpm = 0;
			timeDuration = 0;
			timeCount = 0;
		}

		public void showState ()
		{
			Debug.Log (sequencer.IsPlaying);
			Debug.Log (sequencer.EndTime);
			Debug.Log (playState);
			Debug.Log (playTuggle);
		}

		void OnAudioFilterRead (float[] data, int channel)
		{
			Debug.Assert (this.channel == channel);
			int count = 0;
			while (count < data.Length) {
				
				if (currentBuffer == null || bufferHead >= currentBuffer.Length) {
					if (seqTogle == true)
						sequencer.FillMidiEventQueue ();
					if (seqTogle == false)
						sequencer_bak.FillMidiEventQueue ();
						
					synthesizer.GetNext ();
					currentBuffer = synthesizer.WorkingBuffer;
					bufferHead = 0;

				}
				var length = Mathf.Min (currentBuffer.Length - bufferHead, data.Length - count);
				System.Array.Copy (currentBuffer, bufferHead, data, count, length);
				bufferHead += length;
				count += length;
				


			}
		}

	}
}

*/