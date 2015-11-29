using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Apache.Ignite.Demo.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Apache.Ignite.Demo.Tests
{
    [TestClass]
    public class RepositoryTests
    {
        private Repository _repo;

        [TestInitialize]
        public void TestInitialize()
        {
            _repo = Repository.Instance;

            _repo.PopulateDemoData().Wait();
        }

        [TestMethod]
        public void TestGetPersons()
        {
            var persons = _repo.GetPersons().ToArray();

            Assert.AreEqual(100, persons.Length);
        }

        [TestMethod]
        public async Task TestFriendship()
        {
            var persons = _repo.GetPersons().ToArray();

            var p1 = persons[0];
            var p2 = persons[1];

            await _repo.AddFriendAsync(p1, p2);

            CollectionAssert.AreEquivalent(new[] {p2.Id}, _repo.GetFriendIds(p1.Id).ToArray());
            Assert.AreEqual(0, _repo.GetFriendIds(p2.Id).Count());
        }
    }
}
