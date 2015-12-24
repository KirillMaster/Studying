using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    interface ICrudErrorable
    {
         event EventHandler<Exception> InsertionError;
         event EventHandler<Exception> SelectionError;
         event EventHandler<Exception> DeleteError;
         event EventHandler<Exception> EditError;
         event EventHandler<Exception> AllSelectedError;
    }
}
