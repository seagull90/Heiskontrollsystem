using System.Drawing;
using System.Text.RegularExpressions;

namespace Heiskontrollsystem.Infrastructure
{
    public static class Validator
    {
        private static int maxFloor;

        private static bool IsNumber(string str)
        {
            if (!new Regex("[0-9]").IsMatch(str))
            {
                Console.WriteLine(string.Format("'{0}' is an invalid floor number format. Only positive integers are allowed", str));

                return false;
            }

            return true;
        }

        private static bool IsWithinLimits(int floor, int max)
        {
            if (floor < 1 || floor > max)
            {
                Console.WriteLine(string.Format("'{0}' is an invalid floor number. Must be between 1 and {1}", floor, max));

                return false;
            }

            return true;
        }

        public static bool IsValidMaxFloorNumber(string maxFloorNumberString)
        {
            if (!IsNumber(maxFloorNumberString))
                return false;


            var maxFloorNumber = Convert.ToInt32(maxFloorNumberString);

            if (maxFloorNumber == 1)
            {
                Console.WriteLine(string.Format("'{0}' is an invalid floor number. Must be between 2 and 100", maxFloorNumber));
                return false;
            }

            if (!IsWithinLimits(maxFloorNumber, 100))
                return false;

            maxFloor = maxFloorNumber;

            return true;
        }

        private static bool IsValidFloorNumber(string floorNumberString)
        {
            if (!IsNumber(floorNumberString))
                return false;

            var maxFloorNumber = Convert.ToInt32(floorNumberString);

            if (!IsWithinLimits(maxFloorNumber, maxFloor))
                return false;

            return true;

        }

        internal static bool IsValidDestinationFloors(string destinationFloorsString)
        {
            string[] floors = destinationFloorsString.Split(',').Select(sValue => sValue.Trim()).ToArray();

            foreach (string floor in floors)
            {
                if (IsValidFloorNumber(floor))
                    continue;
                else
                    return false;
            }

            return true;
        }
    }
}