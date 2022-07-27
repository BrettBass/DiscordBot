namespace DiscordBot.games{
public class BasicDrinking {
    private static Random rnd = new Random();
    public static Boolean CoinFlip() {
        return rnd.Next(1) < 1;
    }

    public static int NumberGuess()
    {
        return rnd.Next(9) + 1;
    }
    
}
}