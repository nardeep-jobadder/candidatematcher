﻿using CandidateMatcher.Data;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.SessionStorage;

namespace CandidateMatcher.Services
{
    public class JobService : IJobService
    {
        private readonly HttpClient _httpClient;
        private readonly ISessionStorageService _session;
        private readonly IAppDataService _appData;
        public JobService(HttpClient httpClient, ISessionStorageService session, IAppDataService appDataService)
        {
            _httpClient = httpClient;
            _session = session;
            _appData = appDataService;
        }
       public  async Task<IEnumerable<Job>> GetJobs()
        {
            IEnumerable<Job> jobs = await _session.GetItemAsync<IEnumerable<Job>>("JobsList");
            if (jobs == null)
            {
               //AppDataService appData = new AppDataService(_httpClient,_session);
                var result = await  _appData.InitData();
                jobs = result.Item1;
               // jobs = await _session.GetItemAsync<IEnumerable<Job>>("JobsList");

            }
            return jobs;
        }

        public  async Task<IEnumerable<Candidate>> GetTopCandidates(string skills, int topCandidates)
        {
          JobCandidatesMatcher jobMatcher = new JobCandidatesMatcher(_session);
            return await jobMatcher.GetTop(skills, topCandidates);
        }

    }
}
