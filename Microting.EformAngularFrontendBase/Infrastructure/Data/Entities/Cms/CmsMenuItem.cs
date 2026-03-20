/*
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

namespace Microting.EformAngularFrontendBase.Infrastructure.Data.Entities.Cms
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using eFormApi.BasePn.Infrastructure.Database.Base;

    public class CmsMenuItem : BaseEntity
    {
        public int CmsMenuId { get; set; }

        public int? ParentId { get; set; }

        [Required]
        [StringLength(300)]
        public string Title { get; set; }

        public int? PageId { get; set; }

        [StringLength(2000)]
        public string ExternalUrl { get; set; }

        [StringLength(20)]
        public string Target { get; set; } = "_self";

        public int Order { get; set; }

        public virtual CmsMenu Menu { get; set; }

        public virtual CmsMenuItem Parent { get; set; }

        public virtual List<CmsMenuItem> Children { get; set; } = new List<CmsMenuItem>();

        public virtual CmsPage Page { get; set; }
    }
}
