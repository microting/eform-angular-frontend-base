﻿/*
The MIT License (MIT)

Copyright (c) 2007 - 2021 Microting A/S

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

namespace Microting.EformAngularFrontendBase.Infrastructure.Data.Seed.SeedItems
{
    using System.Linq;
    using Const;
    using Entities.Menu;
    using Microsoft.EntityFrameworkCore;

    public static class MenuItemSeed
    {
        public static ModelBuilder AddDefaultMenu(this ModelBuilder modelBuilder)
        {
            var entities = DefaultMenuStorage.GetDefaultMenu().Select(x => new MenuItem
            {
                Id = x.Id,
                Name = x.Name,
                Link = x.Link,
                E2EId = x.E2EId,
                MenuTemplateId = x.MenuTemplateId,
                Type = x.Type,
                Position = x.Position,
                ParentId = x.ParentId,
                IsInternalLink = true
            });

            modelBuilder.Entity<MenuItem>().HasData(entities);
            return modelBuilder;
        }
    }
}