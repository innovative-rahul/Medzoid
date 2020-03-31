using MedzoidSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedzoidSite.ViewModel
{
    public class BlogViewModel
    {
        public BlogNew Blog { get; set; }
        public List<BlogNew> BlogList { get; set; }
        public List<RecentBlogs> RecentBloglist { get; set; }
        public List<PopularBlogs> PopularBlogsList { get; set; }
       
    }


    public class RecentBlogs : BlogNew
    {
       
    }

    public class PopularBlogs : BlogNew
    {
      
    }


    public class BlogPostViewModel
    {
        public BlogNew blog { get; set; }
        public List<BlogComment> comments { get; set; }
    }

}