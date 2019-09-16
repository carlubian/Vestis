using System;
using System.Collections.Generic;
using System.Text;

namespace Vestis.Core.Failures
{
    public class MissingOrUnknownUserFailure : IFailure
    {
        public string FailureKey => "MissingOrUnknownUser";

        public string InternalData => $"Username: {_user}";

        private readonly string _user;

        internal MissingOrUnknownUserFailure(string user)
        {
            _user = user;
        }
    }
}
