using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Model : ModelBase
    {
        public string SomeProp { get; set; }

        public IdGuid IdGuidProp { get; set; }
    }

    public abstract class ModelBase
    {
        protected ModelBase()
        {
            Id = Id.Generate();
            DateCreated = DateTime.UtcNow;
            DateUpdated = DateCreated;
        }

        public Id Id { get; set; }

        public DateTime DateCreated{ get; set; }

        public DateTime DateUpdated { get; set; }

        public int DontPutMeInDb { get; set; }
    }
}
