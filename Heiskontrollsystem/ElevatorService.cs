namespace Heiskontrollsystem
{
    public class ElevatorService
    {
        public static void Execute()
        {
            var maxFloor = GetMaxFloor();
            var floorList = GetFloorList(maxFloor);
            var elevator = new Elevator(2);
            elevator.SetFloorList(floorList);
            var task = Task.Run(async () => await elevator.Move());

            while(true)
            {
                if (task.IsCompleted)
                {
                    floorList = GetFloorList(maxFloor);
                    elevator.SetFloorList(floorList);
                    task = Task.Run(async () => await elevator.Move());
                }
            }
        }

        private static int GetMaxFloor()
        {
            string maxFloorStr;

            while (true)
            {
                Console.Write("Enter number of floors in the building. Must be between 2 and 100: ");
                maxFloorStr = Console.ReadLine();

                if (Validator.IsValidMaxFloorNumber(maxFloorStr))
                    break;
            }

            var maxFloor = Convert.ToInt32(maxFloorStr);

            return maxFloor;
        }

        private static List<int> GetFloorList(int maxFloor)
        {
            var floorListStr = "";

            while (true)
            {
                Console.Write("\nEnter destination floors in comma-separated values format eg 1, 2, 3 or a single value eg 4. Destination floors must be between 1 and {0}: ", maxFloor);
                floorListStr = Console.ReadLine();

                if (Validator.IsValidFloorList(floorListStr))
                    break;
            }

            var floorList = floorListStr.Split(',').Select(int.Parse).ToList();

            return floorList;
        }
    }
}