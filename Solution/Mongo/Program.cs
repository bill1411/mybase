using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Mongo
{
    class Program
    {
        static void Main(string[] args)
        {
            DoMongo();
        }


        private static void DoMongo()
        {
            //初始化，传入要操作的集合名称
            MongoDBHelper db = new MongoDBHelper("Student");
            Student stu = new Student();

            //检查有没有相同的数据
            var filter1 = Builders<Student>
                .Filter.Eq("stu_no", "00002");
            if (!db.CheckData<Student>(filter1))
            {
                //插入一条记录
                #region 组装
                stu.age = 18;
                stu.name = "刘德华";
                stu.sex = 1;
                stu.stu_no = "00002";
                #endregion

                int result = db.Add<Student>(stu);
                if (result > 0) Console.WriteLine("添加成功");
                else Console.WriteLine("添加失败");
            }


            //查找一条记录
            Console.WriteLine("=========查找数据============");
            string _id = "5db93f4e8876f0354862189f";
            stu = db.FindOne<Student>(_id,true,null);
            if (stu != null)
            {
                Console.WriteLine("学生姓名是：{0}", stu.name);
                Console.WriteLine("学生年龄是：{0}",stu.age);
            }

            //更新一条数据
            Console.WriteLine("=========更新数据============");
            stu.name = "我的名字原来叫小明";
            db.Update<Student>(stu, _id, true);

            Console.WriteLine("=========查找更新后的数据============");
            var filter = Builders<Student>
                .Filter.Eq("_id", new ObjectId("5db93f4e8876f0354862189f"));
            List<Student> list = db.FindList<Student>(filter);
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    Console.WriteLine("更改后学生名字叫:{0}", item.name);
                }
            }

            Console.ReadKey();

        }

        [BsonIgnoreExtraElements]
        public class Student
        {
            public string name { get; set; }
            public int sex { get; set; }
            public int age { get; set; }
            public string stu_no { get; set; }
        }
    }
}
