using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RestAPI.CoolerDbContext;
using RestAPI.Model;

namespace RestAPI.Manager
{
    public class CoolerDbManager
    {
        public CoolerContext _context;

        public CoolerDbManager(CoolerContext context)
        {
            _context = context;
        }

        public IEnumerable<Cooler> GetAll()
        {
            // Linq
            IEnumerable<Cooler> items = from item in _context.Coolers
                                        select item;
            return items;
        }

        public Cooler GetById(int id)
        {
            return _context.Coolers.SingleOrDefault(i => i.Id == id);
        }

        public IEnumerable<Cooler> GetByLocation(string location)
        {
            // linq for filter
            IEnumerable<Cooler> items = from item in _context.Coolers
                                        where item.Location == location
                                        select item;
            return items;
        }

        public IEnumerable<Cooler> OrderByCapacity(int capacity)
        {
            // linq for Ordering
            IEnumerable<Cooler> items = from item in _context.Coolers
                                        where item.Capacity > 20
                                        orderby item.Capacity ascending
                                        select item;
            return items;
        }

        /*
        public IEnumerable<Cooler> GetByDirection(string contains = "")
        {
            // filter string
            if (string.IsNullOrWhiteSpace(contains))
            {
                return _context.Coolers.ToList();
            }
            IEnumerable<Cooler> items = from item in _context.Coolers
                                        where item.Direction.Contains(contains)
                                      select item;
            return items;
        }
        */

        public Cooler Add(Cooler newCooler)
        {
            newCooler.Id = 0; // to ignore the ID supplied from the user
            _context.Coolers.Add(newCooler);
            _context.SaveChanges();
            return newCooler;
        }

        public Cooler DeleteById(int id)
        {
            var delete = GetById(id);
            _context.Remove(delete);
            return delete;
        }

        public void DeleteAll()
        {
            _context.Set<Cooler>().RemoveRange(_context.Set<Cooler>());
            _context.SaveChanges();
        }

        public int UpdateById(int id, Cooler cooler) // how many has been changed, -1 returns error
        {
            Cooler updated = GetById(id);
            updated.Location = cooler.Location;
            updated.Capacity = cooler.Capacity;
            updated.Storage = cooler.Storage;
            updated.Temp = cooler.Temp;
            _context.Coolers.Update(updated);
            return _context.SaveChanges();
        }

        // for checking
        public int Update(Cooler updates) // how many has been changed, -1 returns error
        {
            _context.Coolers.Update(updates);
            return _context.SaveChanges();
        }
    }
}
