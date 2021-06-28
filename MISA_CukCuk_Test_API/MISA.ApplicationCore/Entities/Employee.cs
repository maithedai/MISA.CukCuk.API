using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Entities
{
    public class Employee
    {
        /// <summary>
        /// Thông tin nhân viên
        /// </summary>
        /// CreatedBy: MTDAI 28.06.2021
        public Guid EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public int? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string IdentityNumber { get; set; }
        public string Salary { get; set; }
        public int?  WorkStatus { get; set; }
        public int? MyProperty { get; set; }
        //public string DepartmentId { get; set; }
        public string GenderName { get; set; }
        public string WorkStatusName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        //public string PositionId { get; set; }
    }
}
