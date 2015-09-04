using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameState : Board {
	
	private static bool debug=false;

	public int value=0;
	public bool maxNode=true;
	
	private int turn = PLAYER1;
	private bool endGame = false;
	private int winner = 0;

	public GameState(Board board) : base(new Board(board)) {}
	
	public GameState(Board board, int turn, bool endGame) : base(board)
	{
		setEndGame(endGame);
	}

	public int getWinner() { return winner; }
	public void setWinner(int value) { winner = value; }

	public int getTurn() { return turn; }
	public bool getMaxNode() { return maxNode; }

	public void switchTurn() { turn = (turn == PLAYER1) ? PLAYER2 : PLAYER1; }
	public void switchMaxNode() { maxNode = (maxNode) ? false : true; }


	public bool getEndGame() { return endGame; }
	public void setEndGame(bool endGame) { this.endGame = endGame; }

	public override String ToString() {
		
		return base.ToString() + '\n' +
			"GameState{" +
				" value=" + value +
				", turn=" + turn +
				", endGame=" + endGame +
				"}\n";
	}
	

	public bool isCell(Coordinate c) { return (getCell(c) > -1); }
	public bool isCell(int line, int column) { return (getCell(line, column) > -1); }
	public bool isCell(int cellValue) { return (cellValue > -1); }

	public bool isPlayerStone(int i, int j) { return getCell(i,j) == turn; }
	
	public bool isStone(Coordinate c) { return (getCell(c) > 0); }
	public bool isStone(int line, int column) { return (getCell(line, column) > 0); }
	public bool isStone(int cellValue) { return (cellValue > 0); }
	
	
	public bool isPlayer(Coordinate c, int player) { return isPlayer(c.getLine(), c.getColumn(), player); }
	public bool isPlayer(int line, int column, int player)
	{
		int cellValue = getCell(line, column);
		return (cellValue == player);
	}
	public bool isPlayer(int cellValue, int player) { return (cellValue == player); }
	
	
	public bool isEnemy(Coordinate c, int player) { return isEnemy(c.getLine(), c.getColumn(), player); }
	public bool isEnemy(int line, int column, int player)
	{
		int cellValue = getCell(line, column);
		return (cellValue > 0 && cellValue != player && cellValue != GOAL1 && cellValue != GOAL2);
	}
	public bool isEnemy(int cellValue, int player) { return (cellValue > 0 && cellValue != player); }
	
	public bool isGoal(int cellValue, int player)
	{
		return ((player == PLAYER1 && cellValue == GOAL1) || (player == PLAYER2 && cellValue == GOAL2)) ? true : false ;
	}
	
	public bool isEnemyGoal(int cellValue, int player)
	{
		return ((player == PLAYER1 && cellValue == GOAL2) || (player == PLAYER2 && cellValue == GOAL1)) ? true : false ;
	}
	
	
	public bool isBlocked(Coordinate c) { return isBlocked(c.getLine(), c.getColumn()); }
	public bool isBlocked(int line, int column)
	{
		int player = getCell(line, column);
		
		return (
			isEnemy(line + 0, column - 2, player) ||
			isEnemy(line - 1, column + 1, player) ||
			isEnemy(line + 1, column + 1, player) ||
			isEnemy(line + 0, column + 2, player) ||
			isEnemy(line + 1, column - 1, player) ||
			isEnemy(line - 1, column - 1, player)
			);
	}
	
	
	public bool removeStone(Coordinate c) { return removeStone(c.getLine(), c.getColumn()); }
	public bool removeStone(int line, int column) { return setCell(line, column, EMPTY); }
	
	
	public bool isInLineMovement(Coordinate c) { return isInLineMovement(c.getLine(), c.getColumn()); }
	public bool isInLineMovement(int deltaLine, int deltaColumn)
	{
		bool horizontal = (deltaLine == 0 && deltaColumn > 0 && (deltaColumn % 2) == 0);
		bool diagonal = (deltaLine > 0 && deltaLine == deltaColumn);
		
		return (horizontal ^ diagonal);
	}
	
	
	public bool isJump(Coordinate src, Coordinate dst) { return isJump(src.getLine(), src.getColumn(), dst.getLine(), dst.getColumn()); }
	public bool isJump(int srcLine, int srcColumn, int destLine, int destColumn)
	{
		int deviationLine, deviationColumn, incrI, incrJ;
		int player;
		
		player = getCell(srcLine, srcColumn);
		
		deviationLine = destLine - srcLine;
		deviationColumn = destColumn - srcColumn;
		
		bool diagonal = false;
		
		if (deviationLine == 0)
		{
			incrI = 0;
			incrJ = (deviationColumn > 0) ? 2 : -2;
		}
		else // deviationLine == deviationColumn
		{
			diagonal = true;
			incrI = (deviationLine > 0) ? 1 : -1;
			incrJ = (deviationColumn > 0) ? 1 : -1;
		}
		
		for (int i = srcLine, j = srcColumn; (diagonal && i != destLine && j != destColumn) || (!diagonal && j != destColumn); i += incrI, j += incrJ)
		{
			if (player != getCell(i, j))
			{
				return false;
			}
		}
		
		return true;
	}


	public int deviation(int srcLine, int srcColumn, int dstLine, int dstColumn) {
		return (int) Math.Floor (Math.Sqrt(Math.Pow(dstLine-srcLine,2) + Math.Pow(dstColumn-srcColumn,2)));
	}	
	
	public bool isEndofGame() { return (isEndofGameByGoal() || !anyMovesLeft()); }
	public bool isEndofGameByGoal()
	{
		return (PLAYER1 == getCell(getN()-1, getWidth()-1) || PLAYER2 == getCell(getN()-1, 0));
	}
	
	public bool anyMovesLeft()
	{

		bool p1 = false, p2 = false;
		
		for (int i = 0; i < getHeight(); i++)
			for (int j = 0; j < getWidth(); j++)
		{
			if (!p1 && isPlayer(i, j, PLAYER1) && !isBlocked(i, j))
				p1 = true;

			if (!p2 && isPlayer(i, j, PLAYER2) && !isBlocked(i, j))
				p2 = true;

			if (p1 && p2)
				return true;
		}
		
		return false;
	}
	
	public bool moveStone(Coordinate src, Coordinate dst, int player) { return moveStone(src.getLine(), src.getColumn(), dst.getLine(), dst.getColumn(), player); }
	public bool moveStone(int srcLine, int srcColumn, int destLine, int destColumn, int player)
	{
		int srcCell, destCell, deltaLine, deltaColumn;
		
		srcCell = getCell(srcLine, srcColumn);
		destCell = getCell(destLine, destColumn);
		
		if (player != PLAYER1 && player != PLAYER2)
		{
			if (debug)
				Debug.Log("moveStone - invalid player");
			return false;
		}
		
		if (srcCell != player)
		{
			if (debug)
				Debug.Log("moveStone - invalid source coordinates");
			return false;
		}
		
		if (isBlocked(srcLine, srcColumn))
		{
			if (debug)
				Debug.Log("moveStone - stone is blocked");
			return false;
		}
		
		if (!isCell(destCell) || destCell == player || isEnemyGoal(destCell,player))
		{
			if (debug)
				Debug.Log("moveStone - invalid destination coordinates");
			return false;
		}
		
		// movimento
		deltaLine = System.Math.Abs(srcLine - destLine);
		deltaColumn = System.Math.Abs(srcColumn - destColumn);
		
		if (!isInLineMovement(deltaLine, deltaColumn))
		{
			if (debug)
				Debug.Log("moveStone - invalid movement");
			return false;
		}
		
		if (deltaLine <= 1 && deltaColumn <= 2) // movimento simples
		{
			if (debug)
				Debug.Log("moveStone - movimento simples");
			removeStone(srcLine, srcColumn);
			return setCell(destLine, destColumn, srcCell);
		}
		else if((deltaLine > 1 || deltaColumn > 2) && isJump(srcLine, srcColumn, destLine, destColumn)) // movimento complexo, salto por cima de peças
		{
			if (debug)
				Debug.Log("moveStone - movimento complexo");
			removeStone(srcLine, srcColumn);
			return setCell(destLine, destColumn, srcCell);
		}
		
		if (debug)
			Debug.Log("moveStone - invalid movement");
		
		return false;
	}
	
	// todas as pedras em jogo
	public List<Coordinate> getStoneCoords()
	{
		List<Coordinate> stones = new List<Coordinate>();

		for (int i = 0; i < getHeight(); i++)
			for (int j = 0; j < getWidth(); j++)
				if ( isStone(i,j) )
						stones.Add(new Coordinate(i, j));
			
		return stones;
	}

	// todas as pedras do jogador em turno
	public List<Coordinate> getStoneCoordsForPlayerTurn()
	{
		List<Coordinate> stones = new List<Coordinate>();
		
		for (int i = 0; i < getHeight(); i++)
			for (int j = 0; j < getWidth(); j++)
				if ( isPlayerStone(i,j) )
					stones.Add(new Coordinate(i, j));
		
		return stones;
	}
	
	public List<GameState> getSuccessors()
	{
		List<GameState> completeStateList = new List<GameState>();
		List<GameState>  stateList;
		List<Coordinate> coordinateList = getStoneCoordsForPlayerTurn();

		//Debug.Log ("coordinateList count: " + coordinateList.Count);
		if ( isEndofGame() ) return completeStateList;

		foreach (Coordinate coordinate in coordinateList)
		{
			stateList = getSucessorsFor(coordinate);

			//Debug.Log ("stateList count: " + stateList.Count);

			if (stateList != null)
				foreach (GameState state in stateList)
					completeStateList.Add(state);
		}
		
		return completeStateList;
	}
	
	public List<GameState> getSucessorsFor(Coordinate coordinate)
	{
		List<GameState> stateList = new List<GameState>();
		GameState temp;

		//Debug.Log ("------------------- SUCCESSORS " + coordinate.ToString());

		temp = moveTopLeft(coordinate);
		if ( temp != null ) {
			stateList.Add( temp );
		}
		temp = moveLeft(coordinate);
		if ( temp != null ) {
			stateList.Add( temp );
		}
		temp = moveTopRight(coordinate);
		if ( temp != null ) {
			stateList.Add( temp );
		}
		temp = moveBottomLeft(coordinate);
		if ( temp != null ) {
			stateList.Add( temp );
		}
		temp = moveRight(coordinate);
		if ( temp != null ) {
			stateList.Add( temp );
		}
		temp = moveBottomRight(coordinate);
		if ( temp != null ) {
			stateList.Add( temp );
		}
		return stateList;
	}
	
	
	public GameState moveTopLeft(Coordinate src)     { return moveGeneric(src, new Coordinate(-1,-1) ); }
	public GameState moveLeft(Coordinate src)        { return moveGeneric(src, new Coordinate(+0,-2) ); }
	public GameState moveTopRight(Coordinate src)    { return moveGeneric(src, new Coordinate(-1,+1) ); }
	public GameState moveBottomLeft(Coordinate src)  { return moveGeneric(src, new Coordinate(+1,-1) ); }
	public GameState moveRight(Coordinate src)       { return moveGeneric(src, new Coordinate(+0,+2) ); }
	public GameState moveBottomRight(Coordinate src) { return moveGeneric(src, new Coordinate(+1,+1) ); }
	
	
	public GameState moveGeneric(Coordinate src, Coordinate direction)
	{
		GameState newState=null;
		int player = getCell(src);
		Coordinate dst = move(src,direction,player,false);

		//Debug.Log (" ------------------ Move Generic :: Cell - "+ player + " line:" + src.getLine() + " , column:"+src.getColumn()+" , dst: " + dst);

		if ( dst != null )
		{
			newState = new GameState( (Board) this );

			newState.turn = this.turn;
			newState.switchMaxNode();

			if (!newState.moveStone(src,dst,player) )
				return null;
			newState.setEndGame( newState.isEndofGame() );
          	//Debug.Log(newState.ToString());
		}
		
		return newState;
	}
	
	public Coordinate move(Coordinate src, Coordinate direction, int player, bool jumpedOverFriend)
	{
		Coordinate position = src.Add(direction);
		//Debug.Log (direction.ToString());
		int cell = getCell(position.getLine(), position.getColumn());
		bool flag = jumpedOverFriend;
		
		if ( cell == -1 ) return null;
		if ( cell == 0 || (flag && isEnemy(cell,player)) || isGoal(cell,player)) return position;
		if (!flag && cell == player) flag = true;
		
		return move(direction,position,player,flag);
	}


	public GameState minimax(int depth) 
	{
		List<GameState> successors = getSuccessors();
		List<GameState> bestStates = new List<GameState>();
		int bestValue = Int32.MinValue;

		GameState bestState=null;

		foreach ( GameState successor in successors )
		{
			successor.turn  = getTurn() ;
			successor.value = Algorithms.minimax(successor,depth-1);

			if (bestStates.Count == 0)
			{
				bestStates.Add(successor);
				bestValue = successor.value;
			}
			else if (bestValue < successor.value)
			{
				bestStates.Clear();
				bestStates.Add(successor);
				bestValue = successor.value;
			}
			else if (bestValue == successor.value)
			{
				bestStates.Add(successor);
			}
		}

		/*
		string app = "";
		foreach(GameState vin in bestStates)
			app += vin.value + " , ";
		Debug.Log(app + "\n%%%%%%%%%%%%%%%%%%%%%%%%%");
		*/

		int numStates = bestStates.Count;

		if (numStates > 1)
		{
			System.Random rnd = new System.Random();
			bestState = bestStates[ rnd.Next(0, numStates) ];
		}
		else if (numStates == 1)
		{
			bestState = bestStates[0];
		}

		return bestState;
	}

	public GameState minimaxWithPruning(int depth) 
	{
		List<GameState> successors = getSuccessors();
		List<GameState> bestStates = new List<GameState>();
		int bestValue = Int32.MinValue;
		
		GameState bestState=null;

		int 
			alpha=Int32.MinValue,
			beta=Int32.MaxValue;

		foreach ( GameState successor in successors )
		{
			successor.value = Algorithms.minimaxWithPruning(successor, depth, alpha, beta);

			if (bestStates.Count == 0)
			{
				bestStates.Add(successor);
				bestValue = successor.value;
			}
			else if (bestValue < successor.value)
			{
				bestStates.Clear();
				bestStates.Add(successor);
				bestValue = successor.value;
			}
			else if (bestValue == successor.value)
			{
				bestStates.Add(successor);
			}
		}

		int numStates = bestStates.Count;
		
		if (numStates > 1)
		{
			System.Random rnd = new System.Random();
			bestState = bestStates[ rnd.Next(0, numStates) ];
		}
		else if (numStates == 1)
		{
			bestState = bestStates[0];
		}
		
		return bestState;
	}

}