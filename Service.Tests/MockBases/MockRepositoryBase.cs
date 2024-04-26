using Domain.Entities;
using Domain.Interfaces.Repositories;
using Moq;

namespace Service.Tests.MockBases
{
    public static class MockRepositoryBase
    {
        public static Mock<TRepo> MockRepo<TRepo, T>(ICollection<T> list)
           where TRepo : class, IRepositoryBase<T>
           where T : EntityBase
        {
            var mock = new Mock<TRepo>();

            mock.Setup(x => x.Add(It.IsAny<T>())).Returns((T x) =>
            {
                list.Add(x);
                return x;
            });

            mock.Setup(x => x.GetById(It.IsAny<Guid>())).Returns((Guid x) => { return list.FirstOrDefault(y => y.Id == x); });

            mock.Setup(x => x.GetAll()).Returns(() => list);

            mock.Setup(x => x.GetAllExpanded()).Returns(() => list);

            mock.Setup(x => x.Update(It.IsAny<T>())).Returns((T Entity) => { return Entity; });

            mock.Setup(x => x.Delete(It.IsAny<T>())).Returns((T Entity) =>
            {
                list.Remove(Entity);
                return !list.Contains(Entity);
            });

            mock.Setup(x => x.Filter(It.IsAny<Func<T, Boolean>>())).Returns((Func<T, bool> x) => { return list.Where(x); });

            return mock;
        }
    }
}
