using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazin.Domain.CurieratModels
{
    [AsChoice]
    public static partial class AWBStates
    {
        public interface IAWBaStates { }

        public record SuccededAWB : IAWBaStates
        {
            public SuccededAWB(AWBModel awb)
            {
                AWB = awb;
            }

            public AWBModel AWB { get; }
        }

        public record FailedAWB : IAWBaStates
        {
            internal FailedAWB(AWBModel awb, string reason)
            {
                AWB = awb;
                Reason = reason;
            }

            public AWBModel AWB { get; }
            public string Reason { get; }
        }

        
    }
}
