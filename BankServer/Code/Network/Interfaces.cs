namespace Code
{
    public interface IAccountable
    {
        string Username { get; set; }
        string Password { get; set; }
        int Balance { get; set; }
    }

    public interface IDataProvider
    {
        IAccountable FindByName(string username);
        long TotalBalance();

        bool TransferTo(IAccountable fromUser, string toUser, int amount, ref string msg);
        string[] PrintAllUsersInfo();
        string[] PrintAllTransactions();

        void AddAccount(string user, string pwd);
    }
}