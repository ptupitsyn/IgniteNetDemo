using Apache.Ignite.Demo.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Apache.Ignite.Demo.Tests
{
    [TestClass]
    public class RepositoryTests
    {
        [TestMethod]
        public void TestGetPersons()
        {
            var repo = Repository.Instance;

            var persons = repo.GetPersons();

            Assert.IsNotNull(persons);
        }
    }
}
