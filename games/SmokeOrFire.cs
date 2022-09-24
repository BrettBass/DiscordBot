using DiscordBot.games.util;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;

namespace DiscordBot.games;

public class SmokeOrFire
{
    private readonly Deck _gameDeck;
    private readonly Deck _inPlayCards;
    private readonly Deck _removedCards;

    public SmokeOrFire()
    {
        _gameDeck = new Deck();
        _gameDeck.FillDeck();
        _inPlayCards = new Deck();
        _removedCards = new Deck();
    }

    public bool Color(int guess)
    {
        var currentCard = _gameDeck.DrawRandomCard();
        _inPlayCards.Add(currentCard);
        return guess == (int)Guess.Smoke ? currentCard.GetSuite() > 1 : currentCard.GetSuite() < 2;
    }

    public bool Value(int guess)
    {
        var currentCard = _gameDeck.DrawRandomCard();
        var lastCardValue = _inPlayCards.Get(_inPlayCards.Size() - 1).Value;

        _inPlayCards.Add(currentCard);

        if (guess == (int)Guess.Higher) return currentCard.Value > lastCardValue;
        if (guess == (int)Guess.Lower) return currentCard.Value < lastCardValue;
        return currentCard.Value == lastCardValue;
    }

    public int GetCardsInPlay()
    {
        return _inPlayCards.Size();
    }

    public void ClearInPlayCards()
    {
        _removedCards.Add(_inPlayCards);
        _inPlayCards.Flush();
    }

    public void CardsToEmojis(CommandContext ctx, ref DiscordMessageBuilder msg)
    {
        for (var i = 0; i < _inPlayCards.Size(); i++)
            msg.Content += _inPlayCards.Get(i).GetEmoji(ctx);
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