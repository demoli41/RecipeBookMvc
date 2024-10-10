﻿using RecipeBookMvc.Repositories.Abstract;
using RecipeBookMvc.Models.Domain;

namespace RecipeBookMvc.Repositories.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly DatabaseContext ctx;
        public CategoryService(DatabaseContext ctx)
        {
                this.ctx = ctx;
        }
        public bool Add(Category model)
        {
            try
            {
                ctx.Category.Add(model);
                ctx.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var data = this.GetById(id);
                if (data == null) 
                    return false;
                ctx.Category.Remove(data);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Category GetById(int id)
        {
            return ctx.Category.Find(id);
        }

        public IQueryable<Category> List()
        {
           var data = ctx.Category.AsQueryable();
            return data;
        }

        public bool Update(Category model)
        {
            try
            {
                ctx.Category.Update(model);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
