using MenuApp.Entity.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuApp
{
   public static class Ext
    {
        public static double ToDouble(this string e) => Convert.ToDouble(e);

        public static ObservableCollection<Food> ToObservableCollection(this IEnumerable<Food> e)
        {
            ObservableCollection<Food> t = new ObservableCollection<Food>();
            foreach (var item in e)
            {
                t.Add(item);
            }
            return t;
        }
    }
}
