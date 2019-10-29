using System;
using System.Collections.Generic;
using System.Threading;

namespace Redis
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceStackRedis();
        }

        private static void ServiceStackRedis()
        {
            #region string操作
            //设置key
            SSRedisHelper.RedisString.Set("stringKey", "key的值", DateTime.Now.AddSeconds(5));
            SSRedisHelper.RedisString.Set("stringKey1", "key1的值");

            //循环打印stringKey
            while (true)
            {
                if (SSRedisHelper.RedisString.ContainsKey("stringKey"))
                {
                    Console.WriteLine("stringKey.键的值是:{0},时间 {1}", SSRedisHelper.RedisString.Get("stringKey"), DateTime.Now);
                    Thread.Sleep(1000);
                }
                else
                {
                    Console.WriteLine("键:StringValue,值:我已过期 {0}", DateTime.Now);
                    break;
                }
            }

            //打印stringKey1的值
            Console.WriteLine("stringKey1的值是:{0}", SSRedisHelper.RedisString.Get("stringKey1"));

            //删除stringKey1
            SSRedisHelper.RedisString.Remove("stringKey1");

            //再次打印
            Console.WriteLine("删除stringKey1后，它的值是:{0}", SSRedisHelper.RedisString.Get("stringKey1"));

            #endregion

            #region  hash
            SSRedisHelper.RedisHash.SetEntryInHash("hashTable", "h_name", "刘德华");
            SSRedisHelper.RedisHash.SetEntryInHash("hashTable", "h_age", "58");
            SSRedisHelper.RedisHash.SetEntryInHash("hashTable", "h_title", "歌手、演员");
            SSRedisHelper.RedisHash.SetEntryInHash("hashTable", "h_from", "中国香港");

            Console.WriteLine("hashTable的name值是:{0}", SSRedisHelper.RedisHash.GetValueFromHash("hashTable", "h_name"));
            
            //遍历所有的keys
            List<string> list = SSRedisHelper.RedisHash.GetHashKeys("hashTable");
            foreach (var item in list)
            {
                Console.WriteLine("hashTable的key值分别是:{0}", item);
            }
            //遍历所有的values
            List<string> list1 = SSRedisHelper.RedisHash.GetHashValues("hashTable");
            foreach (var item in list1)
            {
                Console.WriteLine("hashTable的value值分别是:{0}", item);
            }

            //删除
            SSRedisHelper.RedisHash.RemoveEntryFromHash("hashTable", "h_age");

            //再次遍历
            list = SSRedisHelper.RedisHash.GetHashKeys("hashTable");
            foreach (var item in list)
            {
                Console.WriteLine("hashTable的key值分别是:{0}", item);
            }
            #endregion

            #region list
            SSRedisHelper.RedisList.Add("list1", "列表1");
            SSRedisHelper.RedisList.Add("list1", "列表2");
            SSRedisHelper.RedisList.Add("list1", "列表3");
            SSRedisHelper.RedisList.Add("list1", "列表4");
            SSRedisHelper.RedisList.Add("list1", "列表5");

            List<string> list2 = SSRedisHelper.RedisList.Get("list1");
            foreach (var item in list2)
            {
                Console.WriteLine("List的值分别是:{0}", item);
            }

            //删除尾部数据并返回
            Console.WriteLine("List尾部被删除的数据是:{0}", SSRedisHelper.RedisList.PopItemFromList("list1"));

            #endregion

            #region 无序集合
            SSRedisHelper.RedisSet.Add("hashSet001", "小A");
            SSRedisHelper.RedisSet.Add("hashSet001", "小B");
            SSRedisHelper.RedisSet.Add("hashSet001", "小C");
            SSRedisHelper.RedisSet.Add("hashSet001", "小D");

            SSRedisHelper.RedisSet.Add("hashSet002", "小B");
            SSRedisHelper.RedisSet.Add("hashSet002", "小D");
            SSRedisHelper.RedisSet.Add("hashSet002", "小K");
            SSRedisHelper.RedisSet.Add("hashSet002", "小J");

            HashSet<string> hashset1 = SSRedisHelper.RedisSet.GetAllItemsFromSet("hashSet001");
            HashSet<string> hashset2 = SSRedisHelper.RedisSet.GetAllItemsFromSet("hashSet002");
            foreach (var item in hashset1)
            {
                Console.WriteLine("hashSet001的值分别是:{0}", item);
            }

            foreach (var item in hashset2)
            {
                Console.WriteLine("hashSet002的值分别是:{0}", item);
            }

            //交集
            
            SSRedisHelper.RedisSet.StoreIntersectFromSets("hashSet003", new string[] { "hashSet001", "hashSet002" });
            HashSet<string> hashset3 = SSRedisHelper.RedisSet.GetAllItemsFromSet("hashSet003");
            foreach (var item in hashset3)
            {
                Console.WriteLine("hashSet001和hashSet002的交集值分别是:{0}", item);
            }

            //并集
            SSRedisHelper.RedisSet.StoreUnionFromSets("hashSet004", new string[] { "hashSet001", "hashSet002" });
            HashSet<string> hashset4 = SSRedisHelper.RedisSet.GetAllItemsFromSet("hashSet004");
            foreach (var item in hashset4)
            {
                Console.WriteLine("hashSet001和hashSet002的并集值分别是:{0}", item);
            }

            //差集
            SSRedisHelper.RedisSet.StoreDifferencesFromSet("hashSet005", "hashSet001", new string[] { "hashSet002" });
            HashSet<string> hashset5 = SSRedisHelper.RedisSet.GetAllItemsFromSet("hashSet005");
            foreach (var item in hashset5)
            {
                Console.WriteLine("hashSet001和hashSet002的差集值分别是:{0}", item);
            }

            #endregion

            #region 有序集合
            SSRedisHelper.RedisZSet.AddItemToSortedSet("hashSet006", "1.刘德华");
            SSRedisHelper.RedisZSet.AddItemToSortedSet("hashSet006", "2.刘青云");
            SSRedisHelper.RedisZSet.AddItemToSortedSet("hashSet006", "3.周华健");
            SSRedisHelper.RedisZSet.AddItemToSortedSet("hashSet006", "4.风清扬");

            List<string> list3 = SSRedisHelper.RedisZSet.GetAllItemsFromSortedSet("hashSet006");
            foreach (var item in list3)
            {
                Console.WriteLine("hashSet006值分别是:{0}", item);
            }

            #endregion

            Console.ReadKey();
        }
    }
}
