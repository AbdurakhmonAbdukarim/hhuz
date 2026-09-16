namespace hhuz.Mapper;

public interface BaseMapper<T,D>
{
    T ToEntity(D dto);
    D ToDto(T entity);
    List<T>  ToEntities(List<D> dtos);
    List<D>  ToDtos(List<T> dtos);
} 