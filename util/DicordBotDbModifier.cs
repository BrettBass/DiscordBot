using System.Data.SQLite;
using DiscordBot.Models;

namespace discordBot.util;

public class DiscordBotDbModifier
{
    private static readonly SQLiteConnection Con = new (Environment.GetEnvironmentVariable("DISCORDBOTDB_CONNECTIONSTRING"));

    public void Add(Entity entity)
    {
        try
        {
            Con.Open();
            using var cmd = new SQLiteCommand(Con);
            cmd.CommandText = $"INSERT INTO {entity.TableEntityString()} VALUES({entity.ToEntityString()})";
            cmd.ExecuteNonQuery();
            Console.WriteLine("add finished");
            Con.Close();
        } catch(Exception e) { Console.WriteLine(e.Message);}
    }

    public static T? Pull<T>(ulong id, string column)
    {
        T? res = default;
        try
        {
            Con.Open();
            string sql = @$"SELECT {column} FROM Users WHERE Id=@Id";
            using SQLiteCommand cmd = new (sql, Con);
            cmd.Parameters.Add(new ("@Id", id.ToString()));
            
            var reader = cmd.ExecuteReader();

            // int ord = reader.GetOrdinal(column);
            if(!reader.Read())
            {
                reader.Close();
                Con.Close();
                return default;
            }
            var temp = reader[column].ToString();
            reader.Close();
            Con.Close();

            if (temp == null) return default;
            res = (T)Convert.ChangeType(temp, typeof(T));
            
        } catch(Exception e) { Console.WriteLine(e.Message); }
        Con.Close();

        return res;
    }
    
    public static void Set(ulong id, string column, string value)
    {
        string sql = $@"UPDATE Users set {column}=@Value WHERE Id=@Id;";
        UpdateCell(sql, id, value);
    }

    public static void Increment(ulong id, string column, int value)
    {
        if (!Exists(id)) AddEntity(id);
        string sql = $@"UPDATE Users set {column}={column}+@Value WHERE Id=@Id;";
        UpdateCell(sql, id, value.ToString());
    }
    
    public static void Decrement(ulong id, string column, int value)
    {
        string sql = $@"UPDATE Users set {column}={column}-@Value WHERE Id=@Id;";
        UpdateCell(sql, id, value.ToString());
    }
    
    private static void UpdateCell(string sql, ulong id, string value)
    {
        using var cmd = new SQLiteCommand(sql, Con);
        var param = new SQLiteParameter[2];
        param[0] = new("@Value", value);
        param[1]  = new ("@Id", id.ToString());
        foreach (SQLiteParameter p in param) cmd.Parameters.Add(p);
        ExecSqlCmd(cmd.ExecuteNonQuery);
    }
        
    private static void ExecSqlCmd<T>(Func<T> func)
    {
        Con.Open();
        func();
        Con.Close();
    }

    public static bool Exists(ulong id)
    {
        var res = false;
        try
        {
            using var cmd = new SQLiteCommand(Con);
            cmd.CommandText = $"SELECT * FROM Users WHERE Id={id}";
            Con.Open();
            res = cmd.ExecuteScalar() != null;
            Con.Close();
        } catch(Exception e) { Console.WriteLine(e.Message);}

        return res;
    }

    public static void AddEntity(ulong id)
    {
        string sql = $"INSERT INTO Users (Id) VALUES ({id});";
        ExecSqlCmd(new SQLiteCommand(sql, Con).ExecuteNonQuery);
    }

}