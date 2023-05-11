﻿using Idea_Database_Interface.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using PagedList;

namespace Idea_Database_Interface.Viewmodels
{
    public class EmprendedoresListViewModel
    {
        public IPagedList<Emprendedores> Emprendedores { get; set;}
        public SelectList FilterOptions { get; set; }
        public int PageCount { get; set; }
        public string SearchedFilter { get; set; }
        public string SearchedString { get; set; }
    }
}
