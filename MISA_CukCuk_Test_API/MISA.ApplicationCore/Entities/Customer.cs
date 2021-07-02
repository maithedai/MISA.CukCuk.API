using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Entities
{
    /// <summary>
    /// Khách hàng
    /// </summary>
    /// CreateBy: MTDAI 25.06.2021
    public class Customer:BaseEntity
    {
         #region Property
        /// <summary>
        /// Khóa chính
        /// </summary>
        /// 
        [PrimaryKey]
        public Guid CustomerId { get; set; }
        [ColumnName("Mã khách hàng (*)")]

        public string CustomerCode { get; set; }
        [ColumnName("Tên khách hàng (*)")]
        public string FullName { get; set; }
        [ColumnName("Ngày sinh")]
        public DateTime? DateOfBirth { get; set; }
        [ColumnName("Địa chỉ")]
        public string Adress { get; set; }
        [ColumnName("Số điện thoại")]
        public string PhoneNumber { get; set; }
        public int? Gender { get; set; }
        [ColumnName("Email")]
        public string Email { get; set; }
        public Guid? CustomerGroupId { get; set; }
        public string MemberCardCode { get; set; }
        [ColumnName("Ghi chú")]
        public string Note { get; set; }
        [ColumnName("Tên công ty")]
        public string CompanyName { get; set; }
        [ColumnName("Mã số thuế")]
        public string CompanyTaxCode { get; set; }

        #endregion

    }
}
