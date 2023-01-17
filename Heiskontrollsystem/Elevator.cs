namespace Heiskontrollsystem
{
    public class Elevator
    {
        private List<int> _floorList;
        private Direction _currentDirection = Direction.Up;
        private int _currentFloor;
        private readonly int _secondsPerFloor;
        private List<int> _upQueue;
        private List<int> _downQueue;

        public Elevator(int secondsPerFloor)
        {
            _secondsPerFloor = secondsPerFloor;
        }

        public void SetFloorList(List<int> floorList)
        {
            _floorList = floorList;
        }

        public async Task Move()
        {
            _upQueue = _floorList.Where(x => x > _currentFloor).Distinct().ToList();
            _downQueue = _floorList.Where(x => x < _currentFloor).Distinct().ToList();

            if (_upQueue.Count > _downQueue.Count)
                _currentDirection = Direction.Up;
            else
                _currentDirection = Direction.Down;

            var emergencyBreak = false;
            var millisecondsPerFloor = _secondsPerFloor * 1000;

            ShowKeyMap();

            ConsoleKeyInfo cki;

            while (true)
            {
                await Task.Delay(millisecondsPerFloor);

                if (_currentDirection == Direction.Up)
                {
                    _currentFloor++;
                    await UpdateQueueAndDirection(_upQueue);
                }

                if (_currentDirection == Direction.Down)
                {
                    _currentFloor--;
                    await UpdateQueueAndDirection(_downQueue);
                }

                if (!_upQueue.Any() && !_downQueue.Any())
                {
                    Console.Write("\nCurrent floor: {0}. This is the final stop.", _currentFloor);
                    break;
                }

                if (Console.KeyAvailable == true)
                {
                    cki = Console.ReadKey(true);
                    HandleKey(cki, _currentFloor, _secondsPerFloor, ref emergencyBreak, _currentDirection);
                }

                if (emergencyBreak)
                    break;
            }
        }

        private static void ShowKeyMap()
        {
            Console.Write("\nPress\n" +
                "'b' for emergency break\n" +
                "'t' to return estimated time until a certain floor\n" +
                "'d' to show current direction\n" +
                "'f' to show current floor\n");
        }

        private static void HandleKey(ConsoleKeyInfo cki, int currentFloor, int secondsPerFloor, ref bool emergencyBreak, Direction currentDirection)
        {
            switch (cki.Key)
            {
                case ConsoleKey.T:
                    var task = Task.Run(() => GetTimeEstimate(currentFloor, secondsPerFloor));
                    break;

                case ConsoleKey.B:
                    Console.Write("\nEmergency break triggered");
                    ShowKeyMap();
                    emergencyBreak = true;
                    break;

                case ConsoleKey.D:
                    Console.Write("\nCurrent direction: {0}", currentDirection);
                    ShowKeyMap();
                    break;

                case ConsoleKey.F:
                    Console.Write("\nCurrent floor: {0}", currentFloor);
                    ShowKeyMap();
                    break;
            }
        }
        private static int GetFloorNumber()
        {
            string floorNumberStr;

            while (true)
            {
                floorNumberStr = Console.ReadLine();

                if (Validator.IsValidFloorNumber(floorNumberStr))
                    break;
            }

            var maxFloor = Convert.ToInt32(floorNumberStr);

            return maxFloor;
        }

        private static void GetTimeEstimate(int currentFloor, int secondsPerFloor)
        {
            Console.Write("\nEnter floor number to see time estimate: ");
            var floorNumber = GetFloorNumber();
            var diff = Math.Abs(currentFloor - floorNumber);
            var timeEstimate = Math.Abs(diff * secondsPerFloor);
            Console.Write("\nTime to travel between current floor: {0} and floor: {1} is {2} seconds", currentFloor, floorNumber, timeEstimate);
            ShowKeyMap();
        }

        private void UpdateDirection()
        {
            if (!_upQueue.Any() && _currentDirection == Direction.Up)
                _currentDirection = Direction.Down;

            if (!_downQueue.Any() && _currentDirection == Direction.Down)
                _currentDirection = Direction.Up;
        }

        private async Task UpdateQueueAndDirection(List<int> queue)
        {
            var lastStop = "";
            var passangersOut = "";
            var direction = _currentDirection;

            if (queue.Contains(_currentFloor))
            {
                passangersOut = ". Letting passangers in/ out";
                direction = Direction.Stopped;
                await Task.Delay(2000);
                queue.Remove(_currentFloor);
                UpdateDirection();

                if (!_upQueue.Any() && !_downQueue.Any())
                    lastStop = ". This is the final stop";
            }

            //Console.Write("Floor: {0}, Direction: {1}{2}{3}", _currentFloor, direction, passangersOut, lastStop);
        }
    }
}