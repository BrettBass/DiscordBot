using DiscordBot.games.util;

namespace DiscordBot.games;

public class TexasHoldem
{
    private readonly Deck _deck = new();

    private readonly List<Card> _tableCards = new(5);
    
    public TexasHoldem()
    {
        _deck = new Deck();
        _deck.FillDeck();
    }

    public Tuple<Card, Card>[] Deal(int numPlayers)
    {
        var dealtPairs = new Tuple<Card, Card>[numPlayers];

        for (var i = 0; i < numPlayers; i++)
            dealtPairs[i] = new Tuple<Card, Card>(_deck.DrawRandomCard(), _deck.DrawRandomCard());

        return dealtPairs;
    }

    public List<Card> Flop()
    {
        for (var i = 0; i < 3; i++)
            _tableCards[i] = _deck.DrawRandomCard();

        return _tableCards.GetRange(0, 3);
    }

    public List<Card> River()
    {
        _tableCards[3] = _deck.DrawRandomCard();
        return _tableCards.GetRange(0, 4);
    }

    public List<Card> Turn()
    {
        _tableCards[4] = _deck.DrawRandomCard();
        return _tableCards;
    }
}