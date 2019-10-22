using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuitarManagement.Enums
{
    public enum EnumSearch
    {
        [Display(Name = "Tìm kiếm theo Id")]
        SearchById,
        [Display(Name = "Tìm kiếm theo tên")]
        SearchByName
    }
}
