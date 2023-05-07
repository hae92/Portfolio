using BMS.Core.DB.BaseClass;
using BMS.Core.DB.Interface;
using BMS.Core.Common;
using System.Data;

namespace BMS.Core.DB
{
    public class DB
    {
        #region "Properties"
        private const string _namespaceDB = "Module";
        private Dictionary<string, DBMSBase> _databases = new Dictionary<string, DBMSBase>();
        private Dictionary<string, Type> _classes = new Dictionary<string, Type>();
        private string _currentDatabaseKey = string.Empty;
        private ClassCollector _collector = default(ClassCollector);

        #endregion

        #region "Constructors"

        public DB()
        {
            _collector = Singleton<ClassCollector>.Instance;
            GetClassInformation();
        }

        #endregion

        #region "Events"
        #endregion

        #region "Methods"

        public void GetClassInformation()
        {
            var func = _collector.GetExecutingAssembly();
            var currentNamespace = func.Invoke().GetName().Name;
            var targetNamespace = string.Format("{0}.{1}", currentNamespace, _namespaceDB);
            _classes = _collector.GetClasses(targetNamespace, func.Invoke().GetTypes());
        }

        public bool AddDatabase(string databaseType, string databaseIP, string databasePort, string databasePassword, string databaseUserID)
        {
            try
            {
                if (_databases.ContainsKey(databaseType))
                {
                    // 중복 등록에 대한 허용을 할 것인가?
                    // 1개의 Application에서 2개의 Oracle Server에 접속해야 할 경우 -> Key값 수정이 필요하다.
                    // Result True
                }
                else
                {
                    _databases.Add(databaseType, (DBMSBase)Activator.CreateInstance(_classes[databaseType], databaseIP, databasePort, databasePassword, databaseUserID));
                }

                _currentDatabaseKey = databaseType;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public ICommandDB CurrentDB()
        {
            try
            {
                if (_databases.ContainsKey(_currentDatabaseKey))
                {
                    return (ICommandDB)_databases[_currentDatabaseKey];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ICommandDB GetDatabase(string key)
        {
            try
            {
                if (_databases.ContainsKey(key))
                {
                    return (ICommandDB)_databases[key];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ICollection<string> GetDatabase()
        {
            return _databases.Keys;
        }

        public bool DelDatabase(string key)
        {
            try
            {
                if (_databases.ContainsKey(key))
                {
                    var db = (ICommandDB)_databases[key];

                    // Connection 연결상태 확인 후 진행
                    if (db.GetConnectionState() == ConnectionState.Open)
                    {
                        db.Close();
                        if (db.GetConnectionState() == ConnectionState.Closed)
                        {
                            _databases.Remove(key);
                            if (_databases.ContainsKey(key) == false)
                            {
                                // 정상 Return True
                                return true;
                            }
                            else
                            {
                                // Remove 함수 호출 후 Key 값이 존재할 경우 Return False
                                return false;
                            }
                        }
                        else
                        {
                            // Connection Close 실패 Return False
                            return false;
                        }
                    }
                    else
                    {
                        _databases.Remove(key);
                        if (_databases.ContainsKey(key) == false)
                        {
                            // 정상
                            return true;
                        }
                        else
                        {
                            // Remove 함수 호출 후 Key 값이 존재할 경우 Return False
                            return false;
                        }
                    }
                }
                else
                {
                    // Key값 없을 경우 삭제로 판단 Return True
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Exception 발생 시 Return False
                return false;
            }
        }

        public void ClearDatabase()
        {
            try
            {
                foreach (ICommandDB db in _databases.Values)
                {
                    if (db.GetConnectionState() == System.Data.ConnectionState.Open)
                    {
                        db.Close(); // 연결중인 모든 Interface를 종료한다.
                    }
                }

                _databases.Clear();
            }
            catch (Exception ex)
            {

            }
        }

        public int DatabaseCount
        {
            get
            {
                return _databases.Count;
            }
        }

        #endregion
    }
}