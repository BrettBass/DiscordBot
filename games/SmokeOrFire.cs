using Discord;
using DiscordBot.games.util;
using DSharpPlus.Entities;

namespace DiscordBot.games;

public class SmokeOrFire
{
    private Deck gameDeck;
    private Deck inPlayCards;
    private Deck removedCards;
    private static Random rnd = new Random();
    
    private enum Guess
    {
        Smoke = 0,
        Fire,
        Higher,
        Lower
    }

    public SmokeOrFire()
    {
        gameDeck = new Deck();
        gameDeck.FillDeck();
        inPlayCards = new Deck();
        removedCards = new Deck();
    }
    public bool Color(int guess)
    {
        var currentCard = gameDeck.DrawCard(rnd.Next(gameDeck.Size()  - 1));
        inPlayCards.Add(currentCard);
        return guess == (int)Guess.Smoke ? (currentCard.GetSuite() > 1) : (currentCard.GetSuite() < 2);
    }

    public bool Value(int guess)
    {
        var currentCard = gameDeck.DrawCard(rnd.Next(gameDeck.Size() - 1));
        var lastCardValue = inPlayCards.Get(inPlayCards.Size() - 1).Value;
        
        inPlayCards.Add(currentCard);
        
        if (guess == (int)Guess.Higher) return currentCard.Value > lastCardValue; 
        if (guess == (int)Guess.Lower) return currentCard.Value < lastCardValue;
        return  currentCard.Value == lastCardValue;
    }

    public int GetCardsInPlay() { return inPlayCards.Size(); }

    public void ClearInPlayCards()
    {
        removedCards.Add(inPlayCards);
        inPlayCards.Flush();
    }

    public DiscordEmbed Display()
    {
        var embed = new DiscordEmbedBuilder()
            .WithTitle("Cards");
        for(int i = 0; i < inPlayCards.Size(); i++)
        {
            embed.AddField("Card " + i+1 + ":", inPlayCards.Get(i).Name);
        }
        return embed.Build();
    }
}