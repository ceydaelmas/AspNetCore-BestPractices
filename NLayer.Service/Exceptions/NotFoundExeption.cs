using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.Exceptions
{
    public class NotFoundExeption : Exception
    {
        public NotFoundExeption(string message) :base(message) { } //string mesajı exceptionun mesajına yollar.
       
    }
}
