namespace Heiskontrollsystem
{
    public class Elevator
    {
        public List<int> _destinations;
        private Direction _currentDirection = Direction.Up;
        private int _currentFloor;
        private readonly int _secondsPerFloor;
        private List<int> _upQueue;
        private List<int> _downQueue;

        public Elevator(int secondsPerFloor)
        {
            _secondsPerFloor = secondsPerFloor * 1000;
        }

        public void SetDestinations(List<int> destinations)
        {
            _destinations = destinations;
        }

        public async Task Move(CancellationToken ct)
        {
            _upQueue = _destinations.Where(x => x > _currentFloor).Distinct().ToList();
            _downQueue = _destinations.Where(x => x < _currentFloor).Distinct().ToList();

            if (_upQueue.Count > _downQueue.Count)
                _currentDirection = Direction.Up;
            else
                _currentDirection = Direction.Down;

            while (!ct.IsCancellationRequested)
            {
                await Task.Delay(_secondsPerFloor);

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
                    break;
            }
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
            var passangersOut = "";
            var direction = _currentDirection;

            if (queue.Contains(_currentFloor))
            {
                passangersOut = "Letting passangers in/ out.";
                direction = Direction.Stopped;
                await Task.Delay(2000);
                queue.Remove(_currentFloor);
                UpdateDirection();
            }

            Console.WriteLine(string.Format("Current floor: {0}, Current direction: {1}. {2}", _currentFloor, direction, passangersOut));
        }
    }
}