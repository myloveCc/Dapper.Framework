using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;

namespace NETCore.SQLKit
{
    internal static class EntityExtensions
    {
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, string> TypeTableName = new ConcurrentDictionary<RuntimeTypeHandle, string>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> KeyProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> ExplicitKeyProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> TypeProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> ComputedProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();

        /// <summary>
        /// 获取表明
        /// </summary>
        /// <param name="type">实体类型<see cref="Type"/></param>
        /// <returns></returns>
        internal static string GetTableName(this Type type, ISqlParser sqlParser)
        {
            string name;
            if (TypeTableName.TryGetValue(type.TypeHandle, out name)) return name;

            //NOTE: This as dynamic trick should be able to handle both our own Table-attribute as well as the one in EntityFramework 
            var tableAttr = type.GetTypeInfo().GetCustomAttributes(false).SingleOrDefault(attr => attr.GetType().Name == "TableAttribute") as dynamic;
            if (tableAttr != null)
                name = $"{sqlParser.ElementLeftPrefix}{tableAttr.Name}{sqlParser.ElementRightPrefix}";
            else
            {
                name = $"{sqlParser.ElementLeftPrefix}{type.Name}s{sqlParser.ElementRightPrefix}";
                if (type.GetTypeInfo().IsInterface && name.StartsWith("I"))
                    name = $"{sqlParser.ElementRightPrefix}{name.Substring(1)}{sqlParser.ElementRightPrefix}";
            }

            TypeTableName[type.TypeHandle] = name;
            return name;
        }

        /// <summary>
        /// 检查属性是否为自增属性
        /// </summary>
        /// <param name="info">属性信息<see cref="PropertyInfo"/></param>
        /// <param name="type">实体类型<see cref="Type"/></param>
        /// <returns></returns>
        internal static bool IsIdentity(this PropertyInfo info, Type type)
        {
            bool result = false;
            var identityProperties = KeyPropertiesCache(type);

            if (identityProperties.Contains(info))
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 检查属性是否为数据列
        /// </summary>
        /// <param name="info">属性信息<see cref="PropertyInfo"/></param>
        /// <param name="type">实体类型<see cref="Type"/></param>
        /// <returns></returns>
        internal static bool IsNotColumn(this PropertyInfo info, Type type)
        {
            bool result = false;

            //全部属性
            var allProperties = TypePropertiesCache(type);

            //不包含此属性
            if (!allProperties.Contains(info))
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 获取实体类型的全部属性
        /// </summary>
        /// <param name="type">实体类型<see cref="Type"/></param>
        /// <returns></returns>
        internal static List<PropertyInfo> GetColumnPorperties(this Type type)
        {
            var properties = TypePropertiesCache(type);
            return properties;
        }

        //计算属性缓存
        private static List<PropertyInfo> ComputedPropertiesCache(Type type)
        {
            IEnumerable<PropertyInfo> pi;
            if (ComputedProperties.TryGetValue(type.TypeHandle, out pi))
            {
                return pi.ToList();
            }

            var computedProperties = TypePropertiesCache(type).Where(p => p.GetCustomAttributes(true).Any(a => a is ComputedAttribute)).ToList();

            ComputedProperties[type.TypeHandle] = computedProperties;
            return computedProperties;
        }

        //非自增主键缓存
        private static List<PropertyInfo> ExplicitKeyPropertiesCache(Type type)
        {
            IEnumerable<PropertyInfo> pi;
            if (ExplicitKeyProperties.TryGetValue(type.TypeHandle, out pi))
            {
                return pi.ToList();
            }

            var explicitKeyProperties = TypePropertiesCache(type).Where(p => p.GetCustomAttributes(true).Any(a => a is ExplicitKeyAttribute)).ToList();

            ExplicitKeyProperties[type.TypeHandle] = explicitKeyProperties;
            return explicitKeyProperties;
        }

        //自增主键缓存
        private static List<PropertyInfo> KeyPropertiesCache(Type type)
        {
            IEnumerable<PropertyInfo> pi;
            if (KeyProperties.TryGetValue(type.TypeHandle, out pi))
            {
                return pi.ToList();
            }

            var allProperties = TypePropertiesCache(type);
            var keyProperties = allProperties.Where(p =>
            {
                return p.GetCustomAttributes(true).Any(a => a is KeyAttribute);
            }).ToList();

            if (keyProperties.Count == 0)
            {
                //如果是以Id命名的属性，且不为非自增主键，则默认为自增列
                var idProp = allProperties.FirstOrDefault(p => p.Name.ToLower() == "id");
                if (idProp != null && !idProp.GetCustomAttributes(true).Any(a => a is ExplicitKeyAttribute))
                {
                    keyProperties.Add(idProp);
                }
            }

            KeyProperties[type.TypeHandle] = keyProperties;
            return keyProperties;
        }

        //全部数据列属性缓存
        private static List<PropertyInfo> TypePropertiesCache(Type type)
        {
            IEnumerable<PropertyInfo> pis;
            if (TypeProperties.TryGetValue(type.TypeHandle, out pis))
            {
                return pis.ToList();
            }

            var properties = type.GetProperties().Where(IsWriteable).ToArray();
            TypeProperties[type.TypeHandle] = properties;
            return properties.ToList();
        }

        //判断属性是否为数据列
        private static bool IsWriteable(PropertyInfo pi)
        {
            var attributes = pi.GetCustomAttributes(typeof(WriteAttribute), false).AsList();
            if (attributes.Count != 1) return true;

            var writeAttribute = (WriteAttribute)attributes[0];
            return writeAttribute.Write;
        }

        //获取唯一主键（Key/ExplicitKey只能有一个）
        private static PropertyInfo GetSingleKey<T>(string method)
        {
            var type = typeof(T);
            var keys = KeyPropertiesCache(type);
            var explicitKeys = ExplicitKeyPropertiesCache(type);
            var keyCount = keys.Count + explicitKeys.Count;
            if (keyCount > 1)
                throw new Exception($"{method}<T> only supports an entity with a single [Key] or [ExplicitKey] property");
            if (keyCount == 0)
                throw new Exception($"{method}<T> only supports an entity with a [Key] or an [ExplicitKey] property");

            return keys.Any() ? keys.First() : explicitKeys.First();
        }
    }
}
