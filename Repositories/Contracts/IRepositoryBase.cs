using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IRepositoryBase<T>
    {
        //CRUD R AŞAMASI
        IQueryable<T> FindAll(bool trackChanges); //değişiklikleri izliyoruz, değişiklikleri izlemek hız ve performans kaybına neden olur, optimize edilecek.
        IQueryable<T> FindByCondition(Expression<Func<T,bool>> expression, bool trackChanges); //bir koşula bağlı olarak listelemek için
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
