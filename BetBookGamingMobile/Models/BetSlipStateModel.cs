using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetBookGamingMobile.Models;

public class BetSlipStateModel
{
    public List<CreateBetModel> BetsInBetSlip { get; set; } = new();
}
