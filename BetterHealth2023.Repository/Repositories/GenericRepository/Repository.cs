﻿using AutoMapper;
using BetterHealthManagementAPI.BetterHealth2023.Repository.DatabaseContext;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using static System.Linq.Queryable;
using static System.Linq.Enumerable;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.GenericRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly BetterHealthManagementContext context;
        private DbSet<T> _entities;
        protected readonly IMapper mapper;

        public Repository(BetterHealthManagementContext context, IMapper mapper)
        {
            this.context = context;
            _entities = context.Set<T>();
            this.mapper = mapper;
        }

        public Repository(BetterHealthManagementContext context)
        {
            this.context = context;
            _entities = context.Set<T>();
        }

        public async Task<T> Get(string id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task<bool> Insert(T entity)
        {
            await _entities.AddAsync(entity);
            await Update();
            return true;
        }

        public async Task<bool> Update()
        {
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<TView> GetViewModel<TView>(string id)
        {
            T results = await _entities.FindAsync(id);
            TView convertResults = mapper.Map<TView>(results);
            return convertResults;
        }

        public async Task<List<TView>> GetAll<TView>()
        {
            List<T> results = await _entities.ToListAsync();
            return results.Select(model => mapper.Map<TView>(model)).ToList();
        }

        public async Task<PagedResult<TView>> GetAllPaging<TView>(PagingRequestBase requestBase)
        {
            int totalRow = await _entities.CountAsync();
            List<T> results = await _entities.Skip((requestBase.pageIndex - 1) * requestBase.pageItems)
                                              .Take(requestBase.pageItems)
                                              .ToListAsync();
            List<TView> convertResults = results.Select(model => mapper.Map<TView>(model)).ToList();
            var pagedResult = new PagedResult<TView>(convertResults, totalRow, requestBase.pageIndex, requestBase.pageItems);
            return pagedResult;
        }

        public async Task<PagedResult<TView>> PagingExistingQuery<TView>(System.Linq.IQueryable<T> query, int pageIndex, int pageItems)
        {
            int totalRow = await query.CountAsync();
            List<T> results = await query.Skip((pageIndex - 1) * pageItems)
                .Take(pageItems).ToListAsync();
            List<TView> convertResults = results.Select(model => mapper.Map<TView>(model)).ToList();
            var pagedResult = new PagedResult<TView>(convertResults, totalRow, pageIndex, pageItems);
            return pagedResult;
        }

        public TOut TransferBetweenTwoModels<TIn, TOut>(TIn model)
        {
            return mapper.Map<TOut>(model);
        }

        public void TransferBetweenTwoModels<TIn, TOut>(ref TOut dbmodel, TIn model)
        {
            dbmodel = mapper.Map<TOut>(model);
            context.ChangeTracker.Clear();
            context.Entry(dbmodel).State = EntityState.Modified;
        }

        public async Task<bool> InsertRange(List<T> entityList)
        {
            await context.AddRangeAsync(entityList);
            await Update();
            return true;
        }

        public async Task<bool> Remove(T entity)
        {
            context.Remove(entity);
            await Update();
            return true;
        }
    }
}
