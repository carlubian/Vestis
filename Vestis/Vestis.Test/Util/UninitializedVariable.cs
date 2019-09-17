using System;
using System.Collections.Generic;
using System.Text;
using Vestis.Core.Failures;

namespace Vestis.Test.Util
{
    public class UninitializedVariable : IFailure
    {
        public string FailureKey => "UninitializedVariable";

        public string InternalData => "This variable was not initialized.";
    }
}
