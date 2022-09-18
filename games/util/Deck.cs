namespace DiscordBot.games.util;

public class Deck
{
    private static readonly Random rnd = new();
    public List<Card> Cards = new();

    public void FillDeck()
    {
        //Can use a single loop utilising the mod operator % and Math.Floor
        //Using divition based on 13 cards in a suited
        for (var i = 0; i < 52; i++)
        {
            var suite = (Card.Suites)Math.Floor((decimal)i / 13);
            //Add 1 to value as a cards start at 1 (ACE)
            var val = i % 13 + 1;
            Cards.Add(new Card(val, suite));
        }
    }

    public void PrintDeck()
    {
        foreach (var card in Cards) Console.WriteLine(card.Name);
    }

    public Card DrawCard(int position)
    {
        var removedCard = Cards[position];
        Cards.RemoveAt(position);
        return removedCard;
    }

    public void Add(Card newCard)
    {
        Cards.Add(newCard);
    }

    public void Add(Deck deck)
    {
        foreach (var card in deck.Cards) Add(card);
    }

    public int Size()
    {
        return Cards.Count;
    }

    public Card Get(int index)
    {
        return Cards[index];
    }

    public void Flush()
    {
        Cards = new List<Card>();
    }

    public Card DrawRandomCard()
    {
        var position = rnd.Next(Cards.Count);
        var removedCard = Cards[position];
        Cards.RemoveAt(position);
        return removedCard;
    }
}