﻿using DogGo2.Models;
using System.Collections.Generic;

namespace DogGo2.Repositories
{
    public interface IWalkRepository
    {
        List<Walk> GetAllWalks();
        Walk GetWalkById(int id);
        List<Walk> GetWalksByWalkerId(int walkerId);
        void AddWalk(Walk walk);
        void UpdateWalk(Walk walk);
        void DeleteWalk(int walkId);
    }
}