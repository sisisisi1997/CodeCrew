using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace cons_mic_ver
{
	class Eredmeny
	{
		public static void Save(string date, int sec, bool winner)
		{
			if(File.Exists("results.ki"))
				using (StreamWriter ki = new StreamWriter("results.ki", true))
					ki.WriteLine(date + " " + sec + " " + winner);
			else
				using (StreamWriter ki = new StreamWriter("results.ki"))
				{
					ki.WriteLine();
					ki.WriteLine(date + " " + sec + " " + winner);
				}
		}

		public static List<List<string>> Read()
		{
			List<List<string>> back = new List<List<string>>();
			using (StreamReader be = new StreamReader("results.ki"))
				while (!be.EndOfStream)
				{
					string[] beo = be.ReadLine().Split(' ');
					List<string> ossz = new List<string>();
					
					ossz.Add(beo[0]);
					ossz.Add(beo[1]);
					ossz.Add(beo[2]);

					back.Add(ossz);
				}
			
			return back;
		}
	}
}
