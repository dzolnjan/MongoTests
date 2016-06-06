using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Id 
    {
        public string Value { get; private set; }

        public Id(string value)
        {
            Value = value;
        }

        public static implicit operator string(Id id)
        {
            return id.Value;
        }

        public static Id Generate()
        {
            return new Id(ObjectIdHelper.GenerateNewId().ToString());
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
