namespace DiscordBot.games.util;

public class Deck
{
    public List<Card> Cards = new List<Card>();
    private static Random rnd = new Random();
    public void FillDeck()
    {
        //Can use a single loop utilising the mod operator % and Math.Floor
        //Using divition based on 13 cards in a suited
        for (int i = 0; i < 52; i++)
        {
            Card.Suites suite = (Card.Suites)(Math.Floor((decimal)i/13));
            //Add 1 to value as a cards start at 1 (ACE)
            int val = i%13 + 1;
            Cards.Add(new Card(val, suite));
        }
    }

    public void PrintDeck()
    {
        foreach(Card card in this.Cards)
        {
            Console.WriteLine(card.Name);
        }
    }

    public Card DrawCard(int position)
    {
        Card removedCard = Cards[position];
        Cards.RemoveAt(position);
        return removedCard;
    }

    public void Add(Card newCard) { Cards.Add(newCard); }
    
    public void Add(Deck deck) { foreach (var card in deck.Cards) { Add(card); } }
    public int Size() { return Cards.Count; }
    
    public Card Get(int index) { return Cards[index]; }
    
    public void Flush() { Cards = new List<Card>(); }

    public Card DrawRandomCard()
    {
        int position = rnd.Next(Cards.Count);
        Card removedCard = Cards[position];
        Cards.RemoveAt(position);
        return removedCard;
    }
}
