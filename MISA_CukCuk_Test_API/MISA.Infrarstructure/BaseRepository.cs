using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Enums;
using MISA.ApplicationCore.Interfaces;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MISA.Infrarstructure
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity: BaseEntity
    {
        #region DECLARE
        IConfiguration _configuration;
        string _connectionString = string.Empty;
        protected IDbConnection _dbConnection = null;
        protected string _tableName;
        #endregion

        public BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("MISACukCukConnectionString");
            _dbConnection = new MySqlConnection(_connectionString);
            _tableName = typeof(TEntity).Name;
        }

        public int Add(TEntity entity)
        {
            //Khởi tạo kết nối với db:
            var parmaster = MappingDbType(entity);

            //Thực thi command Text
            var rowAffects = _dbConnection.Execute($"Proc_Insert{_tableName}", parmaster, commandType: CommandType.StoredProcedure);

            //Trả về số bản ghi thêm mới đc
            return rowAffects;
        }

        public int Delete(Guid employeeId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetEntities()
        {
            //Kết nối tới CSDL:
            //Khởi tạo các commandType:
            var entities = _dbConnection.Query<TEntity>($"Proc_Get{_tableName}s", commandType: CommandType.StoredProcedure);
            //Trả về dữ liệu
            return entities;
        }

        public TEntity GetEntityById(Guid employeeId)
        {
            throw new NotImplementedException();
        }

        public int Update(TEntity entity)
        {
            //Khởi tạo kết nối với db:
            var parmaster = MappingDbType(entity);

            //Thực thi command Text
            var rowAffects = _dbConnection.Execute($"Proc_Update{_tableName}", parmaster, commandType: CommandType.StoredProcedure);

            //Trả về số bản ghi thêm mới đc
            return rowAffects;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        private DynamicParameters MappingDbType(TEntity entity)
        {
            var properties = entity.GetType().GetProperties();
            var parmaster = new DynamicParameters();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(entity);
                var propertyType = property.PropertyType;
                if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                {
                    parmaster.Add($"{propertyName}", propertyValue, DbType.String);
                }
                else if (propertyType == typeof(bool) || propertyType == typeof(bool?))
                {
                    var dbValue = ((bool)propertyValue == true ? 1: 0);
                    parmaster.Add($"@{propertyName}", dbValue, DbType.Int32);
                }
                else
                {
                    parmaster.Add($"{propertyName}", propertyValue);
                }
            }
            return parmaster;
        }

        public TEntity GetEntityByProperty(TEntity entity, PropertyInfo property)
        { 
            var propertyName = property.Name;
            var propertyValue = property.GetValue(entity);
            var keyValue = entity.GetType().GetProperty($"{_tableName}Id").GetValue(entity);
            var query = string.Empty;
            if (entity.EntityState == EntityState.AddNew)
                query = $"SELECT * FROM {_tableName} WHERE {propertyName} = '{propertyValue}'";
            else if (entity.EntityState == EntityState.Update)
                query = $"SELECT * FROM {_tableName} WHERE {propertyName} = '{propertyValue}' AND {_tableName}Id <> '{keyValue}'";
            else
                return null;
            var entityReturn = _dbConnection.Query<TEntity>(query,commandType: CommandType.Text).FirstOrDefault();
            return entityReturn;
        }
	}
}