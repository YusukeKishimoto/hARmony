using UnityEngine;
using System.IO;
using System.Collections;
using AudioSynthesis;
using AudioSynthesis.Bank;
using AudioSynthesis.Synthesis;
using AudioSynthesis.Sequencer;
using AudioSynthesis.Midi;
using UnityTempo;

namespace UnityMidi
{
	[RequireComponent (typeof(AudioSource))]
	public class MidiPlayer_bak : MonoBehaviour
	{
		public TempoManager tempoManager;

		[SerializeField] StreamingAssetResouce bankSource;
		[SerializeField] StreamingAssetResouce midiSource;
		[SerializeField] bool loadOnAwake = true;
		[SerializeField] bool playOnAwake = true;
		[SerializeField] int channel = 2;
		[SerializeField] int sampleRate = 44100;
		[SerializeField] int bufferSize = 1024;
		PatchBank bank;
		MidiFile midi;
		Synthesizer synthesizer;
		AudioSource audioSource;
		MidiFileSequencer sequencer;
		int bufferHead;
		float[] currentBuffer;
		public int bpm = 120;


		public AudioSource AudioSource { get { return audioSource; } }

		public MidiFileSequencer Sequencer { get { return sequencer; } }

		public PatchBank Bank { get { return bank; } }

		public MidiFile MidiFile { get { return midi; } }

		public void Awake ()
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer) {
				sampleRate = 24000;
			}

			synthesizer = new Synthesizer (sampleRate, channel, bufferSize, 1);
			sequencer = new MidiFileSequencer (synthesizer);
			audioSource = GetComponent<AudioSource> ();

			if (loadOnAwake) {
				LoadBank (new PatchBank (bankSource));
				//LoadMidi (new MidiFile (midiSource));
			}

			if (playOnAwake) {
				//Play ();
			}
		}

		public void LoadBank (PatchBank bank)
		{
			this.bank = bank;
			synthesizer.UnloadBank ();
			synthesizer.LoadBank (bank);
		}

		public void LoadMidi (MidiFile midi)
		{
			Debug.Log ("Loaded");
			this.midi = midi;
			sequencer.Stop ();
			sequencer.UnloadMidi ();
			sequencer.LoadMidi (midi, bpm);
		}

		public void Play ()
		{
			//LoadMidi (new MidiFile (midiSource));
			sequencer.Play ();
			audioSource.Play ();
			Debug.Log ("seq1 play");
		}

		public void Stop ()
		{
			sequencer.Stop ();
			audioSource.Stop ();
			Awake ();
		}

		void OnAudioFilterRead (float[] data, int channel)
		{
			Debug.Assert (this.channel == channel);
			int count = 0;
			while (count < data.Length) {
				if (currentBuffer == null || bufferHead >= currentBuffer.Length) {
					sequencer.FillMidiEventQueue ();
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