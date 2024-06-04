namespace Utilities
{
    public static class Help_Numbers
    {
        public static int GenerateUniqueNumber(int digits = 6)
        {
            HashSet<int> existingNumbers = new HashSet<int>();

            if (digits <= 0 || digits > 9)
            {
                throw new ArgumentException("Number of digits must be between 1 and 9.");
            }

            Random random = new Random();
            int min = (int)Math.Pow(10, digits - 1);
            int max = (int)Math.Pow(10, digits) - 1;
            int newNumber;

            do
            {
                newNumber = random.Next(min, max + 1); // Generates a number between min and max (inclusive)
            } while (existingNumbers.Contains(newNumber));

            existingNumbers.Add(newNumber);
            return newNumber;
        }
    }
}
