namespace ExampleCSharpLibrary
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class Class1
	{
		public Class1()
		{}

		public void AMethod()
		{SharpCover.Results.Add(@"examplecsharp", @"C:\Projekte\ncover\testresults\examplecsharp-actual.xml", 7);  	
			bool x = true;

			if (SharpCover.Results.Add(@"examplecsharp", @"C:\Projekte\ncover\testresults\examplecsharp-actual.xml", 1) && true && true)
			{
				x = false;
			}

			if (SharpCover.Results.Add(@"examplecsharp", @"C:\Projekte\ncover\testresults\examplecsharp-actual.xml", 2) && true && !x)
				x = false;

			int i = 0;
			while (SharpCover.Results.Add(@"examplecsharp", @"C:\Projekte\ncover\testresults\examplecsharp-actual.xml", 3) && i == 0)
			{
				i++;
			}

			while (SharpCover.Results.Add(@"examplecsharp", @"C:\Projekte\ncover\testresults\examplecsharp-actual.xml", 4) && i == 0)
				i++;

			for (int j = 0;SharpCover.Results.Add(@"examplecsharp", @"C:\Projekte\ncover\testresults\examplecsharp-actual.xml", 5) &&  j < 3; j++)
			{
				j = j;
			}

			for (int j = 0;SharpCover.Results.Add(@"examplecsharp", @"C:\Projekte\ncover\testresults\examplecsharp-actual.xml", 6) && j < 3; j++)
				j = j;

			switch (i)
			{
				case 1:
					break;
				case 2:
					break;
				case 3:
					break;
				default:
					break;
			}
		}	
	}
}







