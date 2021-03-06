﻿namespace JobPlatform.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using JobPlatform.Data.Common.Repositories;
    using JobPlatform.Data.Models;
    using JobPlatform.Data.Models.Enums;
    using JobPlatform.Services.Data.Interfaces;
    using JobPlatform.Services.Mapping;

    public class JobsService : IJobService
    {
        private readonly IDeletableEntityRepository<Job> jobRepository;
        private readonly IDeletableEntityRepository<Company> companyRepository;
        private readonly IDeletableEntityRepository<Candidate> candidateRepository;
        private readonly ICandidateService candidateService;

        public JobsService(
            IDeletableEntityRepository<Job> jobRepository,
            IDeletableEntityRepository<Company> companyRepository,
            IDeletableEntityRepository<Candidate> candidateRepository,
            ICandidateService candidateService)
        {
            this.jobRepository = jobRepository;
            this.companyRepository = companyRepository;
            this.candidateRepository = candidateRepository;
            this.candidateService = candidateService;
        }

        public async Task AddJob(
            string companyId,
            string companyEmail,
            string jobTitle,
            string jobCategory,
            string locationCity,
            string jobType,
            string description)
        {
            var company = this.companyRepository.All().FirstOrDefault(x => x.Id == companyId);

            if (company == null)
            {
                return;
            }

            var job = new Job()
            {
            CompanyId = companyId,
            CompanyEmail = companyEmail,
            JobType = Enum.Parse<JobType>(jobType),
            JobCategory = Enum.Parse<JobCategory>(jobCategory),
            LocationCity = Enum.Parse<LocationCity>(locationCity),
            JobTitle = jobTitle,
            Description = description,
            };

            await this.jobRepository.AddAsync(job);
            await this.jobRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllJobs<T>()
        {
            return this.jobRepository.All().To<T>().ToList();
        }

        public IEnumerable<T> GetAllJobsByCompanyId<T>(string id)
        {
            return this.jobRepository.All().Where(x => x.CompanyId == id).To<T>().ToList();
        }

        public async Task<bool> DeleteJobById(string id)
        {
            var job = this.jobRepository.All().FirstOrDefault(x => x.Id == id);
            if (job == null)
            {
                return false;
            }

            this.jobRepository.Delete(job);
            await this.jobRepository.SaveChangesAsync();

            return true;
        }

        public T GetJobById<T>(string id)
        {
            return this.jobRepository.All().Where(x => x.Id == id).To<T>().FirstOrDefault();
        }

        public async Task EditJob(
            string jobId,
            string companyEmail,
            string jobTitle,
            string jobCategory,
            string locationCity,
            string jobType,
            string description)
        {
            var job = this.jobRepository.All().FirstOrDefault(x => x.Id == jobId);

            if (job == null)
            {
                return;
            }

            job.CompanyEmail = companyEmail;
            job.JobType = Enum.Parse<JobType>(jobType);
            job.JobCategory = Enum.Parse<JobCategory>(jobCategory);
            job.LocationCity = Enum.Parse<LocationCity>(locationCity);
            job.JobTitle = jobTitle;
            job.Description = description;

            this.jobRepository.Update(job);
            await this.jobRepository.SaveChangesAsync();
        }

        public async Task<bool> AddCandidate(string jobId, string userId, string cv, string motivationLetter)
        {
            var job = this.jobRepository.All().FirstOrDefault(x => x.Id == jobId);
            var candidateId = this.candidateService.AddCandidate(cv, motivationLetter, userId).Result;

            if (job != null && candidateId != null)
            {
                var jobCandidate = new JobCandidate()
                {
                    CandidateId = candidateId,
                    JobId = job.Id,
                };

                job.Candidates.Add(jobCandidate);
                await this.jobRepository.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}
