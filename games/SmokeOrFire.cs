using DiscordBot.games.util;

namespace DiscordBot.games;

public class SmokeOrFire
{
    private Deck gameDeck;
    private Deck inPlayCards;
    private Deck removedCards;
    private static Random rnd = new Random();
    
    public SmokeOrFire() { gameDeck.FillDeck(); }
    public bool Color(string guess)
    {
        var currentCard = gameDeck.DrawCard(rnd.Next(gameDeck.Size()  - 1));
        var result = guess == "smoke" ? (currentCard.Suite.CompareTo(2) < 0) : (currentCard.Suite.CompareTo(1) > 0);
        if (result) inPlayCards.Add(currentCard);
        return result;
    }

    public bool Value(string guess)
    {
        var currentCard = gameDeck.DrawCard(rnd.Next(gameDeck.Size() - 1));
        var lastCardValue = removedCards.Get(removedCards.Size() - 1).Value;
        bool result;
        
        removedCards.Add(currentCard);
        
        if (guess == "higher") result = currentCard.Value > lastCardValue; 
        else if (guess == "lower") result = currentCard.Value < lastCardValue;
        else result = currentCard.Value == lastCardValue;
        
        if(!result) inPlayCards.Flush();
        return result;
    }

    public int GetCardsInPlay() { return inPlayCards.Size(); }

    public void ClearInPlayCards()
    {
        removedCards.Add(inPlayCards);
        inPlayCards.Flush();
    }
}