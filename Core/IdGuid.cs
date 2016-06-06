using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class IdGuid
    {
        public Guid Value { get; private set; }

        public IdGuid(Guid value)
        {
            Value = value;
        }

        public static implicit operator Guid(IdGuid id)
        {
            return id.Value;
        }

        public static IdGuid Generate()
        {
            return new IdGuid(Guid.NewGuid());
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
