using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.Entites;
using Microsoft.EntityFrameworkCore;

namespace Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity,TContext>:IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()
    {
        public void Add(TEntity entity)
        {
            //IDispossable pattern implementation of c#
            using (TContext context = new TContext())
            {
                var addedEntity = context.Entry(entity);
                addedEntity.State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public void Delete(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var deletedEntity = context.Entry(entity);
                deletedEntity.State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        //Aşağıdaki 2 metod için verilen uyarılar .NET 6 ve sonrasında gelen nullable reference types özelliğinden kaynaklı. Eğitimde mevcut değil.
        //Bu metodda 2 farklı çözüm mevcut. ilki Interface'de tanımlanan Entity Sınıfının nullable yapılması yani T"?" Get(Expression<Func<T, bool>> filter);
        //İkincisi return context.Set<Product>().SingleOrDefault(filter)"!"; kısmına "!" ekleyerek dönecek olan değişkenin kesinlikle null olmayacağını belirtmek.
        //İkinci çözüm tavsiye edilmez çünkü değişken null dönerse kod runtime error verir.
        public TEntity? Get(Expression<Func<TEntity, bool>> filter)
        {
            using (TContext context = new TContext())
            {
                return context.Set<TEntity>().SingleOrDefault(filter);
            }
        }

        //GetAll(Expression<Func<Product, bool>>"?" filter = null kısmına "?" eklenerek filter değişkeninin null alabileceğini söylemiş olduk.
        public List<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null)
        {
            using (TContext context = new TContext())
            {
                return filter == null
                    ? context.Set<TEntity>().ToList()
                    : context.Set<TEntity>().Where(filter).ToList();
            }
        }

        public void Update(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var updatedEntity = context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
