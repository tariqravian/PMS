﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Practices.Unity;
using OSS.Interfaces.Repository;
using OSS.Models.Common;
using OSS.Models.DomainModels;
using OSS.Models.RequestModels;
using OSS.Models.ResponseModels;
using OSS.Repository.BaseRepository;


namespace OSS.Repository.Repositories
{
    public sealed class BuildingRepository : BaseRepository<Building>, IBuildingRepository
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public BuildingRepository(IUnityContainer container)
            : base(container)
        {

        }

        /// <summary>
        /// Primary database set
        /// </summary>
        protected override IDbSet<Building> DbSet
        {
            get { return db.Buildings; }
        }

        #endregion
        #region Private
        /// <summary>
        /// Order by Column Names Dictionary statements - for Product
        /// </summary>
        private readonly Dictionary<BuildingByColumn, Func<Building, object>> buildingClause =
              new Dictionary<BuildingByColumn, Func<Building, object>>
                    {
                        { BuildingByColumn.Name, c => c.Name },
                        { BuildingByColumn.PhoneNumber, c => c.PhoneNumber},
                        { BuildingByColumn.Email, c => c.Email },
                        { BuildingByColumn.Address, c => c.Address},
                        { BuildingByColumn.BuiltDate, c => c.BuiltDate},
                        { BuildingByColumn.NoOfFloors, c => c.NoOfFloors},
                        { BuildingByColumn.NoOfElevators, c => c.NoOfElevators}
                    };
        #endregion

        public BuildingResponse GetAllBuildings()
        {
            return new BuildingResponse
                   {
                       Buildings = DbSet.ToList(),
                       TotalCount = DbSet.ToList().Count
                   };
        }
        public Building GetBuildingByName(int id)
        {
            return DbSet.FirstOrDefault(building => building.BuildingId == id);
        }

        public Building FindBuildingById(int buildingId)
        {
            return DbSet.FirstOrDefault(building => building.BuildingId == buildingId);
        }

        public BuildingResponse GetAllBuildings(BuildingSearchRequest buildingSearchRequest)
        {
            int fromRow = (buildingSearchRequest.PageNo - 1) * buildingSearchRequest.PageSize;
            int toRow = buildingSearchRequest.PageSize;
            Expression<Func<Building, bool>> query =
                s => ( s.UserId.Equals(buildingSearchRequest.UserId)) &&
                    (string.IsNullOrEmpty(buildingSearchRequest.PhoneNumber) || s.PhoneNumber.Contains(buildingSearchRequest.PhoneNumber))&& 
                    (string.IsNullOrEmpty(buildingSearchRequest.SearchString) || s.Name.Contains(buildingSearchRequest.SearchString));
            IEnumerable<Building> buildings = buildingSearchRequest.IsAsc ? DbSet.Where(query).OrderBy(buildingClause[buildingSearchRequest.BuildingOrderBy]).Skip(fromRow).Take(toRow).ToList()
                                           : DbSet.Where(query).OrderByDescending(buildingClause[buildingSearchRequest.BuildingOrderBy]).Skip(fromRow).Take(toRow).ToList();
            return new BuildingResponse { Buildings= buildings, TotalCount = DbSet.Count(query) };
        }
    }
}