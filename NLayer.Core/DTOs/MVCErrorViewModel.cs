using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.DTOs
{
    //mvc tarafında dto demiyoruz da view model diyoruz.
    public class MVCErrorViewModel 
    {
        public List<string> Errors { get; set; } = new List<string>();
    }
}
