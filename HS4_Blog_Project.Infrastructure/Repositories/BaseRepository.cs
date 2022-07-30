using HS4_Blog_Project.Domain.Entities;
using HS4_Blog_Project.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HS4_Blog_Project.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, IBaseEntity
    {
        //where T :class,IBaseEntity yaparak IBaseEntity den implement almayalar classar buraya giremez.

        /*
         CRUD IŞLEMLERİNİ YAPACAĞIMIZ METOTLARI BARINDIRDIĞIMIZ CLASS'IMIZ
        Bu metodlar muhakkak DbSet yani veri tabanındaki varlıkları, uygulama tarafındaki yani application tarafındaki karşılarına ve ORM gereği 
        veri tabanının uygulama tarafındaki karşılığı olan context nesnemize uğramak zorunda.

        Bağımlılığa  neden olan AppDbContext nesnemi Injext ediyorum.

         
         
         */

        private readonly AppDbContext _appDbContext;
        protected DbSet<T> table;

        public BaseRepository(AppDbContext appDbContext) //EF(AppDbContex)
        {
            //SEn ister lassa ister opel lastik gönder bana fark etmez anlamında düşün tipi önemli değil ister comment ister post ister like yollasın.
            //Burası injection.
            _appDbContext = appDbContext;
            table = _appDbContext.Set<T>(); //database hangi tipe bürünmüşse T olarak buraya atıyoruz.Table ın DbSet'ine atıyoruz.Buradaki metotlar tüm entityler  için ortak bir şekilde çalışacağı için bunları yapıyoruz. Dependenct Injection yaptık.
        }

        //AppDbContext bizim contextimiz burada private yaprak burda kullanmak istedik.

        public async Task<bool> Any(Expression<Func<T, bool>> expression)
        {
            return await table.AnyAsync(expression);
        }

        public async Task Create(T entity)
        {
            table.Add(entity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            await _appDbContext.SaveChangesAsync(); //Service katmanında entity'sine göre pasif hale getireceğiz.
        }

        public async Task<T> GetDefault(Expression<Func<T, bool>> expression)
        {
            return await table.FirstOrDefaultAsync(expression);
        }

        public async Task<List<T>> GetDefaults(Expression<Func<T, bool>> expression)
        {
            return await table.Where(expression).ToListAsync();
        }

        public async Task<TResult> GetFilteredFirstOrDefoult<TResult>(
                        Expression<Func<T, TResult>> select,
                        Expression<Func<T, bool>> where, 
                        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
                        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = table; //Select * From Post

            if (where != null)
                query = query.Where(where); //Select * From Post where Status=1

            if (include != null)
                query = include(query);

            if (orderBy != null)
                return await orderBy(query).Select(select).FirstOrDefaultAsync();
            else
                return await query.Select(select).FirstOrDefaultAsync();
        }

        public async Task<List<TResult>> GetFilteredList<TResult>(
                        Expression<Func<T, TResult>> select, 
                        Expression<Func<T, bool>> where, 
                        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
                        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = table; //Select * From Post

            if (where != null)
                query = query.Where(where); //Select * From Post where Status=1

            if (include != null)
                query = include(query);

            if (orderBy != null)
                return await orderBy(query).Select(select).ToListAsync();
            else
                return await query.Select(select).ToListAsync();
        }

        public async Task Update(T entity)
        {
            _appDbContext.Entry<T>(entity).State = EntityState.Modified;
            await _appDbContext.SaveChangesAsync();
        }
    }
}
