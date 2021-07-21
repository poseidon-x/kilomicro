using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreErpPointOfSale.Models;

namespace coreErpPointOfSale.ViewModel
{
    public class CustomersViewModel
    {
        //private ObservableCollection<Customer> customers;

        public static ObservableCollection<Customer> GetCustomer()
        {
                    ObservableCollection<Customer> customers = new ObservableCollection<Customer>();
                    customers.Add(new Customer("Cash Customer"));
                
                return customers;
           
        }
    }

}
