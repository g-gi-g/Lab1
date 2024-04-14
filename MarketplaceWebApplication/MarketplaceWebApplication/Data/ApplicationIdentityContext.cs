﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace MarketplaceWebApplication.Data.Identity
{
    public class ApplicationIdentityContext :
IdentityDbContext<ApplicationUser>
    {
        public ApplicationIdentityContext(DbContextOptions<ApplicationIdentityContext> options)
 : base(options)
        {
        }
    }
}
