using NetCore.Framework.Snowflake;
using Newtonsoft.Json;
using System;

namespace SanFu.Entities
{

    /// <summary>
    /// 泛型实体基类
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public abstract class Entity<TPrimaryKey>
    {
        /// <summary>
        /// 主键
        /// </summary>
        //[JsonIgnore]
        public virtual TPrimaryKey Id { get; set; }
    }


    /// <summary>
    /// 定义默认主键类型为int的实体基类
    /// </summary>
    public abstract class Entity : Entity<long>
    {
        public Entity()
        {
            //雪花算法初始值
            //this.Id = SingletonIdWorker.GetInstance().NextId();
        }
    }
}
