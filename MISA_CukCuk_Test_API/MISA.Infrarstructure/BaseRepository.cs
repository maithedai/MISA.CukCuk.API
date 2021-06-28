using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.ApplicationCore.Interfaces;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MISA.Infrarstructure
{
    class BaseRepository<TEntity> : IBaseRepository<TEntity>
    {
        #region DECLARE
        IConfiguration _configuration;
        string _connectionString = string.Empty;
        IDbConnection _dbConnection = null;
        string _tableName;
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

        public int Update(TEntity employee)
        {
            throw new NotImplementedException();
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
                else
                {
                    parmaster.Add($"{propertyName}", propertyValue);
                }
            }
            return parmaster;
        }
    }
}
