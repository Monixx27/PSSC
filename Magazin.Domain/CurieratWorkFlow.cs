using Magazin.Domain.CurieratModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Magazin.Domain.CurieratModels.AWBStates;

namespace Magazin.Domain
{
    public class CurieratWorkFlow
    {
        static Random rand = new();
        public static AWBModel ExecuteCurieratWorkflow()
        {
            //Get date livrare de la firma de curierat
            var AWB = rand.Next(100, 10000).ToString();
            var DataLivrare = DateTime.Now.AddDays(rand.Next(0, 7));

            return new AWBModel(AWB, DataLivrare);
        }


    }
}
