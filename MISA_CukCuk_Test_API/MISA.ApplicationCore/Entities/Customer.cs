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
        
        /// <summary>
        /// Mã khách hàng
        /// </summary>        
        [CheckDuplicate]
        [Required]
        [DisplayName("Mã khách hàng")]
        public string CustomerCode { get; set; }

        /// <summary>
        /// Họ và tên
        /// </summary>
        /// 
        [DisplayName("Tên khách hàng")]
        public string FullName { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Aderss { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        public int? Gender { get; set; }      

        /// <summary>
        /// Địa chỉ Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Nhóm khách hàng
        /// </summary>
        public Guid? CustomerGroupId { get; set; }

        public string MemberCardCode { get; set; }

        public string Note { get; set; }

        public string CompanyName { get; set; }

        public string CompanyTaxCode { get; set; }

        #endregion

    }
}
