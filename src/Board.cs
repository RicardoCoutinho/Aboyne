using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Board
{
    private static bool debug = false;

    public  static int INVISIBLE = -1;
	public static int EMPTY = 0;
	public static int PLAYER1 = 1;
	public static int PLAYER2 = 2;
	public static int GOAL1 = 3;
	public static int GOAL2 = 4;

    private int n;
    private int height;
    private int width;

    public int[][] board;

    public Board() // size of 5 implementation
    {
        n = 5;
        height = 2 * n - 1;
        width = 4 * n - 3;

        initBoard();
    }

    public Board(int n) // generic implementation
    {
        this.n = n;
        height = 2 * n - 1;
        width = 4 * n - 3;

        initBoard();
    }

    public Board(Board b) // generic implementation
    {
        this.width = b.getWidth();
        this.height = b.getHeight();
        this.n = b.getN();
        this.board = cloneBoard(b.getBoard());
    }

    public int getN() { return n; }
    public int getHeight() { return height; }
    public int getWidth() { return width; }
    public int[][] getBoard() { return board; }

    public override String ToString()
    {
        String matrix = "";
        int value;

        for (int i = 0; i < height; i++)
        {
            value = getCell(i, 0);

            matrix += " " + ((value == -1) ? " " : "" + value);

            for (int j = 1; j < width; j++)
            {
                value = getCell(i, j);
                matrix += " , " + ((value == -1) ? " " : "" + value);
            }

            matrix += "\n";
        }

        return "Board{" +
            "n=" + n +
                ", height=" + height +
                ", width=" + width +
                ", board=\n" + matrix +
                "}";
    }

    public void initBoard()
    {
        board = new int[height][];
        for (int i = 0; i < board.Length; i++) { board[i] = new int[width]; };
        int start, end;

        // sets all cells INVISIBLE
        for (int line = 0; line < height; line++)
        {
            for (int col = 0; col < width; col++)
            {
                setCell(line, col, INVISIBLE);
            }
        }

        // sets EMPTY cells depending on N
        for (int line = 0; line < n; line++)
        {
            start = n - line - 1;
            end = start + (n + line) * 2;

            for (int col = start; col < end; col += 2)
            {
                setCell(line, col, EMPTY);
                if (line + 1 < n)
                    setCell(height - line - 1, col, EMPTY);
            }
        }

        int col1;
        int col2;
        int line2;

        // sets starting pieces on board
        for (int line = 0; line < n - 1; line++)
        {
            col1 = n - line - 1;
            col2 = col1 + (n + line - 1) * 2;
            line2 = height - line - 1;

            setCell(line, col1, PLAYER1);
            setCell(line, col1 + (n + line - 1) * 2, PLAYER2);

            if (height - line - 1 > 0)
            {
                setCell(line2, col1, PLAYER1);
                setCell(line2, col2, PLAYER2);
            }
        }

        setCell(n - 1, 2, PLAYER1);
        setCell(n - 1, width - 2 - 1, PLAYER2);

        setCell(n - 1, 0, GOAL2);
        setCell(n - 1, width - 1, GOAL1);

    }

	public void initBoard2()
	{
		board = new int[height][];
		for (int i = 0; i < board.Length; i++) { board[i] = new int[width]; };
		int start, end;
		
		// sets all cells INVISIBLE
		for (int line = 0; line < height; line++)
		{
			for (int col = 0; col < width; col++)
			{
				setCell(line, col, INVISIBLE);
			}
		}
		
		// sets EMPTY cells depending on N
		for (int line = 0; line < n; line++)
		{
			start = n - line - 1;
			end = start + (n + line) * 2;
			
			for (int col = start; col < end; col += 2)
			{
				setCell(line, col, EMPTY);
				if (line + 1 < n)
					setCell(height - line - 1, col, EMPTY);
			}
		}

		setCell(0, 4, PLAYER1);
		setCell(0, 6, PLAYER1);
		setCell(0, 8, PLAYER1);
		setCell(0, 10, PLAYER1);
		setCell(0, 12, PLAYER1);

		setCell(1, 5, PLAYER1);
		setCell(1, 7, PLAYER1);
		setCell(1, 9, PLAYER1);
		setCell(1, 11, PLAYER1);

		setCell(8, 4, PLAYER2);
		setCell(8, 6, PLAYER2);
		setCell(8, 8, PLAYER2);
		setCell(8, 10, PLAYER2);
		setCell(8, 12, PLAYER2);

		setCell(7, 5, PLAYER2);
		setCell(7, 7, PLAYER2);
		setCell(7, 9, PLAYER2);
		setCell(7, 11, PLAYER2);
		
		setCell(n - 1, 0, GOAL2);
		setCell(n - 1, width - 1, GOAL1);
		
	}


	public void initBoard3()
	{
		board = new int[height][];
		for (int i = 0; i < board.Length; i++) { board[i] = new int[width]; };
		int start, end;
		
		// sets all cells INVISIBLE
		for (int line = 0; line < height; line++)
		{
			for (int col = 0; col < width; col++)
			{
				setCell(line, col, INVISIBLE);
			}
		}
		
		// sets EMPTY cells depending on N
		for (int line = 0; line < n; line++)
		{
			start = n - line - 1;
			end = start + (n + line) * 2;
			
			for (int col = start; col < end; col += 2)
			{
				setCell(line, col, EMPTY);
				if (line + 1 < n)
					setCell(height - line - 1, col, EMPTY);
			}
		}
		
		setCell(0, 4, PLAYER1);
		setCell(1, 3, PLAYER1);
		setCell(1, 5, PLAYER1);
		setCell(2, 2, PLAYER1);
		setCell(2, 4, PLAYER1);
		setCell(2, 6, PLAYER1);
		setCell(3, 5, PLAYER1);
		setCell(3, 3, PLAYER1);
		setCell(3, 1, PLAYER1);
		
		setCell(8, 12, PLAYER2);
		setCell(7, 11, PLAYER2);
		setCell(7, 13, PLAYER2);
		setCell(6, 10, PLAYER2);
		setCell(6, 12, PLAYER2);
		setCell(6, 14, PLAYER2);
		setCell(5, 11, PLAYER2);
		setCell(5, 13, PLAYER2);
		setCell(5, 15, PLAYER2);
		
		setCell(n - 1, 0, GOAL2);
		setCell(n - 1, width - 1, GOAL1);
		
	}

    public int getCell(Coordinate c) { return getCell(c.getLine(), c.getColumn()); }
    public int getCell(int line, int column)
    {
        if (line >= 0 && line < height && column >= 0 && column < width)
        {
            return board[line][column];
        }

        if (debug)
            Debug.Log("getCell - bad coordinates, line: " + line + ", column:" + column);

        return -1;
    }
	
    public bool setCell(Coordinate c, int value) { return setCell(c.getLine(), c.getColumn(), value); }
    public bool setCell(int line, int column, int value)
    {
        if (line >= 0 && line < height && column >= 0 && column < width)
        {
            board[line][column] = value;
            return true;
        }

        if (debug)
            Debug.Log("setCell - bad coordinates, line: " + line + ", column:" + column);

        return false;
    }

	public static int[][] cloneBoard(int[][] input)
	{
		if (input == null)
			return null;
		int[][] result = new int[input.Length][];
		
		int y = input.Length;
		int x = input[0].Length;
		
		for (int i = 0; i < y; i++)
			result[i] = new int[x];

		for (int i=0; i<y; i++)
			for (int j=0; j<x; j++)
				result[i][j] = input[i][j];

		return result;
	}
    

}