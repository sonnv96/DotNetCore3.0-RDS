using RDS.Core.Entities.Base;
using System;

namespace RDS.Core.Entities.Tokens
{
    public class BearerToken : IBaseEntity
    {
        public int Id { get; set; }
        public string AccToken { get; set; }

        public string RefToken { get; set; }

        public int UserId { get; set; }

        public DateTime RefTokenExpAt { get; set; }
    }
}
