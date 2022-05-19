
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordBot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>{
        DrinkExchange bank = new DrinkExchange();

        [Command("ping")]
        public async Task Ping(){
            await ReplyAsync("go fuck yourself");
        }

        //two commands for same action, balance and bal
        [Command("balance")]
        [Alias("bal")]
        public async Task Balance(){
            //get the id of user who sent the command
            var user = Context.User;

            // convert id to string
            var userId = user.Id.ToString();

            var embed = new EmbedBuilder()
                .WithTitle("Balance")
                .WithColor(new Color(0, 255, 0))
                .AddField("User", user.Username);
                Dictionary<string, double> balances = bank.AllBalances(userId);
                foreach (KeyValuePair<string, double> entry in balances)
                    embed.AddField(entry.Key, entry.Value, true);
                
            await ReplyAsync("", false, embed.Build());

        }

        [Command("deposit")]
        public async Task Deposit(string currency, double amount){
            var user = Context.User;
            var userId = user.Id.ToString();
            
            ICurrency cur = bank.GetCurrency(currency);
            if(cur == null){
                await ReplyAsync("Invalid currency");
                return;
            }
            bank.Deposit(userId, cur, amount);
            await ReplyAsync("Deposited " + amount + " " + currency);
        }

        //case insensitive command withdraw
        [Command("withdraw")]

        public async Task Withdraw(string currency, double amount){
            var user = Context.User;
            var userId = user.Id.ToString();
            
            ICurrency cur = bank.GetCurrency(currency);
            if(cur == null){
                await ReplyAsync("Invalid currency");
                return;
            }
            
            bank.Withdraw(userId, cur, amount);
            await ReplyAsync("Withdrew " + amount + " " + currency);
        }

        [Command("addDrink")]
        [Alias("ad"), Summary("increase the amount of drinks"), Remarks("addDrink <user> <amount>")]
        public async Task AddDrink(string user, int amount){
            //given the username pull the id
            var userId = user;
            
            //if string starts with <@ then remove it and string ends with > then remove it
            if(userId.StartsWith("<@") && userId.EndsWith(">")){
                userId = userId.Substring(2, userId.Length - 3);
            }
            //check if user exists in guild
            if(Context.Guild.Users.FirstOrDefault(x => x.Id == ulong.Parse(userId)) == null){
                await ReplyAsync("User not found");
                return;
            }

            var cur = bank.GetCurrency("Drinks");
            
            bank.Withdraw(userId, cur, amount);
            await ReplyAsync("Added " + amount + " drinks to " + user);
        }
    }
}
