using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Enums
{
    /// <summary>
    /// MISACode để xác định trạng thái của Validate
    /// </summary>
    public enum MISACode
    {
        /// <summary>
        /// Dữ liệu hợp lệ
        /// </summary>
        IsValid = 100,
        /// <summary>
        /// Dữ liệu không hợp lệ
        /// </summary>
        NotValid = 900,
        /// <summary>
        /// thành công
        /// </summary>
        Success = 200,    
    }
    /// <summary>
    /// Xác định trạng thái của object
    /// </summary>
    public enum EntityState
    { 
        AddNew = 1,
        Update = 2,
        Delete = 3, 
    }
}
