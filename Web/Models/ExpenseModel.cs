using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class ExpenseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public string Description { get; set; }

        public CategoryModel Cat { get; set; }

        public Double Amount { get; set; }

    }
}