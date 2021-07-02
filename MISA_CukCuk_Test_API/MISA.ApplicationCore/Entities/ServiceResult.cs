using MISA.ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.ApplicationCore.Entities
{
    public class ServiceResult
    {
        public object Data { get; set; }
        public string Messenger { get; set; }
        public MISACode MISACode { get; set; }
        public static ServiceResult GetResult(MISACode MISACode, string messenger, object data = default(object))
        {
            return new ServiceResult
            {
                MISACode = MISACode,
                Messenger = messenger,
                Data = data
            };
        }
    }
}
