using System;

public class Data {

	public int winner;
	public int score;
	public double time;	

	public Data(int a, int b, double c)
	{
		winner = a;
		score = b;
		time = c;
	}

	public override string ToString()
	{
		return "" + winner + "\t" + time + "\n";
	}
}
