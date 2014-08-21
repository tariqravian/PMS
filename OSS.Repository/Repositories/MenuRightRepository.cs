﻿using System.Data.Entity;
using System.Linq;
using Microsoft.Practices.Unity;
using OSS.Interfaces.Repository;
using OSS.Models.MenuModels;
using OSS.Repository.BaseRepository;

namespace OSS.Repository.Repositories
{
    /// <summary>
    /// Menu Repository
    /// </summary>
    public sealed class MenuRightRepository : BaseRepository<MenuRight>, IMenuRightRepository
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public MenuRightRepository(IUnityContainer container)
            :base(container)
        {

        }
        /// <summary>
        /// Primary database set
        /// </summary>
        protected override IDbSet<MenuRight> DbSet
        {
            get { return db.MenuRights; }
        }
        #endregion

        /// <summary>
        /// Get Menu items by role id
        /// </summary>
        public IQueryable<MenuRight> GetMenuByRole(string roleId)
        {
            return
                DbSet.Where(menu => menu.Role.Id == roleId)
                    .Include(menu => menu.Menu)
                    .Include(menu => menu.Menu.ParentItem)                    
                    .Include(menu => menu.Role);
        }
    }
}