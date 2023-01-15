using System.Threading.Tasks;

namespace Heiskontrollsystem
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await ElevatorService.ExecuteAsync();
        }
    }
}