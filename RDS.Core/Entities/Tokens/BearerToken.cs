using RDS.Core.Entities.Base;
using System;

namespace RDS.Core.Entities.Tokens
{
    public class BearerToken : BaseEntity
    {
        public string AccToken { get; set; }

        public string RefToken { get; set; }

        public int StaffId { get; set; }

        public DateTime RefTokenExpAt { get; set; }
    }
}
