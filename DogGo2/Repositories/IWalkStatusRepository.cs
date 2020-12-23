using DogGo2.Models;
using System.Collections.Generic;

namespace DogGo2.Repositories
{
    public interface IWalkStatusRepository
    {
        List<WalkStatus> GetAll();
        WalkStatus GetWalkStatusById(int id);
    }
}