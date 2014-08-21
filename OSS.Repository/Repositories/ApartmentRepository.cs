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
    public sealed class ApartmentRepository : BaseRepository<Apartment>, IApartmentRepository
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ApartmentRepository(IUnityContainer container)
            : base(container)
        {

        }

        /// <summary>
        /// Primary database set
        /// </summary>
        protected override IDbSet<Apartment> DbSet
        {
            get { return db.Apartments; }
        }

        #endregion
        #region Private
        /// <summary>
        /// Order by Column Names Dictionary statements - for Product
        /// </summary>
        private readonly Dictionary<ApartmentsByColumn, Func<Apartment, object>> apartmentClause =
              new Dictionary<ApartmentsByColumn, Func<Apartment, object>>
                    {
                        { ApartmentsByColumn.AppartmentNo, c => c.ApartmentNo},
                        { ApartmentsByColumn.BuildingNo, c => c.BuildingId },
                        { ApartmentsByColumn.NoOfRooms, c => c.NoOfRooms },
                        { ApartmentsByColumn.NoOfRestRoooms, c => c.NoOfRestRoooms},
                        { ApartmentsByColumn.NoOfAlmarah, c => c.NoOfAlmarah},
                        { ApartmentsByColumn.TotalArea, c => c.TotalArea},
                        { ApartmentsByColumn.Comment, c => c.Comment},
                        { ApartmentsByColumn.MasterBedSize, c => c.MasterBedSize},
                        { ApartmentsByColumn.KitchenSize, c => c.KitchenSize},
                        { ApartmentsByColumn.NoOfWindows, c => c.NoOfWindows},
                        { ApartmentsByColumn.NoOfDoors, c => c.NoOfDoors},
                        { ApartmentsByColumn.NoOfWashRooms, c => c.NoOfWashRooms},
                        { ApartmentsByColumn.FloorNumber, c => c.FloorNumber},
                        { ApartmentsByColumn.NoOfElectricalsInstalled, c => c.NoOfElectricalsInstalled},
                        { ApartmentsByColumn.NoOfDoorLocks, c => c.NoOfDoorLocks},
                        { ApartmentsByColumn.SecurityCamerasInstalled, c => c.SecurityCamerasInstalled}
                    };
        #endregion
        public ApartmentResponse GetAllApartments(ApartmentSearchRequest apartmentSearchRequest)
        {
            int fromRow = (apartmentSearchRequest.PageNo - 1) * apartmentSearchRequest.PageSize;
            int toRow = apartmentSearchRequest.PageSize;
            Expression<Func<Apartment, bool>> query =
                s => (s.Building.UserId.Equals(apartmentSearchRequest.UserId)) &&
                     (string.IsNullOrEmpty(apartmentSearchRequest.ApartmentNumber) ||
                      s.ApartmentNo.Contains(apartmentSearchRequest.ApartmentNumber));
                    //&&(string.IsNullOrEmpty(buildingSearchRequest.SearchString) || s.Name.Contains(buildingSearchRequest.SearchString));
            IEnumerable<Apartment> apartments = apartmentSearchRequest.IsAsc ? DbSet.Where(query).OrderBy(apartmentClause[apartmentSearchRequest.ApartmentOrderBy]).Skip(fromRow).Take(toRow).ToList()
                                           : DbSet.Where(query).OrderByDescending(apartmentClause[apartmentSearchRequest.ApartmentOrderBy]).Skip(fromRow).Take(toRow).ToList();
            return new ApartmentResponse { Apartments= apartments, TotalCount = DbSet.Count(query) };
        }
        public Apartment FindApartmentById(int apartmentId)
        {
            return DbSet.FirstOrDefault(apartment => apartment.ApartmentId == apartmentId);
        }
    }
}