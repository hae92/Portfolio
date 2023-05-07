using BMS.Core.DB.BaseClass;
using BMS.Core.DB.Common;
using BMS.Core.DB.Interface;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace BMS.Core.DB.Module
{
    public class OracleDB : DBMSBase, ICommandDB
    {
        #region "Properties"

        private OracleConnection _connection;
        private OracleDataReader _dataReader;

        #endregion

        #region "Constructors"

        public OracleDB() { }

        /// <summary>
        /// args[0] Target
        /// args[1] Port
        /// args[2] Password
        /// args[3] UserID
        /// </summary>
        /// <param name="args"></param>
        public OracleDB(params string[] args)
        {
            base.Target = args[0];
            base.Port = args[1];
            base.Password = args[2];
            base.UserID = args[3];

            string dataSourceFormat =
                @"(DESCRIPTION =
                    (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = {0})(PORT = {1}))
                )
                (
                    CONNECT_DATA = (SERVICE_NAME = {2}))
                )";

            string dataSource = string.Format(dataSourceFormat, Target, Port, "xe");
            base.ConnectionString = string.Format("Data Source={0};User ID={1};Password={2}", dataSource, UserID, Password);

            _connection = new OracleConnection(ConnectionString);
        }

        #endregion

        #region "Events"
        #endregion

        #region "Methods"

        public void Connect()
        {
            _connection.Open();
        }
        public void Close()
        {
            _connection.Close();
        }

        public void Init(string str)
        {
            _connection = new OracleConnection(ConnectionString);
        }

        public Boolean NonSelectQuery(string query)
        {
            try
            {
                using (var command = new OracleCommand())
                {
                    if (_dataReader != null)
                        _dataReader.Close();
                    command.Connection = _connection;
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public DBData SelectQuery(string query)
        {
            try
            {
                DataTable dt = new DataTable();
                DBData result = null;

                using (var command = new OracleCommand())
                {
                    if (_dataReader != null)
                        _dataReader.Close();
                    command.Connection = _connection;
                    command.CommandText = query;
                    _dataReader = command.ExecuteReader(); // 사용법 확인 필요

                    using (var dp = new OracleDataAdapter(command))
                    {
                        dp.Fill(dt);
                        dt.AcceptChanges();
                        result = new DBData(dt.AsEnumerable().Select(r => dt.Columns.Cast<DataColumn>().ToDictionary(c => c.ColumnName, c => r[c].ToString())).ToList());
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ConnectionState GetConnectionState()
        {
            return _connection.State;
        }
        #endregion
    }
}
