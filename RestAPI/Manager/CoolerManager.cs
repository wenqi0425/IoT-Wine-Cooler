using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary;

namespace RestAPI.Manager
{
    public class CoolerManager
    {
        private static int _id = 1;
        private static List<Cooler> _coolers = new List<Cooler>()
        {
            new Cooler(_id++, "Copenhagen",4,4, 18),
            new Cooler(_id++, "Amarge",4,0, 12),
            new Cooler(_id++, "FSB",3,1, 12),
            new Cooler(_id++, "Husum",48,44, 10),
            new Cooler(_id++, "Odense",124,100, 14)
        };

        public IEnumerable<Cooler> GetAllCoolers()
        {
            return _coolers;
        }

        public Cooler GetCoolerById(int id)
        {
            // find cooler where cooler.Id == id,  => can be regarded as where.
            var cooler = _coolers.Find(cooler => cooler.Id == id);
            return cooler;
        }

        public List<Cooler> GetByLocation(string substring)
        {
            //creating a copy of list, and filtering on it according to lambda expression

            _coolers.Remove(null);
            List<Cooler> results = new List<Cooler>(_coolers);

            if (substring != null && substring.Length != 0)
            {
                results = results.FindAll(result => result.Location.Contains(substring, StringComparison.OrdinalIgnoreCase));
            }
            return results;
        }

        public Cooler AddCooler(Cooler cooler)
        {
            // in the List, _id++ and return to _id, so _id is alwayse the biggest one. 
            cooler.Id = _id;
            _id++;
            _coolers.Add(cooler);
            return cooler;
        }

        public List<Cooler> AddWineToCooler(int id)
        {
            Cooler cooler = GetCoolerById(id);
            if (cooler == null)
            {
                return null;
            }
            cooler.AddWine();
            UpdateCooler(id, cooler);
            return _coolers;
        }

        public Cooler UpdateCooler(int id, Cooler cooler)
        {
            var updated = GetCoolerById(id);
            if (updated == null)
            {
                return null;
            }
            updated.Location = cooler.Location;
            updated.Capacity = cooler.Capacity;
            updated.Storage = cooler.Storage;
            updated.Temp = cooler.Temp;
            return updated;
        }

        public List<Cooler> FilterByCapacity(int capacity)
        {
            _coolers.Remove(null);
            var filterList = new List<Cooler>();
            foreach (Cooler cooler in _coolers)
            {
                if (cooler.Capacity < capacity)
                {
                    filterList.Add(cooler);
                }                
            }

            return filterList;
        }

        public Cooler DeleteCooler(int id)
        {
            var cooler = GetCoolerById(id);
            if (cooler == null)
            {
                return null;
            }
            _coolers.Remove(cooler);
            _id--;
            return cooler;
        }
    }
}
