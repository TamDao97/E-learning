using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Services.Maper
{
    public static class AutoMaper
    {
        public static IMapper Mapper { get; private set; }
        public static void Init()
        {
            var config = new MapperConfiguration(cf =>
            {

                //cf.CreateMap<MaterialGroupTPA, MarterialGroupTPAHistoryModel>();

            });

            Mapper = config.CreateMapper();
        }
    }
}
