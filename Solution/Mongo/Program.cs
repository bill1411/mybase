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

            //插入一条记录
            Student stu = new Student();
            stu.age = 18;
            stu.name = "刘德华";
            stu.sex = 1;
            stu.stu_no = "00002";

            //int result = db.Add<Student>(stu);
            //if (result > 0) Console.WriteLine("添加成功");
            //else Console.WriteLine("添加失败");

            //返回所有数据
            var filter = Builders<Student>
                .Filter.Eq("_id", "ObjectId(\"5db93f4e8876f0354862189f\")");
            List<Student> list = list = db.FindList<Student>(filter);
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    Console.WriteLine("学生姓名是:{0}",item.name);
                }
            }


            //查找一条记录
            stu = db.FindOne<Student>("ObjectId(\"5db93f4e8876f0354862189f\")");
            if (stu != null)
            {
                Console.WriteLine("学生姓名是：{0}", stu.name);
                Console.WriteLine("学生年龄是：{0}",stu.age);
            }
            Console.ReadKey();

        }

        public class Student
        {
            public string name { get; set; }
            public int sex { get; set; }
            public int age { get; set; }
            public string stu_no { get; set; }
        }
    }
}
