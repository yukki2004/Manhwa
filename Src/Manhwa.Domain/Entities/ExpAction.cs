using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Domain.Entities
{
    public class ExpAction
    {
        public int ExpActionId { get; set; }
        public int IdAct { get; set; } // Mã định danh hành động (VD: 1: Comment, 2: Rate)
        public string Act { get; set; } = null!; // Tên hành động
        public int ExpValue { get; set; } // Số điểm EXP nhận được
    }
}
