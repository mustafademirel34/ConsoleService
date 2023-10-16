using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTesting
{
    public class Guard
    {

        //okunabilir olması için yapmak gereklir.
        public dynamic RedisPort//static olmasa da söylüyor güzellll
        {
            get { return _redisport; }
            set { _redisport = (dynamic)value; }//aldığın şeyi aynen kaydet demişti ben dynamic yaptım, dolayısıyla burası genel oldu
        }

        private static string _redisport =
            "redis-10246.c93.us-east-1-3.ec2.cloud.redislabs.com:10246," +
            "password=oo8RjNJg8Khj8KfMhXy7UnnWQ3Yj9c8S";
    }

}
