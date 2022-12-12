using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
     public static  class Automapper<Tsource, TDestination>
    {
        private static Mapper _mapper = new Mapper(new MapperConfiguration(
           cfg => cfg.CreateMap<Tsource, TDestination>()));
        public static List<TDestination> MapList(List<Tsource> soure)
        { 
            var result = soure.Select(x=> _mapper.Map<TDestination>(x)).ToList();
            return result;
        }
        public static TDestination MapClass(Tsource source)
        {
            var result = _mapper.Map<TDestination>(source);
            return result;
        }
    }

}
