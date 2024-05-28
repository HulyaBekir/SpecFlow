using NUnit.Framework;

namespace MyTests
{
    [TestFixture]
    public class MyTestClass
    {
        [OneTimeSetUp]
        public void Init()
        {
            // Initialization code here
            // Runs once before all tests in this class
        }

        [SetUp]
        public void Setup()
        {
            // Setup code here
            // Runs before each test method in this class
        }

        [Test]
        public void Test1()
        {
            // Your test code here
        }

        [Test]
        public void Test2()
        {
            // Your test code here
        }

        [TearDown]
        public void Cleanup()
        {
            // Cleanup code here
            // Runs after each test method in this class
        }

        [OneTimeTearDown]
        public void Dispose()
        {
            // Teardown code here
            // Runs once after all tests in this class
        }
    }
}
