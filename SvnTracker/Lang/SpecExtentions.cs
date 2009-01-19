using NUnit.Framework;

namespace SvnTracker.Lang
{
    public static class SpecExtentions
    {
        public static void ShouldEqual(this object obj, object other)
        {
            Assert.AreEqual(other, obj);
        }
        public static void ShouldNotEqual(this object obj, object other)
        {
            Assert.AreNotEqual(other, obj);
        }
    }
}
