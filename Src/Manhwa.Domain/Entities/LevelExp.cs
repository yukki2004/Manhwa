using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Entities
{
    public class LevelExp
    {
        public int LevelExpId { get; set; }
        public int Level { get; set; } // Cấp độ (VD: 1, 2, 3...)
        public int ExpValue { get; set; } // Tổng EXP cần để đạt/duy trì level này
    }
}
