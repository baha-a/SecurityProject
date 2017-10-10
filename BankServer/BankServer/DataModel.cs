using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Code;

class Account : IAccountable
{
    public string AES_Key { get; set; }
    public string RSA_PublicKey { get; set; }

    public string Username { get; set; }
    public string Password { get; set; }
    public int Balance { get; set; }

    public override string ToString()
    {
        return string.Format("username[{0}], password[{1}], balance[{2}]]", Username, "*****", Balance);
    }
}

class Transaction
{
    static int id = 0;
    public Transaction()
    {
        Date = DateTime.Now;
        Id = id++;
    }

    public IAccountable From { get; set; }
    public IAccountable To { get; set; }
    public int Amount { get; set; }

    public int Id { get; private set; }
    public DateTime Date { get; private set; }

    public override string ToString()
    {
        return string.Format("id[{0}], from[{1}], to[{2}], amount[{3}], date[{4}]", Id, From.Username, To.Username, Amount, Date.ToString());
    }
}

class DataSet :IDataProvider
{
    List<Account> Accounts { get; set; }
    List<Transaction> Transactions { get; set; }

    private DataSet()
    {
        Accounts = new List<Account>();

        for (int i = 1 ; i <= 10; i++)
            Accounts.Add(new Account() { Balance = 10000 * i, Username = "user" + i, Password = "pwd" + i });

        Transactions = new List<Transaction>();
    }

    static IDataProvider _this = new DataSet();
    public static IDataProvider CreateDataSet()
    {
        if (_this == null)
            return _this = new DataSet();
        return _this;
    }

    public IAccountable FindByName(string username)
    {
        return Accounts.SingleOrDefault(u => u.Username == username);
    }
    public long TotalBalance()
    {
        long sum = 0;
        Accounts.ForEach(s => sum += s.Balance);
        return sum;
    }

    public bool TransferTo(IAccountable fromUser, string toUser, int amount, ref string msg)
    {
        if (amount <= 0)
            msg = "amount must be bigger then zero";
        else if (fromUser.Balance - amount < 0)
            msg = "you don't have enough money";    
        else if(toUser == fromUser.Username)
            msg = "you can't transfer money to yourself";
        else
        {
            var touser = FindByName(toUser);
            if (touser == null)
                msg = "User \'" + touser + "\' not found";
            else
            {
                fromUser.Balance -= amount;
                touser.Balance += amount;
                Transactions.Add(new Transaction() { From = fromUser, To = touser, Amount = amount });
                return true;
            }
        }
        return false;
    }


    public string[] PrintAllUsersInfo()
    {
        return Accounts.Select(x => x.ToString()).ToArray();
    }

    public string[] PrintAllTransactions()
    {
        return Transactions.Select(x => x.ToString()).ToArray();
    }


    public void AddAccount(string user, string pwd)
    {
        Accounts.Add(new Account() { Username = user, Password = pwd, Balance = 0 });
    }
}

