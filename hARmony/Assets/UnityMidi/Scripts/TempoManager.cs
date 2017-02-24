using UnityEngine;
using System.Collections;
using UnityMidi;

namespace UnityTempo
{
	public class TempoManager : MonoBehaviour
	{
		public MidiPlayer midiplayer;
		public MidiPlayer_bak midiplayer_bak;

		float timeCount = 0;
		float timeDuration = 0;
		bool isActiveTapTempo = false;
		float[] bpmarray;
		float totalbpm = 0;
		int bpmcount = 0;
		int bpm_accuracy = 15;
		bool firsttap = true;
		public int bpm = 120;

		// Use this for initialization
		void Start ()
		{
			bpm = 120;
		}
	
		// Update is called once per frame
		void Update ()
		{
		}

		public void FixedUpdate ()
		{
			if (timeCount - timeDuration > 1.8) {
				clearTempo ();
			}
			if (isActiveTapTempo) {
				timeCount += 1.0f * Time.deltaTime;
			}


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
				//Debug.Log (bpm);
				midiplayer.bpm = bpm;
				midiplayer_bak.bpm = bpm;

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

		public void resetTempo ()
		{
			bpm = 120;
			midiplayer.bpm = bpm;
			midiplayer_bak.bpm = bpm;
		}
	}
}
