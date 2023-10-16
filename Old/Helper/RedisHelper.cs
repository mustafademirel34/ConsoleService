using Newtonsoft.Json;  // Newtonsoft.Json kütüphanesini kullanabilmek için gereken using ifadesi.
using StackExchange.Redis;  // StackExchange.Redis kütüphanesini kullanabilmek için gereken using ifadesi.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTesting.Helper
{
    class RedisControl
    {
        public RedisControl()
        {
            // Redis sunucusuna bağlantı dizesi ve kimlik doğrulama bilgileri.
            string redisConnectionString =
                "redis-10246.c93.us-east-1-3.ec2.cloud.redislabs.com:10246," +
                "password=oo8RjNJg8Khj8KfMhXy7UnnWQ3Yj9c8S";

            // RedisHelper sınıfından bir örnek oluşturarak bağlantıyı kurar.
          ////  var redisHelper = new RedisHelper(redisConnectionString);

          //  // Veri ekleme
          //  redisHelper.Set("myKey", "myValue");

          //  // Veri alma
          //  string value = redisHelper.Get<string>("myKey");

          //  // Anahtar var mı kontrolü
          //  bool exists = redisHelper.Exists("myKey");

          //  // Anahtar silme
          //  redisHelper.Remove("myKey");

          //  // Belirli bir desene sahip anahtarları silme
          //  redisHelper.RemoveByPattern("myPattern");

          //  // Tüm anahtarları alma
          //  List<string> allKeys = redisHelper.GetAllKeys();
        }
    }

    public class RedisHelper
    {
        public RedisHelper()
        {
           // Console.WriteLine("selam");
            //CONSTRUCTOR OLMAK ZORNDA?--------------------- INFERENCE İÇİN------------------
        }

        private readonly ConnectionMultiplexer _connectionMultiplexer;  // Redis sunucusuna bağlantıyı yöneten nesne.
        private readonly IDatabase _database;  // Redis veritabanı işlemleri için kullanılır.

        //public RedisHelper(string redisConnectionString)
        //{
        //    // Bağlantıyı kurmak için ConnectionMultiplexer kullanılır.
        //    _connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
        //    //TRY KULLANILMASI YARARLI OLABİLİR. ŞİFRE YANLIŞ GİBİ DURUMLARDA HATA VERİYOR KULLANICIYA BİLGİ VERİLMELİ

        //    // Veritabanı işlemleri için ilgili IDatabase nesnesi alınır.
        //    _database = _connectionMultiplexer.GetDatabase();
        //}

        public void Set<T>(string key, T value, TimeSpan? expiry = null)
        {
            // Değerin JSON formatına dönüştürülmesi.
            var serializedValue = JsonConvert.SerializeObject(value);

            // Redis veritabanına anahtar ve değer eklenir. Varsa süre verilebilir.
            _database.StringSet(key, serializedValue, expiry);
        }

        public T Get<T>(string key)
        {
            // Redis veritabanından anahtarın değeri alınır.
            var serializedValue = _database.StringGet(key);

            // Değer boşsa varsayılan değer döndürülür.
            if (serializedValue.IsNullOrEmpty)
            {
                return default(T);
            }

            // JSON formatındaki değeri ilgili tipte nesneye dönüştürme.
            return JsonConvert.DeserializeObject<T>(serializedValue);
        }

        public bool Exists(string key)
        {
            // Belirtilen anahtarın var olup olmadığını kontrol eder.
            return _database.KeyExists(key);
        }

        public void Remove(string key)
        {
            // Belirtilen anahtarı Redis veritabanından siler.
            _database.KeyDelete(key);
        }

        public void RemoveByPattern(string pattern)
        {
            // Belirli bir desene sahip anahtarları siler.
            var keysToRemove = new List<RedisKey>();

            foreach (var endPoint in _connectionMultiplexer.GetEndPoints())
            {
                var server = _connectionMultiplexer.GetServer(endPoint);
                var keys = server.Keys(pattern: pattern + "*");

                foreach (var key in keys)
                {
                    keysToRemove.Add(key);
                }
            }

            _database.KeyDelete(keysToRemove.ToArray());
        }

        public List<string> GetAllKeys()
        {
            // Tüm anahtarları Redis sunucusundan alır.
            var server = GetServer();
            var keys = server.Keys(database: _database.Database);

            // Anahtarları string listesine çevirip döndürür.
            return keys.Select(k => k.ToString()).ToList();
        }

        private IServer GetServer()
        {
            // Redis sunucusunun adresini alarak ilgili IServer nesnesini döndürür.
            var endPoint = _connectionMultiplexer.GetEndPoints().FirstOrDefault();
            if (endPoint != null)
            {
                return _connectionMultiplexer.GetServer(endPoint);
            }
            throw new Exception("No Redis endpoints available.");
        }

        public static int Dispose(int nesne)
        {
           return nesne + 50;
        }

    }
}
