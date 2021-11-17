using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageFilter
{
    class FiltersCreator
    {
        private String[] filters = {"first", "second", "third", "fourth"};

        public FilterCell[] create(Bitmap original)
        {
            FilterCell[] res = new FilterCell[filters.Length];
            for(int i = 0; i< filters.Length; i++)
            {
                res[i] = new FilterCell(filters[i], original);
            }
            Console.WriteLine("Length of res" + res.Length);
            return res;
        }
    }
}
