using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCS.Core.Domains.CodeSharing;
using Xunit;

namespace VMCS.Core.Tests.Tests
{
    public class CodeSharingTests
    {
        private readonly ICodeSharing _codeSharing;

        public CodeSharingTests()
        {
            _codeSharing = new CodeSharing();
        }

    }
}
