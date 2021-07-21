using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace coreLogic
{
    class CommonBO
    {
        public IQueryable<currencies> GetCurrencies()
        {
            core_dbEntities ent = new core_dbEntities();
            return (from e in ent.currencies  select e);
        }
        public void UpdateCurrency(int currency_id, string major_name, string minor_name,
            string major_symbol, string minor_symbol, string last_modifier)
        {
            currencies cur = new currencies();
            cur.currency_id = currency_id;
            cur.major_name = major_name;
            cur.minor_name = minor_name;
            cur.major_symbol = major_symbol;
            cur.minor_symbol = minor_symbol;
            cur.last_modifier = last_modifier;
            cur.modification_date = DateTime.Now;
            core_dbEntities ent = new core_dbEntities();
            ent.currencies.Attach(cur);
            ent.SaveChanges();
        }
        public void InsertCurrency(string major_name, string minor_name,
            string major_symbol, string minor_symbol, string creator)
        {
            currencies cur = new currencies(); 
            cur.major_name = major_name;
            cur.minor_name = minor_name;
            cur.major_symbol = major_symbol;
            cur.minor_symbol = minor_symbol;
            cur.creator = creator;
            cur.creation_date = DateTime.Now;
            core_dbEntities ent = new core_dbEntities();
            ent.currencies.Add(cur);
            ent.SaveChanges();
        }
        public void DeleteCurrency(int currency_id)
        {
            currencies cur = new currencies();
            cur.currency_id = currency_id;
            core_dbEntities ent = new core_dbEntities();
            ent.currencies.Remove(cur);
            ent.SaveChanges();
        }
    }
}
