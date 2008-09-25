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
		{ 	
			bool x = true;

			if (true && true)
			{
				x = false;
			}

			if (true && !x)
				x = false;

			int i = 0;
			while (i == 0)
			{
				i++;
			}

			while (i == 0)
				i++;

			for (int j = 0; j < 3; j++)
			{
				j = j;
			}

			for (int j = 0;j < 3; j++)
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






