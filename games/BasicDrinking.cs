namespace DiscordBot.games{
public class BasicDrinking {
    private static readonly Random Rnd = new Random();
    public static bool CoinFlip() {
        return Rnd.Next(2) < 1;
    }

    public static int NumberGuess()
    {
        return Rnd.Next(1, 11);
    }
    
}
}