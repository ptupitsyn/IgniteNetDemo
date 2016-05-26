using System.Linq;
using System.Threading.Tasks;
using Apache.Ignite.Demo.Data;
using static System.Console;

namespace Apache.Ignite.Demo.Console
{
    class DemoRunner
    {
        static void Main(string[] args)
        {
            //Environment.SetEnvironmentVariable("IGNITE_H2_DEBUG_CONSOLE", "true");

            RunDemo().Wait();
        }

        static async Task RunDemo()
        {
            var repo = Repository.Instance;

            await repo.PopulateDemoData();

            var person = repo.CreatePerson("Vasya Pupkin");
            person.Country = "Russia";
            person.City = "New Vasyki";

            await repo.SavePersonAsync(person);

            var friends = repo.SearchPersons(person.Country + "*");

            foreach (var friend in friends.Where(f => person.Id != f.Id))
            {
                await repo.AddFriendAsync(person, friend);
                await repo.AddFriendAsync(friend, person);
            }

            while (true)
            {
                Write("Enter search text: ");
                var text = ReadLine();

                if (string.IsNullOrWhiteSpace(text))
                    return;

                foreach (var p in repo.SearchPersons(text))
                {
                    WriteLine(p);

                    foreach (var friend in await repo.GetFriendsAsync(p.Id))
                    {
                        WriteLine(" -> " + friend);
                    }
                }
            }
        }
    }
}
