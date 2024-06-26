﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FindKočka.Models;

namespace FindKočka.Data
{
    public class FindKočkaContext : DbContext
    {
        public FindKočkaContext(DbContextOptions<FindKočkaContext> options)
            : base(options)
        {
        }

        public DbSet<Owner> Owners { get; set; }

        public DbSet<Cat> Cats { get; set; }
    }
}
