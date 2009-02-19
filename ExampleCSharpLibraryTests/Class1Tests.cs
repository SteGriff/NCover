using NUnit.Framework;
using ExampleCSharpLibrary;

namespace ExampleCSharpLibraryTests
{
    [TestFixture]
    public class Class1Tests
    {

        public Class1Tests()
        {
        }

        [Test]
        public void ConstructorTest()
        {
            Class1 target = new Class1();
            Assert.IsNotNull(target);
        }

        [Test]
        public void AMethod1Test()
        {
            Class1 target = new Class1();
            target.AMethod();
        }

    }
}
