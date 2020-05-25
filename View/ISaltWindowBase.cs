using SaltyLogistics.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaltyLogistics.View
{
    interface ISaltWindowBase
    {
        void InitWindow(OpenWindowAction Action, Object Params);
        void CloseWindow();
    }
}
