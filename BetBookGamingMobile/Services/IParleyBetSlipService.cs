﻿
using BetBookGamingMobile.Models;

namespace BetBookGamingMobile.Services
{
    public interface IParleyBetSlipService
    {
        Task<bool> CreateParleyBet(ParleyBetSlipModel parleyBet);
    }
}