using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//BoxCanvas
public class click_UI : MonoBehaviour
{
	public Animator animator;
	public Text chordText;


	private int now_ten_num = 0;
	private int now_chord_num = 0;

	public string root_text = "C";
	public string ten_text = "M";

	void Awake ()
	{
		setChordName ();
	}

	public void OnClick ()
	{
		Debug.Log ("Button click!");
	}

	public void OnClickEditMenu ()
	{
		if (animator.GetBool ("isEditMenuShow") == false) {
			animator.SetBool ("isEditMenuShow", true);
		} else {
			animator.SetBool ("isEditMenuShow", false);
		}
	}

	public void OnClickChordMenu ()
	{
		if (animator.GetBool ("isChordMenuShow") == false) {
			animator.SetBool ("isChordMenuShow", true);
		} else {
			animator.SetBool ("isChordMenuShow", false);
		}
	}

	public void OnClickRootMenu ()
	{
		if (animator.GetBool ("isRootMenuShow") == false) {
			animator.SetBool ("isRootMenuShow", true);
		} else {
			animator.SetBool ("isRootMenuShow", true);
		}
	}

	public void OnClickPopularMenu ()
	{
		if (animator.GetBool ("isPopularMenuShow") == false) {
			animator.SetBool ("isPopularMenuShow", true);
		} else {
			animator.SetBool ("isPopularMenuShow", true);
		}
	}

	public void setChordName ()
	{
		chordText.text = root_text + "<size=80>" + ten_text + "</size>";
	}

	public string getChordText ()
	{
		return root_text + ten_text;
	}


	//BoxCanvas内にあるChord_Textの数を返すために使いたい
	public int getChordAmount ()
	{
		return 1;
	}


	public void setRootText (int chord_num)
	{
		switch (chord_num) {
		case 0:
			root_text = "A";
			break;
		case 1:
			root_text = "A#";
			break;
		case 2:
			root_text = "B";
			break;
		case 3:
			root_text = "C";
			break;
		case 4:
			root_text = "C#";
			break;
		case 5:
			root_text = "D";
			break;
		case 6:
			root_text = "D#";
			break;
		case 7:
			root_text = "E";
			break;
		case 8:
			root_text = "E#";
			break;
		case 9:
			root_text = "F";
			break;
		case 10:
			root_text = "G";
			break;
		case 11:
			root_text = "G#";
			break;
		default:
			break;

		}

		now_chord_num = chord_num;
		setChordName ();
	}



	public void setTensionText (int ten_num)
	{
		
		switch (ten_num) {
		case 0:
			ten_text = "M";
			break;		
		case 1:
			ten_text = "m";
			break;
		case 2:
			ten_text = "M7";
			break;
		case 3:
			ten_text = "m7";
			break;
		case 4:
			ten_text = "7";
			break;
		case 5:
			ten_text = "sus4";
			break;
		case 6:
			ten_text = "dim";
			break;
		case 7:
			ten_text = "aug";
			break;
		case 8:
			ten_text = "m(b5)";
			break;
		case 9:
			ten_text = "6";
			break;
		case 10:
			ten_text = "add9";
			break;
		case 11:
			ten_text = "m7(b5)";
			break;
		default:
			break;

		}
		now_ten_num = ten_num;
		setChordName ();
	}
}



