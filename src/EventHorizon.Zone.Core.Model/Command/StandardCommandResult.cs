﻿namespace EventHorizon.Zone.Core.Model.Command
{
    public class StandardCommandResult
        : CommandResult<VoidResult>
    {
        public StandardCommandResult()
            : base(VoidResult.DEFAULT)
        {
        }

        public StandardCommandResult(
            bool success
        ) : base(success, VoidResult.DEFAULT)
        {
        }

        public StandardCommandResult(
            string errorCode
        ) : base(errorCode)
        {
        }
    }

    public struct VoidResult
    {
        public static VoidResult DEFAULT = new VoidResult(); 
    }
}
