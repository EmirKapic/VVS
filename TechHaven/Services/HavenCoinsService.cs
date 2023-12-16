namespace TechHaven.Services
{
    public class HavenCoinsService
    {
        private readonly IFileOperations _fileOperations;
        public List<int[]> csv;
        public HavenCoinsService(IFileOperations fileOperations)
        {
            csv = new List<int[]>();
            _fileOperations = fileOperations;
            var lines = _fileOperations.ReadAllLines("../../../usercoins.csv");
            foreach (var line in lines)
            {
                var values = line.Split(',');
                csv.Add(new int[2] { Int32.Parse(values[0]), Int32.Parse(values[1]) });
            }
        }

        public int getHavenCoins(int id)
        {
            try
            {
                var usersLine = csv.First(element => element[0] == id);
                return usersLine[1];
            }
            catch (InvalidOperationException)
            {
                throw new ArgumentException("User with such id doesnt exist");
            }
        }

        public bool AddHavenCoins(int id, int coins)
        {
            if (coins < 0)
            {
                throw new ArgumentException("Can't subtract coins");
            }
            try
            {
                var usersLine = csv.First(element => element[0] == id);
                if (usersLine[1] + coins > 50)
                {
                    return false;
                }
                usersLine[1] += coins;
                SaveNewCsv();
                return true;
            }
            catch (InvalidOperationException)
            {
                throw new ArgumentException("User with such id doesnt exist");
            }
        }

        public bool SubtractHavenCoins(int id, int coins)
        {
            if (coins < 0)
            {
                throw new ArgumentException("Can't subtract coins");
            }
            try
            {
                var usersLine = csv.First(element => element[0] == id);
                if (usersLine[1] - coins < 0)
                {
                    return false;
                }
                usersLine[1] -= coins;
                SaveNewCsv();
                return true;
            }
            catch (InvalidOperationException)
            {
                throw new ArgumentException("User with such id doesnt exist");
            }
        }

        public void SaveNewCsv()
        {
            var lines = new List<string>();
            foreach (var entry in csv)
            {
                lines.Add(string.Join(",", entry));
            }

            _fileOperations.WriteAllLines("../../../usercoins.csv", lines.ToArray());
        }
    }
}
