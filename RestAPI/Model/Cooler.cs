using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Model
{
    public class Cooler
    {
        private int _storage;
        private int _capacity;

        public int Id { get; set; }
        public string Location { get; set; }
        public int Capacity
        {
            get
            {
                return _capacity;
            }
            set
            {
                if (value < 0 /*|| value > 4*/)
                {
                    throw new ArgumentException("Capacity can not be less than 0.");
                }

                else
                    _capacity = value;
            }
        }

        public int Storage
        {
            get
            {
                return _storage;
            }
            set
            {
                if (value < 0 /*|| value > 4*/)
                {
                    throw new ArgumentException("Storage can not be less than 0.");
                }

                //else if (value > _capacity /*|| value > 4*/)
                //{
                //    throw new ArgumentException("Storage can not be more than Capacity.");
                //}

                else
                    _storage = value;
            }
        }

        public int Temp { get; set; }

        public Cooler()
        {

        }

        public Cooler(int id, string location, int capacity, int storage, int temp)
        {
            Id = id;
            Location = location;
            Capacity = capacity;
            Storage = storage;
            Temp = temp;
        }


        /// <summary>
        /// When Capacity == Storage, the Cooler is full
        /// </summary>
        /// <returns></returns>
        public bool CoolerIsFull()
        {
            if (this.Capacity == this.Storage)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// add a single bottle of wine to the cooler
        /// </summary>
        /// <returns></returns>
        public int AddWine()
        {
            this.Storage += 1;

            if (this.Capacity < this.Storage)
            {
                throw new Exception("No more space in cooler.");
            }

            return Storage;
        }

        public string CheckTemp(int temp)
        {
            string info = "";
            if (temp > 15)
            {
                info = "Warning: The temp is greater than 15.";
            }

            if (temp < 5)
            {
                info = "Warning: The temp is less than 5.";
            }

            return info;
        }

    }
}
