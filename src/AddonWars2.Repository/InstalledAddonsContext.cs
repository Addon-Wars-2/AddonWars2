// ==================================================================================================
// <copyright file="InstalledAddonsContext.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================


namespace AddonWars2.Repository
{
    using AddonWars2.LibraryManager.Models;
    using AddonWars2.Repository.Models;
    using Microsoft.EntityFrameworkCore;

    public class InstalledAddonsContext : DbContext
    {
        #region Constructors

        public InstalledAddonsContext(DbContextOptions contextOptions)
            : base(contextOptions)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        #endregion Constructors

        #region Properties

        public DbSet<InstalledAddon> Addon { get; set; } = null!;

        public DbSet<InstalledAddonFile> AddonFile { get; set; } = null!;

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        #endregion Methods
    }
}
