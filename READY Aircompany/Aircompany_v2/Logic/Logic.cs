using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    public abstract class Logic : ICrudErrorable
    {
       public virtual event EventHandler<Exception> InsertionError;
       public virtual event EventHandler<Exception> SelectionError;
       public virtual event EventHandler<Exception> DeleteError;
       public virtual event EventHandler<Exception> EditError;
       public virtual event EventHandler<Exception> AllSelectedError;

       
    }
}
