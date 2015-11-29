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

            foreach (var p in repo.SearchPersons("Russia"))
            {
                WriteLine(p);
            }
        }
    }
}
