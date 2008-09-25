using System.IO;
using NUnit.Framework;
using SharpCover.Utilities;

namespace SharpCover.Parsing.CSharp
{
	[TestFixture]
	public class ParserTest
	{
		public ParserTest()
		{
		}

		[Test]
		public void TestConstructor()
		{
			Parser parser = new Parser(new ReportSettings());
			Assert.AreEqual(0, parser.CoveragePoints.Length);
			Assert.IsNotNull(parser.Matchers);
			Assert.IsTrue(parser.Matchers.Count > 0);
		}

		[Test]
		public void TestMatching()
		{
			Parser parser = new Parser(new ReportSettings());
			MockMatcher mockmatcher = new MockMatcher(null);

			string source = GetCodeFile();
			parser.Matchers.Clear();
			parser.Matchers.Add(mockmatcher);
			
			mockmatcher.InsertPointReturnValue = -1;
			string result = parser.ParseString(source);

			Assert.IsTrue(mockmatcher.InsertPointCalled);
			Assert.IsFalse(mockmatcher.MatchCalled);
			Assert.AreEqual(result, source);
			
			mockmatcher.InsertPointReturnValue = -2;
			mockmatcher.InsertPointCalled = false;

			result = parser.ParseString(source);

			Assert.IsTrue(mockmatcher.InsertPointCalled);
			Assert.IsTrue(mockmatcher.MatchCalled);
			Assert.AreEqual("foofoo", result);
		}
		[Test]
		public void TestLiveMatching()
		{
			Parser parser = new Parser(new ReportSettings());
			string source = GetCodeFile();
			string result = parser.ParseString(source);
			Assert.IsTrue(source != result);
		}

		[Test]
		public void TestParseFile()
		{
			Parser parser = new Parser(new ReportSettings());
			
			string filename = Path.GetTempFileName();

			FileStream fs = new FileStream(filename, FileMode.Create);
			ResourceManager.WriteResourceToStream(fs ,"SharpCover.Resources.CodeFile.cs", ResourceType.Text, this.GetType().Assembly);
			fs.Close();

			string source = GetCodeFile();
			
			string fileresult = parser.Parse(filename);
			Assert.IsNotNull(fileresult);
			string stringresult = parser.ParseString(source);
			Assert.IsNotNull(stringresult);
		}

		private string GetCodeFile()
		{
			string retval;
			Stream stream = ResourceManager.GetResource("SharpCover.Resources.CodeFile.cs", this.GetType().Assembly);
			using (StreamReader reader = new StreamReader(stream))
			{
				retval = reader.ReadToEnd();
			}

			return retval;
		}
	}
}
