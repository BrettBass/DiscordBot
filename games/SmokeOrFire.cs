using DiscordBot.games.util;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;

namespace DiscordBot.games;

public class SmokeOrFire
{
    private readonly Deck gameDeck;
    private readonly Deck inPlayCards;
    private readonly Deck removedCards;

    public SmokeOrFire()
    {
        gameDeck = new Deck();
        gameDeck.FillDeck();
        inPlayCards = new Deck();
        removedCards = new Deck();
    }

    public bool Color(int guess)
    {
        var currentCard = gameDeck.DrawRandomCard();
        inPlayCards.Add(currentCard);
        return guess == (int)Guess.Smoke ? currentCard.GetSuite() > 1 : currentCard.GetSuite() < 2;
    }

    public bool Value(int guess)
    {
        var currentCard = gameDeck.DrawRandomCard();
        var lastCardValue = inPlayCards.Get(inPlayCards.Size() - 1).Value;

        inPlayCards.Add(currentCard);

        if (guess == (int)Guess.Higher) return currentCard.Value > lastCardValue;
        if (guess == (int)Guess.Lower) return currentCard.Value < lastCardValue;
        return currentCard.Value == lastCardValue;
    }

    public int GetCardsInPlay()
    {
        return inPlayCards.Size();
    }

    public void ClearInPlayCards()
    {
        removedCards.Add(inPlayCards);
        inPlayCards.Flush();
    }

    public void CardsToEmojis(CommandContext ctx, ref DiscordMessageBuilder msg)
    {
        for (var i = 0; i < inPlayCards.Size(); i++)
            msg.Content += inPlayCards.Get(i).GetEmoji(ctx);
    }

    private enum Guess
    {
        Smoke = 0,
        Fire,
        Higher,
        Lower,
        SameCard
    }
}