using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaltyLogistics.Model
{
    public interface IAccountTypeClient
    {
        long Id { get; }
        bool IsActive { get; }
        bool IsAsset { get; set; }
        bool IsInterestComputed { get; set; }
        string Name { get; set; }

        void Activate();
        void Deactivate();
    }
}
