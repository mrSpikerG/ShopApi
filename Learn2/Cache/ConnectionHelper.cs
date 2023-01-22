using StackExchange.Redis;

namespace Learn2.Cache
{
    public class ConnectionHelper
    {
        private static Lazy<ConnectionMultiplexer> _lazyConnection;
        public static ConnectionMultiplexer Connection
        {
            get { return _lazyConnection.Value; }
        }

        static ConnectionHelper()
        {
            _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(ConfigManager.AppSetting["RedisUrl"]);
            });
        }

    }
}
