using UnityEngine;
using System.Collections;
using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;

public class Main : MonoBehaviour {

	public GameObject mapPrefab;

	public GameObject display;
	public GameObject ui;
	public GameObject map;

	public DisplayController displayController;
	public UIController uiController;
	public MapController mapController;

	public GameState gameState;

	public int result;
	Stopwatch watch;
	TimeSpan ts;
	string elapsedTime;
	
	List<Data> data;

	public int mode = 0;
	public bool withPruning = false;

	int winner;
	int score;
	double time;

	static int resetCount = 9;
	int count = resetCount;

	void Start() {
		uiController = ui.GetComponent<UIController>();
		clear();

		data = new List<Data>();
	}

	void Update()
	{
		modeListener();

		if ( gameState != null )
		{
			if ( gameState.getEndGame() == false || count > 0) // game continues
			{
				if (gameState.getEndGame())
				{
					count--;
					init (3);
					watch = Stopwatch.StartNew();
					//gameState.switchTurn();
					mode = 3;
				}

				switch (mode) {
				case 1 : modePlayerVsPlayer(); 
					break;
				case 2 : modePlayerVsComputer(); 
					break;
				case 3 : modeComputerVsComputer(); 
					break;
				}

				if ( mapController != null && mapController.refresh)
				{
					refresh();
				}


				if (gameState.getEndGame() && count == 0)
				{
					UnityEngine.Debug.Log("Saving to file");
					saveToFile();
				}
	
			}
			else // end of game
			{

			}
		}
		else // cleared no game in site
		{

		}

	}

	void modeListener() {

		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			init (1);
			mode = 1;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2)) {
			init (2);
			mode = 2;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3)) {
			data.Clear();
			count = resetCount;

			init (3);
			watch = Stopwatch.StartNew();
			//gameState.switchTurn();
			mode = 3;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha8))
		{
			toggleHeuristics();
		}
		else if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			togglePruning();
		}
		else if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			clear ();
			mode = 0;
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			uiController.incrDepth();
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			uiController.decrDepth();
		}
		
	}

	void modePlayerVsPlayer() {
		

	}
	
	void modePlayerVsComputer() {

		if (gameState.getTurn() == 2) {

			if (withPruning)
				gameState = gameState.minimaxWithPruning( uiController.depth );
			else
				gameState = gameState.minimax( uiController.depth );

			mapController.refresh = true;
		}
		
	}
	
	void modeComputerVsComputer() {

		if (gameState.getTurn() == 1) {

			if (withPruning)
				gameState = gameState.minimaxWithPruning( uiController.depth );
			else
				gameState = gameState.minimax( uiController.depth );
			
			mapController.refresh = true;
		}
		
		else if (gameState.getTurn() == 2) {
			
			if (withPruning)
				gameState = gameState.minimaxWithPruning( uiController.depth );
			else
				gameState = gameState.minimax( uiController.depth );
			
			mapController.refresh = true;
		}


		// Debug.Log ("Turn:" + gameState.getTurn()+ " , maxNode: " + gameState.getMaxNode());

	}

	void init(int modeNumber) {
		clear ();

		uiController.hide = false;
		uiController.reset();
		uiController.setup(modeNumber,withPruning);

		displayController = display.GetComponent<DisplayController>();
		displayController.EraseBoard();
		
		Board board = new Board();
		gameState = new GameState( board );
		
		map = new GameObject("Map");
		map.transform.parent = gameObject.transform;
		map.AddComponent<MapController>();
		mapController = map.GetComponent<MapController>();
		
		displayController.DisplayBoard(gameState,map.transform);
		
		mapController.gameState = gameState;
	}
	
	void refresh() {
		displayController.EraseBoard();
		
		map = Instantiate(mapPrefab);
		map.transform.parent = gameObject.transform;
		map.AddComponent<MapController>();
		
		displayController.DisplayBoard(gameState,map.transform);

		mapController = map.GetComponent<MapController>();
		mapController.gameState = gameState;

		if (mode==1 || (mode==2 && gameState.getTurn() == 1))
		{
			Algorithms.evaluate(gameState);
		}

		result = gameState.value;

		//Debug.Log (gameState.getTurn() +" = " + result);

		if (gameState.getTurn() == 1)
			uiController.updateLeft(result);
		else
			uiController.updateRight(result);

		if ( gameState.getEndGame() )
		{
			winner = gameState.getWinner();
			score = gameState.value;

			uiController.updateWinMessage(winner);
			gameState.switchTurn();

			watch.Stop();
			ts = watch.Elapsed;
			elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",
		                            	       ts.Minutes,
			                                   ts.Seconds,
			                                   ts.Milliseconds / 10);

			uiController.updateTime(elapsedTime);

			time = ts.TotalMilliseconds;

			data.Add( new Data(winner,score,time));
		}
		else
		{
			gameState.switchTurn();
			uiController.updatePlayerTurn(gameState.getTurn());
		}

		mapController.refresh = false;
	}

	void clear() {
		uiController.hide = true;
		uiController.clear();

		if (displayController != null)
			displayController.EraseBoard();

		displayController = null;
		mapController = null;
		gameState = null;
		result = 0;
		mode = 0;
	}

	void togglePruning() {withPruning = (withPruning) ? false : true ; uiController.updateWithPruning(withPruning); }

	void toggleHeuristics() { 

		if (Algorithms.mode>0 && Algorithms.mode<4) 
			Algorithms.mode++;
		else
			Algorithms.mode = 1;

		uiController.updateHeuristics(Algorithms.mode);
	}

	void saveToFile()
	{
		string log ="";
		foreach( Data entry in data )
			log += entry.ToString();

		UnityEngine.Debug.Log (log);
	}
}


