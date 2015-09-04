using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public GameObject leftScoreObject;
	public GameObject rightScoreObject;
	public GameObject turnTextObject;
	public GameObject modeTextObject;
	public GameObject depthTextObject;
	public GameObject timeTextObject;
	public GameObject heurTextObject;

	Text leftText;
	Text rightText;
	Text turnText;
	Text modeText;
	Text depthText;
	Text timeText;
	Text heurText;

	public int playerLeftScore=0;
	public int playerRightScore=0;
	public int depth=1;

	string mode = "";

	public bool hide = false;

	void Start()
	{
		leftText  = leftScoreObject.GetComponent<Text>();
		rightText = rightScoreObject.GetComponent<Text>();
		turnText  = turnTextObject.GetComponent<Text>();
		modeText  = modeTextObject.GetComponent<Text>();
		depthText = depthTextObject.GetComponent<Text>();
		timeText = timeTextObject.GetComponent<Text>();
		heurText = heurTextObject.GetComponent<Text>();
	}

	public void reset()
	{
		turnText.text = "";
		modeText.text = "";
		timeText.text = "";

		updateLeft(0);
		updateRight(0);
	}

	public void clear()
	{
		leftText  = leftScoreObject.GetComponent<Text>();
		rightText = rightScoreObject.GetComponent<Text>();
		turnText  = turnTextObject.GetComponent<Text>();
		modeText  = modeTextObject.GetComponent<Text>();
		timeText  = timeTextObject.GetComponent<Text>();

		turnText.text = "";
		modeText.text = "";
		leftText.text = "";
		rightText.text = "";
		timeText.text = "";

		mode = "";
	}

	public void setup(int mode,bool pruning) {
		switch (mode)
		{
		case 1 : 
			updateMode("Player VS Player",false);
			updatePlayerTurn(1);
			updateLeft(0);
			updateRight(0);
			timeText.text = "";
			break;

		case 2 : 
			updateMode("Player VS Computer",pruning);
			updatePlayerTurn(1);
			updateLeft(0);
			updateRight(0);
			timeText.text = "";
			break;

		case 3 : 
			updateMode("Computer VS Computer",pruning);
			updatePlayerTurn(1);
			updateLeft(0);
			updateRight(0);
			timeText.text = "";
			break;
		}

	}
	
	public void incrementLeft()
	{
		playerLeftScore++;
		leftText.text = "" + playerLeftScore;
	}
	
	public void incrementRight()
	{
		playerRightScore++;
		rightText.text = "" + playerRightScore;
	}
	
	public void updateLeft(int score)
	{
		playerLeftScore = score;
		leftText.text = "" + playerLeftScore;
	}
	
	public void updateRight(int score)
	{
		playerRightScore = score;
		rightText.text = "" + playerRightScore;
	}

	public void updatePlayerTurn(int player)
	{
		if (player == 1)
		{
			turnText.text = "Turn: Player 1 (Blue)";
		}
		else if (player==2)
		{
			turnText.text = "Turn: Player 2 (Red)";
		}
		else
		{
			Debug.Log("Error in updatePlayerTurn");
		}
	}

	public void updateMode(string text,bool pruning)
	{
		mode = text;
		modeText.text = "" + text + "\nPruning: " + ((pruning)?"on":"off");
	}

	public void updateWinMessage(int player)
	{
		if (player == 1)
			turnText.text = "Blue Player Wins";
		else 
			turnText.text = "Red Player Wins";
	}

	public void updateWithPruning(bool pruning) {
			modeText.text = "" + mode + "\nPruning: " + ((pruning)?"on":"off");
	}

	public void updateDepth(int value)
	{
		depth = value;
		depthText.text = "Depth: " + depth;
	}

	public void incrDepth()
	{
		depth += (depth<3)?+1:0;
		depthText.text = "Depth: " + depth;
	}
	public void decrDepth()
	{
		depth -= (depth>1)?1:0;
		depthText.text = "Depth: " + depth;
	}

	public void updateTime(string value) { timeText.text = "Time: " + value ; }

	public void updateHeuristics(int value)
	{

		switch (value)
		{
		case 1: heurText.text = "Heur1 / Heur1"; 
			break;
		case 2: heurText.text = "Heur1 / Heur2"; 
			break;
		case 3: heurText.text = "Heur2 / Heur1"; 
			break;
		case 4: heurText.text = "Heur2 / Heur2"; 
			break;
		default: heurText.text = "error";
			break;
		}


	}
}

