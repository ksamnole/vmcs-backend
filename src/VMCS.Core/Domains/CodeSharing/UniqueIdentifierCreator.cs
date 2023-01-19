using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMCS.Core.Domains.CodeSharing
{
    public class UniqueIdentifierCreator
    {
        private int id;

        public int GetUniqueIdentifier()
        {
            return id++;
        } 
    }
}
