using System;
using NUnit.Framework;

namespace SharpCover.Parsing
{
	[TestFixture]
	public class InsertTest
	{
		public InsertTest()
		{
		}

		[Test]
		public void TestConstructor()
		{
			Insert ins = new Insert(2, "Test");
			Assert.AreEqual(2, ins.InsertPoint, "InsertPoint should be the same as entered by the constructor");
			Assert.AreEqual("Test", ins.SelectedArea, "SelectedArea should be the same as passed to constructor");
			Assert.AreEqual("Test".Length, ins.Length, "Length should be length of text passed to constructor");
		}

		[Test]
		[ExpectedException(typeof(ApplicationException))]
		public void TestInvalidInsertPoint()
		{
			Insert ins = new Insert(-1,"Test");
		}
	}
}
