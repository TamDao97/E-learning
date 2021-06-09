using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Entities
{
    /// <summary>
    /// Chuyên gia hiển thị trang chủ font end
    /// </summary>
    public class EmployeeSpecialist
    {
        /// <summary>
        /// Id 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Trang chủ chuyên gia
        /// </summary>
        public int HomeSpecialistId { get; set; }

        /// <summary>
        /// Chuyên gia
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Ảnh chuyên gia
        /// </summary>
        public string Avartar { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
        public string Facebook { get; set; }
        public string Lotus { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
    }
}
