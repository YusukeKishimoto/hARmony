using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using System.Collections;

public class ChordManager : MonoBehaviour
{
	private GameObject[] ArCanvas;
	private GameObject[] Targets;

	private click_UI chordTexts;
	private TrackableBehaviour tracker;

	//ChordAmount と PlayChord のlengthは一緒になるはず
	//PlayChordに string でコード名を順番に格納する
	private int[] ChordAmount;
	private string[] PlayChord;

	public Text UI_TEXT;

	//ここでmidiをよみこむ順番なども制御する？

	//コードはあらかじめ認識させてから再生しなさいよ
	//再生中にコードを変更は出来ないけど順番は入れ替えることが出来る

	//全部のコードの中で何個目かをここて覚えておく必要があるのかな？

	// Use this for initialization
	void Start ()
	{
		setChordAmount ();
		//GameObject[] targets = gameObject.get
		ArCanvas = GameObject.FindGameObjectsWithTag ("AR_Canvas");
		Targets = GameObject.FindGameObjectsWithTag ("Target");
		chordTexts = ArCanvas [0].GetComponent<click_UI> ();
		tracker = Targets [0].GetComponent<TrackableBehaviour> ();

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (tracker.CurrentStatus == TrackableBehaviour.Status.TRACKED ||
		    tracker.CurrentStatus == TrackableBehaviour.Status.TRACKED ||
		    tracker.CurrentStatus == TrackableBehaviour.Status.EXTENDED_TRACKED) {
			UI_TEXT.text = chordTexts.getChordText ();
			PlayChord [0] = chordTexts.getChordText ();
		} else {
			if (UI_TEXT.text != "") {
				UI_TEXT.text = "";
				PlayChord [0] = "";
			}
		}
	}

	//再生しなきゃいけないコードの量を設定
	private void setChordAmount ()
	{
		//最初から上限値をきめてしまえ
		ChordAmount = new int[32];
		PlayChord = new string[32];
	}

	//再生しなきゃいけないコードを返す
	public string getPlayChord ()
	{
		return PlayChord [0];	
	}


}
