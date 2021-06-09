using Elearning.Model.Entities;
using Elearning.Model.Models.SystemParam;
using Elearning.Models.Entities;
using Microsoft.EntityFrameworkCore;
using NTS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elearning.Services.SystemParams
{
    public class SystemParamService : ISystemParamService
    {
        private readonly ElearningContext sqlContext;
        public SystemParamService(ElearningContext sqlContext)
        {
            this.sqlContext = sqlContext;
        }

        /// <summary>
        /// Lấy danh sách thông số hệ thống
        /// </summary>
        /// <returns></returns>
        public async Task<List<SystemParamModel>> GetListSystemParamAsync()
        {
            var data = await (from a in sqlContext.SystemParams.AsNoTracking()
                              orderby a.Index
                              select new SystemParamModel
                              {
                                  SystemParamId = a.SystemParamId,
                                  ParamName = a.ParamName,
                                  ParamValue = a.ParamValue,
                                  DisplayName = a.DisplayName,
                                  Index = a.Index,
                                  ControlType = a.ControlType
                              }).ToListAsync();

            return data;
        }

        /// <summary>
        /// Cập nhật thông số hệ thống
        /// </summary>
        /// <returns></returns>
        public async Task UpdateSystemParamAsync(List<SystemParamModel> listSystemParam)
        {
            var list = sqlContext.SystemParams.ToList();
            SystemParam systemParam = new SystemParam();
            foreach (var item in listSystemParam)
            {
                systemParam = new SystemParam();
                systemParam = list.FirstOrDefault(i => i.SystemParamId.Equals(item.SystemParamId));
                if (systemParam == null)
                {
                    throw NTSException.CreateInstance($"{systemParam.DisplayName} không tồn tại!");
                }
                systemParam.ParamValue = item.ParamValue;
            }

            await sqlContext.SaveChangesAsync();
        }
    }
}
