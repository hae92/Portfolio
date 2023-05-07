using System.Reflection;

namespace BMS.Core.Common
{
    public class ClassCollector : IDisposable
    {
        public ClassCollector ()
        {

        }

        public Func<Assembly> GetExecutingAssembly()
        {
            try
            {
                return Assembly.GetExecutingAssembly;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Dictionary<string, Type> GetClasses(string nameSpace, Type[] list)
        {
            Dictionary<string, Type> classes = new Dictionary<string, Type>();
            for (int index = 0; index < list.Length; index++)
            {
                if (list[index].Namespace == nameSpace)
                {
                    if (classes.ContainsKey(list[index].Name) == false)
                    {
                        classes.Add(list[index].Name, list[index]);
                    }
                }
            }
            return classes;
        }

        public void Dispose()
        {
            //this.Dispose();
        }
    }
}