using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;


public class Alarms
{
	public int countAlarms(int[] volume, int S) {
		int count = 0;
		int n = volume.Length;
		int idx = 0;
		while (S > 0) {
			S -= volume[idx];
			count++;
			idx = (idx + 1) % n;
		}
		return count;
	}

	#region Testing code
	[STAThread]
	private static Boolean KawigiEdit_RunTest(int testNum, int[] p0, int p1, Boolean hasAnswer, int p2) {
		Console.Write("Test " + testNum + ": [" + "{");
		for (int i = 0; p0.Length > i; ++i) {
			if (i > 0) {
				Console.Write(",");
			}
			Console.Write(p0[i]);
		}
		Console.Write("}" + "," + p1);
		Console.WriteLine("]");
		Alarms obj;
		int answer;
		obj = new Alarms();
		DateTime startTime = DateTime.Now;
		answer = obj.countAlarms(p0, p1);
		DateTime endTime = DateTime.Now;
		Boolean res;
		res = true;
		Console.WriteLine("Time: " + (endTime - startTime).TotalSeconds + " seconds");
		if (hasAnswer) {
			Console.WriteLine("Desired answer:");
			Console.WriteLine("\t" + p2);
		}
		Console.WriteLine("Your answer:");
		Console.WriteLine("\t" + answer);
		if (hasAnswer) {
			res = answer == p2;
		}
		if (!res) {
			Console.WriteLine("DOESN'T MATCH!!!!");
		} else if ((endTime - startTime).TotalSeconds >= 2) {
			Console.WriteLine("FAIL the timeout");
			res = false;
		} else if (hasAnswer) {
			Console.WriteLine("Match :-)");
		} else {
			Console.WriteLine("OK, but is it right?");
		}
		Console.WriteLine("");
		return res;
	}
	public static void Run() {
		Boolean all_right;
		all_right = true;
		
		int[] p0;
		int p1;
		int p2;
		
		// ----- test 0 -----
		p0 = new int[]{5,2,4};
		p1 = 13;
		p2 = 4;
		all_right = KawigiEdit_RunTest(0, p0, p1, true, p2) && all_right;
		// ------------------
		
		// ----- test 1 -----
		p0 = new int[]{5,2,4};
		p1 = 3;
		p2 = 1;
		all_right = KawigiEdit_RunTest(1, p0, p1, true, p2) && all_right;
		// ------------------
		
		// ----- test 2 -----
		p0 = new int[]{1};
		p1 = 10000;
		p2 = 10000;
		all_right = KawigiEdit_RunTest(2, p0, p1, true, p2) && all_right;
		// ------------------
		
		// ----- test 3 -----
		p0 = new int[]{42,68,35,1,70,25,79,59,63,65,6,46,82,28,62,92,96,43,28,37,92,5,3,54,93,83,22,17,19,96,48,27,72,39,70,13,68,100,36,95,4,12,23,34,74,65,42,12,54,69};
		p1 = 9999;
		p2 = 203;
		all_right = KawigiEdit_RunTest(3, p0, p1, true, p2) && all_right;
		// ------------------
		
		if (all_right) {
			Console.WriteLine("You're a stud (at least on the example cases)!");
		} else {
			Console.WriteLine("Some of the test cases had errors.");
		}
	}
	#endregion
}
