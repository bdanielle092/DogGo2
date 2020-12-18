using DogGo2.Models;
using System.Collections.Generic;

namespace DogGo2.Repositories
{
    public interface INeighborhoodRepository
    {
        List<Neighborhood> GetAll();
    }
}