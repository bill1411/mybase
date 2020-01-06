using System;
using System.Collections.Generic;
using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SwaggerCoreApi.Extensions
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class SwaggerExtension
    {
        /// <summary>
        /// 添加控制器swagger扩展类
        /// </summary>
        public class ApplyTagDescriptions : IDocumentFilter
        {
            /// <summary>
            /// swagger汉化标签
            /// </summary>
            /// <param name="swaggerDoc"></param>
            /// <param name="context"></param>
            public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
            {
                //Name就是接口的大类   Description就是关于该类的描述
                swaggerDoc.Tags = new List<Tag>
                {
                    new Tag { Name = "Values", Description = "企业相关接口" },
                    //new Tag { Name = "Ticket", Description = "机票相关接口" }
                };
            }
        }
    }

}
