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
    }

    public abstract class ModelBase
    {
        public Id Id { get; set; }

        protected ModelBase()
        {
            Id = Id.Generate();
        }

        public int DontPutMeInDb { get; set; }
    }
}
