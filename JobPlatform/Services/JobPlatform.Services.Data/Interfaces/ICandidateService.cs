﻿namespace JobPlatform.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICandidateService
    {
        Task<string> AddCandidate(string cv, string motivationLetter, string userId);

        IEnumerable<T> GetAllCandidates<T>();

        IEnumerable<T> GetAllCandidatesByCompanyId<T>(string id);

        T GetCandidateByUserId<T>(string id);

        T GetCandidateById<T>(string id);
    }
}
