using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.EmployeeSpecialist
{
    public class EmployeeSpecialistModel
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
        public string Avatar { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
    }
}
