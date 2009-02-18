using System.IO;
using NUnit.Framework;

namespace SharpCover.Utilities
{
	[TestFixture]
	public class ResourceManagerTests
	{
		public ResourceManagerTests()
		{
		}

		private readonly string resource = "SharpCover.Resources.ExpectedCoverageFile.xml";

		[Test]
		public void TestGetResource()
		{
			// When resource doesn't exist
			Assert.IsNull(ResourceManager.GetResource("foo"));
			Assert.IsNull(ResourceManager.GetResource("foo", this.GetType().Assembly));
		
			Assert.IsNotNull(ResourceManager.GetResource(this.resource, this.GetType().Assembly));
		}

		[Test]
		public void TestWriteResourceToStream()
		{
			MemoryStream ms = new MemoryStream();
			Assert.AreEqual(0, ms.Length);
			ResourceManager.WriteResourceToStream(ms, resource, ResourceType.Text, this.GetType().Assembly);
			Assert.IsTrue(ms.Length > 0);
		}

		[Test]
		public void TestWriteResourceToStreamWhenResourceNotFound()
		{
			MemoryStream ms = new MemoryStream();
			Assert.AreEqual(0, ms.Length);
			ResourceManager.WriteResourceToStream(ms, resource, ResourceType.Text);
			Assert.IsFalse(ms.Length > 0);
		}

	}
}
