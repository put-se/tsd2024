using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;


public class Ameba
{
	public int simulate(int[] X, int A)
	{
		int ameba_size = A;
		for (int i = 0; i < X.Length; ++i)
		{
			if (X[i] == ameba_size)
			{
				ameba_size += X[i];
			}
		}
		return ameba_size;
	}

	#region Testing code
	[STAThread]
	private static Boolean KawigiEdit_RunTest(int testNum, int[] p0, int p1, Boolean hasAnswer, int p2)
	{
		Console.Write("Test " + testNum + ": [" + "{");
		for (int i = 0; p0.Length > i; ++i)
		{
			if (i > 0)
			{
				Console.Write(",");
			}
			Console.Write(p0[i]);
		}
		Console.Write("}" + "," + p1);
		Console.WriteLine("]");
		Ameba obj;
		int answer;
		obj = new Ameba();
		DateTime startTime = DateTime.Now;
		answer = obj.simulate(p0, p1);
		DateTime endTime = DateTime.Now;
		Boolean res;
		res = true;
		Console.WriteLine("Time: " + (endTime - startTime).TotalSeconds + " seconds");
		if (hasAnswer)
		{
			Console.WriteLine("Desired answer:");
			Console.WriteLine("\t" + p2);
		}
		Console.WriteLine("Your answer:");
		Console.WriteLine("\t" + answer);
		if (hasAnswer)
		{
			res = answer == p2;
		}
		if (!res)
		{
			Console.WriteLine("DOESN'T MATCH!!!!");
		}
		else if ((endTime - startTime).TotalSeconds >= 2)
		{
			Console.WriteLine("FAIL the timeout");
			res = false;
		}
		else if (hasAnswer)
		{
			Console.WriteLine("Match :-)");
		}
		else
		{
			Console.WriteLine("OK, but is it right?");
		}
		Console.WriteLine("");
		return res;
	}
	public static void Main(string[] args)
	{
		Boolean all_right;
		all_right = true;

		int[] p0;
		int p1;
		int p2;

		// ----- test 0 -----
		p0 = new int[] { 2, 1, 3, 1, 2 };
		p1 = 1;
		p2 = 4;
		all_right = KawigiEdit_RunTest(0, p0, p1, true, p2) && all_right;
		// ------------------

		// ----- test 1 -----
		p0 = new int[] { 1, 4, 9, 16, 25, 36, 49 };
		p1 = 10;
		p2 = 10;
		all_right = KawigiEdit_RunTest(1, p0, p1, true, p2) && all_right;
		// ------------------

		// ----- test 2 -----
		p0 = new int[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 1024, 2048 };
		p1 = 1;
		p2 = 512;
		all_right = KawigiEdit_RunTest(2, p0, p1, true, p2) && all_right;
		// ------------------

		// ----- test 3 -----
		p0 = new int[] { 817, 832, 817, 832, 126, 817, 63, 63, 126, 817, 832, 287, 823, 817, 574 };
		p1 = 63;
		p2 = 252;
		all_right = KawigiEdit_RunTest(3, p0, p1, true, p2) && all_right;
		// ------------------

		if (all_right)
		{
			Console.WriteLine("You're a stud (at least on the example cases)!");
		}
		else
		{
			Console.WriteLine("Some of the test cases had errors.");
		}
	}
	#endregion
}
