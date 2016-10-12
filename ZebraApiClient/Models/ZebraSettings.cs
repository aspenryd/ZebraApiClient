namespace ZebraApiClient.Models
{
    public class ZebraSettings
    {
        public double Randomness { get; set; }
        public int SearchDepth  { get; set; }
        public int ExactDepth { get; set; }
        public int WinLoseDrawDepth { get; set; }

        public ZebraSettings()
        {
            Randomness = 0.1d;
            SearchDepth = 16;
            ExactDepth = 14;
            WinLoseDrawDepth = 20;
        }
    }
}