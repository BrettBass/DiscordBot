using Newtonsoft.Json;

namespace discordBot.util;

public record UserData
{
    [JsonConstructor]
    public UserData(double chipBalance = 100, double barTab = 0, double trustworthy = 0)
    {
        _chipBalance = chipBalance;
        _barTab = barTab;
        _trustworthy = trustworthy;
    }

    public double _chipBalance { get; set; }
    public double _barTab { get; set; }
    public double _trustworthy { get; set; }

    public void UpdateBalance(double amount)
    {
        _chipBalance += amount;
    }

    public void UpdateTab(double amount)
    {
        _barTab += amount;
    }
}