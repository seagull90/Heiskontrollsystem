using Heiskontrollsystem.Infrastructure;

namespace Heiskontrollsystem
{
    public class ElevatorService
    {
        private static int _maxFloorNumber;

        public static async Task ExecuteAsync()
        {
            string maxFloorNumberString;

            while (true)
            {
                Console.Write("Enter number of floors in the building. Must be between 2 and 100: ");
                maxFloorNumberString = Console.ReadLine();

                if (Validator.IsValidMaxFloorNumber(maxFloorNumberString))
                {
                    break;
                }
            }

            _maxFloorNumber = Convert.ToInt32(maxFloorNumberString);

            var destinations = GetDestinations();
            var fastElevator = new Elevator(2);
            fastElevator.SetDestinations(destinations);
            var tokenSource = new CancellationTokenSource();
            var task = Task.Run(async () => await fastElevator.Move(tokenSource.Token), tokenSource.Token);

            Console.Write("Press 'x' flor emergency break");
            string isEmergencyBreakRequested;

            while (true)
            {
                isEmergencyBreakRequested = Console.ReadLine();

                if (task.IsCompleted)
                {
                    fastElevator.SetDestinations(GetDestinations());
                    task = Task.Run(async () => await fastElevator.Move(tokenSource.Token), tokenSource.Token);
                }

                if (tokenSource.Token.IsCancellationRequested)
                {
                    Console.Write("Emergency break");
                    break;
                }

                if (isEmergencyBreakRequested == "x")
                    tokenSource.Cancel();
            }
        }

        private static List<int> GetDestinations()
        {
            var destinationFloorsString = "";

            while (true)
            {
                Console.Write(string.Format("Enter destination floors in comma-separated values format e.g 1, 2, 3. Destination floors must be between 1 and {0}: ", _maxFloorNumber));
                destinationFloorsString = Console.ReadLine();

                if (Validator.IsValidDestinationFloors(destinationFloorsString))
                {
                    break;
                }
            }

            var destinations = destinationFloorsString.Split(',').Select(int.Parse).ToList();

            return destinations;
        }
    }
}