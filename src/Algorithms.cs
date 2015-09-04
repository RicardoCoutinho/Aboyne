using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Algorithms {

	public static int mode = 1;

	public static int player_distanceToGoalValue = +10;
	public static int enemy_distanceToGoalValue  = -10;

	public static int player_stoneValue 	= +50;
	public static int enemy_stoneValue 		= -50;

	public static int player_blockedValue	= +100;
	public static int enemy_blockedValue	= -100;

	public static int player_winValue      	= +10000;
	public static int enemy_winValue     	= -10000;

	// ----------------

	public static int player_distanceToGoalValue2 = +20;
	public static int enemy_distanceToGoalValue2  = -10;
	
	public static int player_stoneValue2 	= +75;
	public static int enemy_stoneValue2 	= -50;
	
	public static int player_blockedValue2	= +200;
	public static int enemy_blockedValue2	= -100;
	
	public static int player_winValue2      = +10000;
	public static int enemy_winValue2     	= -10000;


	public static int minimax(GameState gameState, int depth)
	{
		if ( gameState.isEndofGame() || depth == 0) 
		{	
			return evaluate(gameState);
		}
		else {

			int v;

			List<GameState> successors = gameState.getSuccessors();
			bool isMaxNode = successors[0].maxNode;

			if (isMaxNode) // Max Mode
			{
				v = Int32.MinValue;
				
				foreach ( GameState successor in successors )
				{
					v = Math.Max (v , minimax(successor,depth-1));
				}
			}
			else // Min Mode
			{
				v = Int32.MaxValue;
				
				foreach ( GameState successor in successors )
				{
					v = Math.Min (v , minimax(successor,depth-1));
				}
			}
			
			return v;
		}
		
	}

	public static int minimaxWithPruning(GameState gameState, int depth, int alpha, int beta)
	{
		if ( gameState.isEndofGame() || depth == 0) 
		{	
			return evaluate(gameState);
		}
		else
		{
			int v;

			List<GameState> successors = gameState.getSuccessors();
			bool isMaxNode = successors[0].maxNode;
			
			if (isMaxNode) // Max Mode
			{
				v = Int32.MinValue;
				foreach(GameState successor in successors)
				{
					v = Int32.MinValue;
					alpha = Math.Max( v , minimaxWithPruning(successor, depth-1, alpha, beta));

					if(beta <= alpha)
					{break;}
				}
			}
			else // min node
			{
				v = Int32.MaxValue;
				foreach(GameState successor in successors)
				{
					v = Int32.MaxValue;
					beta = Math.Min( v , minimaxWithPruning(successor, depth-1, alpha, beta));
					
					if(beta <= alpha)
					{break;}
				}
			}

			return v;
		}

	}


	public static int evaluate(GameState gameState)
	{
		int score=0;
		
		List<Coordinate> coordinateList = gameState.getStoneCoords();
		
		int  
			player = gameState.getTurn(), 
			enemy  = (player==1) ? 2 : 1 ,
			pStones=0,
			eStones=0,
			pBlocked=0,
			eBlocked=0,
			pWin=0,
			pDistance=0,
			eDistance=0,
			cellValue=0;
		
		foreach ( Coordinate coordinate in coordinateList )
		{		
			cellValue = gameState.getCell(coordinate);
			
			if (cellValue == player)
			{
				pStones  += 1;
				pBlocked += gameState.isBlocked(coordinate) ? 1 : 0;
				pDistance += (17 - gameState.deviation(4,16,coordinate.getLine(),coordinate.getColumn()));
			}
			else if (cellValue == enemy)
			{
				eStones  += 1;
				eBlocked += gameState.isBlocked(coordinate) ? 1 : 0;
				eDistance +=  (17 - gameState.deviation(4,0,coordinate.getLine(),coordinate.getColumn()));
			}
		}
		
		if ( gameState.isEndofGameByGoal() || eStones==eBlocked ) // win
		{
			gameState.setEndGame(true);
			gameState.setWinner(player);
			pWin = 1;
		}

		if (mode == 1 || (mode == 2 && player == 1) || (mode == 2 && player == 2))
		{
			score = (gameState.getWinner()>0) ? 
					(
						(pWin 		* player_winValue)
					) : 
					(
						(pStones	* player_stoneValue) +
						(eStones 	* enemy_stoneValue) +
						
						(pBlocked	* player_blockedValue) +
						(eBlocked 	* enemy_blockedValue) + 
						
						(pDistance	* player_distanceToGoalValue) +
						(eDistance 	* enemy_distanceToGoalValue)
						);
		}
		else
		{
			score = (gameState.getWinner()>0) ? 
					(
						(pWin 		* player_winValue2)
					) : 
					(
						(pStones	* player_stoneValue2) +
						(eStones 	* enemy_stoneValue2) +
						
						(pBlocked	* player_blockedValue2) +
						(eBlocked 	* enemy_blockedValue2) + 
						
						(pDistance	* player_distanceToGoalValue2) +
						(eDistance 	* enemy_distanceToGoalValue2)
						);
		}






		//Debug.Log ("Player >>"+ player+" "+score +" :: " + pWin + " "+ pStones + " "+ eStones + " "+ pBlocked + " "+ eBlocked + " "+ pDistance + " " + eDistance );

		return score;
	}


}
