using System;
using System.Collections.Generic;
using System.Text;

namespace NTS.Common.Utils
{
    public interface IPasswordUtilsService
    {
        public string CreatePasswordHash();
        public string ComputeHash(string target);
    }
}
