using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Framework.Cache
{
    public sealed class CacheManager
    {
        public CacheManager() { }
        public static ICacheHelper _Cache;
        /// <summary>
        /// 判断缓存是否存在
        /// </summary>
        /// <typeparam name="CacheType">缓存类型</typeparam>
        /// <param name="key">键名</param>
        /// <returns></returns>
        public bool Exists<CacheType>(string key) where CacheType : ICacheHelper, new()
        {
            if (_Cache != null && typeof(CacheType).Equals(_Cache.GetType()))
                return _Cache.Exists(key);
            else
            {
                _Cache = new CacheType();
                return _Cache.Exists(key);
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">转换的类</typeparam>
        /// <typeparam name="CacheType">缓存类型</typeparam>
        /// <param name="key">键名</param>
        /// <returns>转换为T类型的值</returns>
        public T GetCache<T, CacheType>(string key)
            where T : class
            where CacheType : ICacheHelper, new()
        {
            if (_Cache != null && typeof(CacheType).Equals(_Cache.GetType()))
                return _Cache.GetCache<T>(key);
            else
            {
                _Cache = new CacheType();
                return _Cache.GetCache<T>(key);
            }
        }

        /// <summary>
        /// 保存缓存
        /// </summary>
        /// <typeparam name="CacheType">缓存类型</typeparam>
        /// <param name="key">键名</param>
        /// <param name="value">值</param>
        public void Save<CacheType>(string key, object value) where CacheType : ICacheHelper, new()
        {
            if (_Cache != null && typeof(CacheType).Equals(_Cache.GetType()))
                _Cache.SetCache(key, value);
            else
            {
                _Cache = new CacheType();
                _Cache.SetCache(key, value);
            }
        }

        /// <summary>
        /// 保存缓存并设置绝对过期时间
        /// </summary>
        /// <typeparam name="CacheType">缓存类型</typeparam>
        /// <param name="key">键名</param>
        /// <param name="value">值</param>
        /// <param name="expiressAbsoulte">绝对过期时间</param>
        public void Save<CacheType>(string key, object value, DateTimeOffset expiressAbsoulte) where CacheType : ICacheHelper, new()
        {
            if (_Cache != null && typeof(CacheType).Equals(_Cache.GetType()))
                _Cache.SetCache(key, value, expiressAbsoulte);
            else
            {
                _Cache = new CacheType();
                _Cache.SetCache(key, value, expiressAbsoulte);
            }
        }

        /// <summary>
        /// 保存滑动缓存
        /// </summary>
        /// <typeparam name="CacheType">只能用memorycache，redis暂不实现</typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="t">间隔时间</param>
        public void SaveSlidingCache<CacheType>(string key, object value, TimeSpan t) where CacheType : MemoryCacheHelper, new()
        {
            if (_Cache != null && typeof(CacheType).Equals(_Cache.GetType()))
                _Cache.SetSlidingCache(key, value, t);
            else
            {
                _Cache = new CacheType();
                _Cache.SetSlidingCache(key, value, t);
            }
        }

        /// <summary>
        /// 删除一个缓存
        /// </summary>
        /// <typeparam name="CacheType">缓存类型</typeparam>
        /// <param name="key">要删除的key</param>
        public void Delete<CacheType>(string key) where CacheType : ICacheHelper, new()
        {
            if (_Cache != null && typeof(CacheType).Equals(_Cache.GetType()))
                _Cache.RemoveCache(key);
            else
            {
                _Cache = new CacheType();
                _Cache.RemoveCache(key);
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        /// <typeparam name="CacheType"></typeparam>
        public void Dispose<CacheType>() where CacheType : ICacheHelper, new()
        {
            if (_Cache != null && typeof(CacheType).Equals(_Cache.GetType()))
                _Cache.Dispose();
            else
            {
                _Cache = new CacheType();
                _Cache.Dispose();
            }
        }

        public void GetMessage<CacheType>() where CacheType : RedisCacheHelper, new()
        {
            if (_Cache != null && typeof(CacheType).Equals(_Cache.GetType()))
                _Cache.GetMssages();
            else
            {
                _Cache = new CacheType();
                _Cache.GetMssages();
            }
        }

        public void Publish<CacheType>(string msg) where CacheType : RedisCacheHelper, new()
        {
            if (_Cache != null && typeof(CacheType).Equals(_Cache.GetType()))
                _Cache.Publish(msg);
            else
            {
                _Cache = new CacheType();
                _Cache.Publish(msg);
            }
        }
    }
}
