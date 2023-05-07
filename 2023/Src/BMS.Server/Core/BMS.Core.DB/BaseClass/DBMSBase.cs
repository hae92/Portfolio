namespace BMS.Core.DB.BaseClass
{
    public class DBMSBase
    {
        protected string Target = string.Empty;
        protected object ConnectionLock = new object();
        protected string ConnectionString = string.Empty;
        protected int RetryCount = 0;
        protected string Address = string.Empty;
        protected string Port = string.Empty;
        protected string UserID = string.Empty;
        protected string Password = string.Empty;
    }
}
