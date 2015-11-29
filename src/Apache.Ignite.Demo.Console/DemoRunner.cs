using System.Linq;
using Apache.Ignite.Demo.Data;
using static System.Console;

namespace Apache.Ignite.Demo.Console
{
    class DemoRunner
    {
        static void Main(string[] args)
        {
            RunDemo();
        }

        static async void RunDemo()
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
                await repo.AddFriend(person, friend);
                await repo.AddFriend(friend, person);
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

                    foreach (var friend in repo.GetFriends(p.Id))
                    {
                        WriteLine(" -> " + friend);
                    }
                }
            }
        }
    }
}
