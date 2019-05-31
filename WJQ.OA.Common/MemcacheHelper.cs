using Memcached.ClientLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJQ.OA.Common
{
    public class MemcacheHelper
    {
        private static readonly MemcachedClient mc = null;
        static MemcacheHelper()
        {
            string[] serverlist = { "172.16.53.61:11211", "10.0.0.132:11211" };

            //初始化池
            SockIOPool pool = SockIOPool.GetInstance();
            pool.SetServers(serverlist);

            pool.InitConnections = 3;
            pool.MinConnections = 3;
            pool.MaxConnections = 5;

            pool.SocketConnectTimeout = 1000;
            pool.SocketTimeout = 3000;

            pool.MaintenanceSleep = 30;
            pool.Failover = true;

            pool.Nagle = false;
            pool.Initialize();

            // 获得客户端实例
            mc = new MemcachedClient();
            mc.EnableCompression = false;
        }
        public static bool Set(string key,object value)
        {
           return mc.Set(key, value);
        }
        public static bool Set(string key, object value,DateTime time)
        {
            return mc.Set(key, value,time);
        }
        public static object Get(string key)
        {
            return mc.Get(key);
        }
        public static bool Delete(string key)
        {
            if (mc.KeyExists(key))
            {
                return mc.Delete(key);
            }
            return false;
        }
    }
}
