namespace POS.Backend.Helper
{
    public static class Utility
    {
        public static string RandomNumberGenerator(int length)
        {
            var random = new Random();
            var chars = "123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(x => x[random.Next(x.Length)]).ToArray());
        }
    }
}
