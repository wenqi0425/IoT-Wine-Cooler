using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class CoolerGenerator
    {
        // static attribute is the same for all of class instances. 
        private static readonly string[] Locations = { "Copenhagen", "Amarge", "FSB", "Odense", "Husum"};  // 5 items
        // private int _capacity = 6;  // can not be static, due to the _ran.Next need the last number.
        private int _locationId = 0;
        private int _capacity = 100;
        private int _storage = 50;
        private int _temp = 5;
        private readonly Random _ran = new Random();  // 使用了readonly的属性，只能在定义时，或者构造函数中初始化，其他的地方都不能再修改其值

        // const: 声明const时，必须要赋值，第一次赋值之后就不能再改变。
        public string CreateLocation()
        {
            /*
            _directionId += _ran.Next(-1, 2);  // -1, 0, 1
            if (_directionId == -1) _directionId = 7;
            if (_directionId == 8) _directionId = 0;
            return Directions[_directionId];
            */
            _locationId = _ran.Next(0, 5);
            if (_locationId >= 5) _locationId = 0;
            return Locations[_locationId];
        }

        public int CreateCapacity()
        {
            _capacity += _ran.Next(-90, 51); // 10 - 150
            if (_capacity <= 0) _capacity = 0;
            return _capacity;
        }

        public int CreateStorage()
        {
            _storage += _ran.Next(-50, 51);  // 0 - 100 
            if (_storage <= 0) _storage = 0;
            return _storage;
        }

        public int CreateTemp()
        {
            _temp += _ran.Next(-1, 2);
            return _temp;
        }
    }
}
