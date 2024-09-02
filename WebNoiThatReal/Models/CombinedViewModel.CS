using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace WebNoiThatReal.Models
{
    public class CombinedViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Event> Events { get; set; }
    }
}
