using BetBookGamingMobile.Models;
using BetBookGamingMobile.StateManagement;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetBookGamingMobile.Commands;

public record SubmitParleyBetSlipCommand(
    UserModel loggedInUser, decimal parleyWagerAmount) : IRequest<BetSlipState>;

