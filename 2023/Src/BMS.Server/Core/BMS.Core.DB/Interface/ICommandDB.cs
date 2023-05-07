using System.Data;
using BMS.Core.DB.Common;

namespace BMS.Core.DB.Interface
{
    public interface ICommandDB
    {
        void Init(string str);
        ConnectionState GetConnectionState();
        void Connect();
        void Close();
        Boolean NonSelectQuery(string sql);
        DBData SelectQuery(string sql);
    }
}
