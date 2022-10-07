using BetBookGamingMobile.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetBookGamingMobile.Queries;

public record GetBettorSingleBetsQuery(string userId) : IRequest<IEnumerable<SingleBetModel>>;

