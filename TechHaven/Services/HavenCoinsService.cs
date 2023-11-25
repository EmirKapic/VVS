namespace TechHaven.Services
{
    public class HavenCoinsService
    {
        public List<int[]> csv;
        public HavenCoinsService()
        {
            csv = new List<int[]>();
            var lines = File.ReadAllLines("../../../usercoins.csv");
            foreach (var line in lines.Skip(1))
            {
                var values = line.Split(',');
                csv.Add(new int[2] { int.Parse(values[0]), int.Parse(values[1]) });
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


        private void SaveNewCsv()
        {
            using (StreamWriter sw = new StreamWriter("../../../usercoins.csv"))
            {
                sw.WriteLine("id,coins");
                foreach(var entry in csv)
                {
                    sw.WriteLine($"{entry[0]},{entry[1]}");
                }
            }
        }
    }
}
